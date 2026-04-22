using Godot;
using System;

public partial class RoguelikeMovement : Node2D {
	public static RoguelikeMovement Instance;
	DogController dogController;


public enum DogBreed {
	GoldenRetriever,
	Akita,
	GreatDane,
	Schnauzer,
	SaintBernard,
	SiberianHusky,
	FrostDog
	
}

	// NEW: Dog types
	public enum DogType {
		Balanced,
		Fast,
		Heavy
	}

	// selectable in inspector
	[Export] public DogBreed CurrentDog = DogBreed.FrostDog;
	
	[Signal] public delegate void Room1TreatsCollectedEventHandler();
	[Signal] public delegate void Room2TreatsCollectedEventHandler();

	CharacterBody2D activeDog;
	CharacterBody2D activeHuman;
	AnimatedSprite2D dogAnim;
	Node treatCounter;
	CollisionShape2D dogCollision;
	
	
	public static int TotalTreats= 0;         // to keep track of how many treats player has for the shop
	int totalTreats = 8;
	int livingRoomTreats = 3;
	int kitchenTreats = 7;
	int collectedTreats = 0;
	//bool levelEnded = false;
	
	bool usedRevive = false;
	

	//  turn speed for different dog handling
	//float dogTurnSpeed = 8f;


	public override void _Ready(){
	Instance = this;
	CurrentDog = GachaShopUi.SelectedDog;
			
	activeDog = GetNode<CharacterBody2D>("DummyDog");       // Change to "ActiveDog" when the actual dog scene is ready
	dogAnim = activeDog.GetNode<AnimatedSprite2D>("Anim");
	activeHuman = GetNode<CharacterBody2D>("DummyHuman");   // Change to "ActiveHuman" when the actual human scene is ready
	var goalArea = GetNode<Area2D>("DummyGoal");             // This becomes the collision for the canna-biscuits that end a sucessful round
	var failArea = activeHuman.GetNode<Area2D>("DogDetection");                             // This becomes the collision for the human that ends a failed round
	dogCollision = activeDog.GetNode<CollisionShape2D>("CollisionShape2D");
	
			
	dogAnim = activeDog.GetNode<AnimatedSprite2D>("Anim");
	SetDogSprite(CurrentDog);
	
	dogController = new DogController();
	AddChild(dogController);

	dogController.Setup(
		CurrentDog,
		GLOBAL_CONSTANTS.HUMAN_SPEED_Modifier,
		activeDog,
		activeHuman
	);
	
	
	GD.Print("Current dog: " + CurrentDog);
	
	GD.Print("Ready to run.");
	
	failArea.BodyEntered += (body) => {
		if (body == activeDog)
			OnFailTouched();
	};

	GD.Print("Ready to run.");
}

