using Godot;
using System.Collections.Generic;

public partial class Human : CharacterBody2D
{
	enum HumanState { Patrolling, Chasing, Paused, Distracted }
	HumanState currentState = HumanState.Chasing;

	NavigationAgent2D navAgent;
	CharacterBody2D dog;
	Marker2D preDoorTarget;
	bool reachedPreDoor = false;
	Marker2D doorTarget;
	Marker2D doorTarget2;
	List<Marker2D> unstuckMarkers = new List<Marker2D>();
	Marker2D lastUnstuckMarker = null;

	float speed = 170f; // safe fallback until initialized
	bool speedInitialized = false;
	float stuckTimer = 0f;
	float stuckThreshold = 0.5f;
	Vector2 lastPosition;

	bool headingToDoor = false;
	bool headingToDoor2 = false;
	bool headingToUnstuck = false;
	Marker2D currentUnstuckTarget = null;

	AnimatedSprite2D humanAnim;

	public override void _Ready()
	{
		navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		dog = GetNode<CharacterBody2D>("../DummyDog");
		lastPosition = GlobalPosition;

		doorTarget = GetNode<Marker2D>("../LivingRoom_Navigation/WP_Door");
		doorTarget2 = GetNode<Marker2D>("../Kitchen_Navigation/WP_Door2");
		preDoorTarget = GetNode<Marker2D>("../LivingRoom_Navigation/WP_PreDoor");

		Area2D doorTrigger = GetNode<Area2D>("../LivingRoom_Navigation/DoorTrigger");
		doorTrigger.BodyEntered += OnDoorAreaEntered;

		Area2D doorTrigger2 = GetNode<Area2D>("../Kitchen_Navigation/DoorTrigger2");
		doorTrigger2.BodyEntered += OnDoorTrigger2Entered;

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
			reachedPreDoor = false;
			GD.Print("Dog entered door, human heading to pre-door marker!");
		}
	}

	private void OnDoorTrigger2Entered(Node2D body)
	{
		if (body.Name == "DummyDog" && !headingToDoor2)
		{
			headingToDoor2 = true;
			GD.Print("Dog entered door 2, human heading to door 2 marker!");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		ZIndex = (int)(GlobalPosition.Y / 10);

		// initialize speed once dogController is ready
		if (!speedInitialized)
		{
			var gameScript = GetNode<RoguelikeMovement>("..");
			if (gameScript.dogController != null)
			{
				speed = gameScript.dogController.dogSpeed * GachaShopUi.GetHumanSpeedMultiplier();
				speedInitialized = true;
				GD.Print("Human speed initialized: " + speed);
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
		Vector2 target;

		// animation
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
			if (!reachedPreDoor)
			{
				target = preDoorTarget.GlobalPosition;
				if (GlobalPosition.DistanceTo(preDoorTarget.GlobalPosition) < 30f)
				{
					reachedPreDoor = true;
					GD.Print("Reached pre-door marker, heading to door!");
				}
			}
			else
			{
				target = doorTarget.GlobalPosition;
				if (GlobalPosition.DistanceTo(doorTarget.GlobalPosition) < 30f)
				{
					headingToDoor = false;
					reachedPreDoor = false;
					GD.Print("Through the door, resuming chase!");
				}
			}
		}
		else if (headingToDoor2)
		{
			
			
			target = doorTarget2.GlobalPosition;
			if (GlobalPosition.DistanceTo(doorTarget2.GlobalPosition) < 30f)
			{
				headingToDoor2 = false;
				GD.Print("Through door 2, resuming chase!");
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

				if (!headingToUnstuck && !headingToDoor && !headingToDoor2)
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
		var game = GetNode<RoguelikeMovement>("..");
		float finalSpeed = speed * game.dogController.humanSpeedModifier;

		Vector2 direction = (nextPoint - GlobalPosition).Normalized() * finalSpeed;
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
