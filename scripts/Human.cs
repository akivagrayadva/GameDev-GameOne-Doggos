using Godot;
using System;

public partial class Human : CharacterBody2D
{
	enum HumanState { Patrolling, Chasing, Paused, Distracted }
	HumanState currentState = HumanState.Chasing;

	NavigationAgent2D navAgent;
	CharacterBody2D dog;

	float speed = 100f;
	float stuckTimer = 0f;
	float stuckThreshold = 0.5f;
	Vector2 lastPosition;

	public override void _Ready()
	{
		navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		dog = GetNode<CharacterBody2D>("../DummyDog");
		lastPosition = GlobalPosition;

		GD.Print("HumanAI Ready!");
		GD.Print("NavAgent: " + navAgent);
		GD.Print("Dog: " + dog);
	}

	public override void _PhysicsProcess(double delta)
	{
		switch (currentState)
		{
			case HumanState.Chasing: HandleChase(delta); break;
			case HumanState.Patrolling: break; // coming soon
			case HumanState.Paused: break;     // coming soon
			case HumanState.Distracted: break; // coming soon
		}
	}

	private void HandleChase(double delta)
	{
		navAgent.TargetPosition = dog.GlobalPosition;

		// check if stuck
		float distanceMoved = GlobalPosition.DistanceTo(lastPosition);
		if (distanceMoved < 1f)
		{
			stuckTimer += (float)delta;
			if (stuckTimer > stuckThreshold)
			{
				navAgent.TargetPosition = dog.GlobalPosition;
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