	public void TreatCollected(){
		collectedTreats++;
		TotalTreats++;
		
		//Opens the first door after all the treats have been collected
		if(collectedTreats == livingRoomTreats){
			EmitSignal(SignalName.Room1TreatsCollected);
		}
		
		//Open the second door after all the treats have been collected
		if(collectedTreats == kitchenTreats) {
			EmitSignal(SignalName.Room2TreatsCollected);
		}
		
		if(collectedTreats == totalTreats){
			OnGoalTouched();
		}
		
	}

private DogType GetDogType(DogBreed dogBreed)
{
	switch (dogBreed)
	{
		case DogBreed.GreatDane:
		case DogBreed.SaintBernard:
			return DogType.Heavy;

		case DogBreed.SiberianHusky:
		case DogBreed.FrostDog:
			return DogType.Fast;

		case DogBreed.GoldenRetriever:
		case DogBreed.Akita:
		case DogBreed.Schnauzer:
		default:
			return DogType.Balanced;
	}
}

private void SetDogSprite(DogBreed dog)
{
	RectangleShape2D rectShape = dogCollision.Shape as RectangleShape2D;
	
	switch (dog)
	{
		case DogBreed.Schnauzer:
			dogAnim.SpriteFrames = GD.Load<SpriteFrames>("res://assets/Pet Dogs Pack/schnauzer_frames.tres");
			dogAnim.Scale = new Vector2(2.5f, 2.5f);
			if (rectShape != null)
				rectShape.Size = new Vector2(40,16);
			dogCollision.Position = new Vector2(0, 0);
			break;

		case DogBreed.GoldenRetriever:
			dogAnim.SpriteFrames = GD.Load<SpriteFrames>("res://assets/Pet Dogs Pack/goldenretriever_frames.tres");
			dogAnim.Scale = new Vector2(3.8f, 3.8f);
			if (rectShape != null)
				rectShape.Size = new Vector2(60, 30);
			dogCollision.Position = new Vector2(0, 3);
			break;

		case DogBreed.Akita:
			dogAnim.SpriteFrames = GD.Load<SpriteFrames>("res://assets/Pet Dogs Pack/akita_frames.tres");
			dogAnim.Scale = new Vector2(4.5f, 4.5f);
			if (rectShape != null)
				rectShape.Size = new Vector2(44, 24);
			dogCollision.Position = new Vector2(0, 3);
			break;

		case DogBreed.GreatDane:
			dogAnim.SpriteFrames = GD.Load<SpriteFrames>("res://assets/Pet Dogs Pack/greatdane_frames.tres");
			dogAnim.Scale = new Vector2(3.3f, 3.3f);
			if (rectShape != null)
				rectShape.Size = new Vector2(70, 35);
			dogCollision.Position = new Vector2(0, 4);
			break;

		case DogBreed.SaintBernard:
			dogAnim.SpriteFrames = GD.Load<SpriteFrames>("res://assets/Pet Dogs Pack/saintbernard_frames.tres");
			dogAnim.Scale = new Vector2(3.0f, 3.0f);
			if (rectShape != null)
				rectShape.Size = new Vector2(75, 35);
			dogCollision.Position = new Vector2(0, 0);
			break;

		case DogBreed.SiberianHusky:
			dogAnim.SpriteFrames = GD.Load<SpriteFrames>("res://assets/Pet Dogs Pack/siberianhusky_frames.tres");
			dogAnim.Scale = new Vector2(3.5f, 3.5f);
			if (rectShape != null)
				rectShape.Size = new Vector2(60, 25);
			dogCollision.Position = new Vector2(0, 0);
			break;
			
			case DogBreed.FrostDog:
			dogAnim.SpriteFrames = GD.Load<SpriteFrames>("res://assets/Pet Dogs Pack/frostdog_frames.tres");
			dogAnim.Scale = new Vector2(2.0f, 2.0f);
			if (rectShape != null)
				rectShape.Size = new Vector2(60, 30);
			dogCollision.Position = new Vector2(0, 3);
			break;
	}

	dogAnim.Stop(); // reset animation
}

	public override void _PhysicsProcess(double delta)
{
	Vector2 inputVector = Vector2.Zero;

	if (Input.IsActionPressed("ui_right"))
		inputVector.X += 1;
	if (Input.IsActionPressed("ui_left"))
		inputVector.X -= 1;
	if (Input.IsActionPressed("ui_down"))
		inputVector.Y += 1;
	if (Input.IsActionPressed("ui_up"))
		inputVector.Y -= 1;

	inputVector = inputVector.Normalized();

	// update abilities
	dogController.Update(delta);

	if (Input.IsActionJustPressed("dog_ability"))
		dogController.TryUseAbility();

	// movement
	dogController.ApplyMovement(inputVector);
	activeDog.MoveAndSlide();

	// animation
	if (inputVector != Vector2.Zero)
	{
		dogAnim.Play("walk");
		dogAnim.FlipH = inputVector.X < 0;
	}
	else
	{
		dogAnim.Stop();
	}

	// human movement
	activeHuman.Velocity =
	humanPathingAlgorithm(activeHuman.Position, activeDog.Position)
	* dogController.dogSpeed
	* dogController.humanSpeedModifier;

	activeHuman.MoveAndSlide();
}
	public void OnGoalTouched() {
		GD.Print("Goal touched! Returning to Gacha Shop...");
		GetTree().ChangeSceneToFile("res://scenes/gacha_shop.tscn");
	}

	public void OnFailTouched() {
		if (CurrentDog == DogBreed.SaintBernard && !usedRevive)
	{
		usedRevive = true;
		GD.Print("Second Chance!");
		return;
	}
		
		GD.Print("You Got Caught! Returning to Gacha Shop...");
		GetTree().ChangeSceneToFile("res://scenes/gacha_shop.tscn");
	}

	private Vector2 humanPathingAlgorithm(Vector2 humanPos, Vector2 dogPos) {
		// Placeholder for a more complex pathfinding algorithm
		return (dogPos - humanPos).Normalized();
	}
	
	
}
