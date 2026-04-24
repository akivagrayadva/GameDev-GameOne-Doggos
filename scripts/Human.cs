using Godot;
using System.Collections.Generic;

public partial class Human : CharacterBody2D
{
	enum HumanState { Patrolling, Chasing, Paused, Distracted }
	HumanState currentState = HumanState.Chasing;

	NavigationAgent2D navAgent;
	CharacterBody2D dog;
	Marker2D doorTarget;
	List<Marker2D> unstuckMarkers = new List<Marker2D>();
	Marker2D lastUnstuckMarker = null;

	float speed = 0f;
	float stuckTimer = 0f;
	float stuckThreshold = 0.5f;
	Vector2 lastPosition;

	bool headingToDoor = false;
	bool headingToUnstuck = false;
	Marker2D currentUnstuckTarget = null;
	AnimatedSprite2D humanAnim;

	public override void _Ready()
	{
		navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		dog = GetNode<CharacterBody2D>("../DummyDog");
		lastPosition = GlobalPosition;

		doorTarget = GetNode<Marker2D>("../LivingRoom_Navigation/WP_Door");

		Area2D doorTrigger = GetNode<Area2D>("../LivingRoom_Navigation/DoorTrigger");
		doorTrigger.BodyEntered += OnDoorAreaEntered;

		// set speed relative to dog speed
		var gameScript = GetNode<RoguelikeMovement>("..");
		speed = gameScript.dogSpeed * 0.85f;
		GD.Print("Human speed set to: " + speed);

		// load unstuck markers
		Node unstuckNode = GetNode("../UnstuckMarkers");
		foreach (Marker2D marker in unstuckNode.GetChildren())
		{
			unstuckMarkers.Add(marker);
		}
		
		humanAnim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		GD.Print("HumanAI Ready!");
		GD.Print("NavAgent: " + navAgent);
		GD.Print("Dog: " + dog);
		GD.Print("Unstuck markers loaded: " + unstuckMarkers.Count);
	}

	private void OnDoorAreaEntered(Node2D body)
	{
		if (body.Name == "DummyDog" && !headingToDoor)
		{
			headingToDoor = true;
			GD.Print("Dog entered door, human heading to door marker!");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
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
		Vector2 target;
		
		humanAnim.Play("walk");

		if (Velocity.Length() > 0)
{
	humanAnim.Play("walk");
	if (Velocity.X < 0)
		humanAnim.FlipH = true;
	else if (Velocity.X > 0)
		humanAnim.FlipH = false;
}
else
{
	humanAnim.Play("idle");
}

		if (headingToDoor)
		{
			target = doorTarget.GlobalPosition;
			if (GlobalPosition.DistanceTo(doorTarget.GlobalPosition) < 30f)
			{
				headingToDoor = false;
				GD.Print("Through the door, resuming chase!");
			}
		}
		else if (headingToUnstuck && currentUnstuckTarget != null)
		{
			target = currentUnstuckTarget.GlobalPosition;
			if (GlobalPosition.DistanceTo(currentUnstuckTarget.GlobalPosition) < 30f)
			{
				headingToUnstuck = false;
				currentUnstuckTarget = null;
				GD.Print("Reached unstuck marker, resuming chase!");
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
				stuckTimer = 0f;

				if (!headingToUnstuck && !headingToDoor)
				{
					currentUnstuckTarget = FindNearestUnstuckMarker();
					if (currentUnstuckTarget != null)
					{
						headingToUnstuck = true;
						GD.Print("Stuck! Heading to unstuck marker: " + currentUnstuckTarget.Name);
					}
				}
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

	private Marker2D FindNearestUnstuckMarker()
	{
		Marker2D nearest = null;
		float shortestDistance = float.MaxValue;

		foreach (Marker2D marker in unstuckMarkers)
		{
			if (marker == lastUnstuckMarker) continue;

			float distance = GlobalPosition.DistanceTo(marker.GlobalPosition);
			if (distance < shortestDistance)
			{
				shortestDistance = distance;
				nearest = marker;
			}
		}

		lastUnstuckMarker = nearest;
		return nearest;
	}
}
