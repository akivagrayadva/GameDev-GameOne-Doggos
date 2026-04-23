using Godot;
using System;

public partial class Human : CharacterBody2D
{
	enum HumanState { Patrolling, Chasing, Paused, Distracted }
	HumanState currentState = HumanState.Chasing;

	NavigationAgent2D navAgent;
	CharacterBody2D dog;
	Marker2D doorTarget;

	float speed = 100f;
	float stuckTimer = 0f;
	float stuckThreshold = 0.5f;
	Vector2 lastPosition;

	bool headingToDoor = false;
	bool doorDelayActive = false;
	float doorDelayTimer = 0f;
	float doorDelayDuration = 3f;

	public override void _Ready()
	{
		navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		dog = GetNode<CharacterBody2D>("../DummyDog");
		lastPosition = GlobalPosition;

		doorTarget = GetNode<Marker2D>("../../Markers/LivingRoom/Door/WP_Door1");

		var gameScript = GetNode<RoguelikeMovement>("..");
		gameScript.Room1TreatsCollected += OnRoom1Complete;

		GD.Print("HumanAI Ready!");
		GD.Print("NavAgent: " + navAgent);
		GD.Print("Dog: " + dog);
	}

	private void OnRoom1Complete()
	{
		doorDelayActive = true;
		doorDelayTimer = 0f;
		GD.Print("Room 1 complete, heading to door in 3 seconds!");
	}

	public override void _PhysicsProcess(double delta)
	{
		// count down delay timer
		if (doorDelayActive)
		{
			doorDelayTimer += (float)delta;
			if (doorDelayTimer >= doorDelayDuration)
			{
				doorDelayActive = false;
				headingToDoor = true;
				GD.Print("Now heading to door!");
			}
		}

		switch (currentState)
		{
			case HumanState.Chasing: HandleChase(delta); break;
			case HumanState.Patrolling: break;
			case HumanState.Paused: break;
			case HumanState.Distracted: break;
		}
	}

	private void HandleChase(double delta)
	{
		// decide target based on state
		Vector2 target;
		if (headingToDoor)
		{
			target = doorTarget.GlobalPosition;

			// once through door resume chasing
			if (GlobalPosition.DistanceTo(doorTarget.GlobalPosition) < 30f)
			{
				headingToDoor = false;
				GD.Print("Through the door, resuming chase!");
			}
		}
		else
		{
			target = dog.GlobalPosition;
		}

		navAgent.TargetPosition = target;

		// stuck detection
		float distanceMoved = GlobalPosition.DistanceTo(lastPosition);
		if (distanceMoved < 1f)
		{
			stuckTimer += (float)delta;
			if (stuckTimer > stuckThreshold)
			{
				navAgent.TargetPosition = target;
				stuckTimer = 0f;
				GD.Print("Stuck! Recalculating path");
			}
		}
		else
		{
			stuckTimer = 0f;
		}

		lastPosition = GlobalPosition;

		Vector2 nextPoint = navAgent.GetNextPathPosition();
		Vector2 direction = (nextPoint - GlobalPosition).Normalized() * speed;
		Velocity = direction;
		MoveAndSlide();
	}
}
