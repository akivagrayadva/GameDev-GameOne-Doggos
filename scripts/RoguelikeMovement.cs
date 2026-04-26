using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class RoguelikeMovement : Node2D {
	public static RoguelikeMovement Instance;
public DogController dogController;
Random rand = new Random();
HashSet<Marker2D> usedMarkers = new HashSet<Marker2D>();
private bool hasSpawned = false;

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
	[Signal] public delegate void Room3TreatsCollectedEventHandler();
	
	CharacterBody2D activeDog;
	CharacterBody2D activeHuman;
	AnimatedSprite2D dogAnim;
	Node treatCounter;
	CollisionShape2D dogCollision;
	
	public static int LastSelectedDogIndex = 0;
	
	public static float TotalTreats = 0;         // to keep track of how many treats player has for the shop
	public static float PremiumTreats = 0;
	
	public static DogBreed[] OwnedDogs =
	{
		DogBreed.GoldenRetriever
	};
	
	int totalTreats = 12;
	int livingRoomTreats = 4;
	int kitchenTreats = 8;
	int backyardTreats = 12;
	int collectedTreats = 0;
	int premiumCollected = 0;
	//bool levelEnded = false;
	
		
	public float dogSpeed = GLOBAL_CONSTANTS.DOG_SPEED;

	//  turn speed for different dog handling
	//float dogTurnSpeed = 8f;


	public override void _Ready(){
	
	

	GD.Print("SPAWNING START");
	if (hasSpawned) return;
	hasSpawned = true;

	
	Instance = this;
	usedMarkers.Clear();
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
	totalTreats = 0;
	
	int premiumLeft = (int)GD.RandRange(1, 3); // 1–2 per run

	var spawns = GetNode<Node>("TreatSpawns");

	SpawnTreatsInArea(spawns.GetNode("LivingRoom"), 4);
	GD.Print("After LivingRoom - totalTreats: " + totalTreats);
	SpawnTreatsInArea(spawns.GetNode("Kitchen"), 4);
	GD.Print("After Kitchen - totalTreats: " + totalTreats);
	SpawnTreatsInArea(spawns.GetNode("Backyard"), 4);
	 GD.Print("After Backyard - totalTreats: " + totalTreats);
	
	// premium treats (extra)
	var validRooms = new List<Node>();

if (spawns.GetNode("LivingRoom").HasNode("PremiumSpawns"))
	validRooms.Add(spawns.GetNode("LivingRoom"));

if (spawns.GetNode("Kitchen").HasNode("PremiumSpawns"))
	validRooms.Add(spawns.GetNode("Kitchen"));

if (spawns.GetNode("Backyard").HasNode("PremiumSpawns"))
	validRooms.Add(spawns.GetNode("Backyard"));

if (validRooms.Count == 0)
{
	GD.Print("No rooms have premium spawns!");
	return;
}

// pick ONE room
var chosenRoom = validRooms[rand.Next(validRooms.Count)];

// spawn ONE premium
SpawnOnePremium(chosenRoom);

}
	
	private void SpawnOnePremium(Node room)
{
	//  check if PremiumSpawns exists
	if (!room.HasNode("PremiumSpawns"))
	{
		GD.Print(room.Name + " has no premium spawns — skipping");
		return;
	}

	var markers = room.GetNode("PremiumSpawns").GetChildren()
		.OfType<Marker2D>()
		.OrderBy(x => rand.Next())
		.ToList();

	if (markers.Count == 0)
	{
		GD.Print("No premium markers in " + room.Name);
		return;
	}

	var premiumScene = GD.Load<PackedScene>("res://scenes/PremiumTreat.tscn");

	var spawn = markers[0];

	var treatInstance = (Node2D)premiumScene.Instantiate();
	room.AddChild(treatInstance);

	treatInstance.GlobalPosition = spawn.GlobalPosition;
	treatInstance.ZIndex = 100;
}
	
	
	 
private void SpawnTreatsInArea(Node areaNode, int count)
{
	GD.Print("CALL SpawnTreatsInArea: " + areaNode.Name);
	var treatScene = GD.Load<PackedScene>("res://scenes/Treat.tscn");

	var markers = areaNode.GetNode("StandardSpawns").GetChildren()
		.OfType<Marker2D>()
		.Where(m => !usedMarkers.Contains(m)) 
		.ToList();;

	//  shuffle
	for (int i = markers.Count - 1; i > 0; i--)
	{
		int j = rand.Next(i + 1);
		(markers[i], markers[j]) = (markers[j], markers[i]);
	}

	//  FORCE exactly count
	for (int i = 0; i < count; i++)
	{
		GD.Print("SPAWNED STANDARD in " + areaNode.Name);
		var spawn = markers[i];
		usedMarkers.Add(spawn);

		var treatInstance = (Node2D)treatScene.Instantiate();
		areaNode.AddChild(treatInstance);

		treatInstance.GlobalPosition = spawn.GlobalPosition;
		treatInstance.ZIndex = 100;

		totalTreats++;
	}
}



	public void PremiumTreatCollected()
{
	PremiumTreats++;

	GD.Print("Premium total: " + PremiumTreats);
}

	public void TreatCollected(){
		collectedTreats++;
		TotalTreats += GachaShopUi.GetTreatMultiplier();
		
		GD.Print("Multiplier: " + GachaShopUi.GetTreatMultiplier());
		GD.Print("Total now: " + TotalTreats);
		
		//Opens the first door after all the treats have been collected
		if(collectedTreats == 4){
			EmitSignal(SignalName.Room1TreatsCollected);
		}
		
		//Open the second door after all the treats have been collected
		if(collectedTreats == 8) {
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
				rectShape.Size = new Vector2(60,30);
			dogCollision.Position = new Vector2(0, 8);
			break;

		case DogBreed.GoldenRetriever:
			dogAnim.SpriteFrames = GD.Load<SpriteFrames>("res://assets/Pet Dogs Pack/goldenretriever_frames.tres");
			dogAnim.Scale = new Vector2(3.8f, 3.8f);
			if (rectShape != null)
				rectShape.Size = new Vector2(95, 55 );
			dogCollision.Position = new Vector2(0, 6);
			break;

		case DogBreed.Akita:
			dogAnim.SpriteFrames = GD.Load<SpriteFrames>("res://assets/Pet Dogs Pack/akita_frames.tres");
			dogAnim.Scale = new Vector2(4.5f, 4.5f);
			if (rectShape != null)
				rectShape.Size = new Vector2(65, 45);
			dogCollision.Position = new Vector2(0, 3);
			break;

		case DogBreed.GreatDane:
			dogAnim.SpriteFrames = GD.Load<SpriteFrames>("res://assets/Pet Dogs Pack/greatdane_frames.tres");
			dogAnim.Scale = new Vector2(3.3f, 3.3f);
			if (rectShape != null)
				rectShape.Size = new Vector2(90, 60);
			dogCollision.Position = new Vector2(1, 4);
			break;

		case DogBreed.SaintBernard:
			dogAnim.SpriteFrames = GD.Load<SpriteFrames>("res://assets/Pet Dogs Pack/saintbernard_frames.tres");
			dogAnim.Scale = new Vector2(3.0f, 3.0f);
			if (rectShape != null)
				rectShape.Size = new Vector2(95,65);
			
			dogCollision.Position = new Vector2(1, 4);
			break;

		case DogBreed.SiberianHusky:
			dogAnim.SpriteFrames = GD.Load<SpriteFrames>("res://assets/Pet Dogs Pack/siberianhusky_frames.tres");
			dogAnim.Scale = new Vector2(3.5f, 3.5f);
			if (rectShape != null)
				rectShape.Size = new Vector2(85, 60);
			dogCollision.Position = new Vector2(0, 0);
			break;
			
			case DogBreed.FrostDog:
			dogAnim.SpriteFrames = GD.Load<SpriteFrames>("res://assets/Pet Dogs Pack/frostdog_frames.tres");
			dogAnim.Scale = new Vector2(2.0f, 2.0f);
			if (rectShape != null)
				rectShape.Size = new Vector2(85, 60);
			dogCollision.Position = new Vector2(0, 3);
			break;
	}

	dogAnim.Stop(); // reset animation
}

	public override void _PhysicsProcess(double delta)
{
	
	activeDog.ZIndex = (int)(activeDog.GlobalPosition.Y / 10);
	
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
		

		//activeHuman.Velocity = humanPathingAlgorithm(activeHuman.Position, activeDog.Position) * dogSpeed * humanSpeedModifier;
		//activeHuman.MoveAndSlide();
	}

	public void OnGoalTouched() {
		GD.Print("Goal touched! Returning to Gacha Shop...");
		GetTree().ChangeSceneToFile("res://scenes/gacha_shop.tscn");
	}

	public void OnFailTouched() {
		
		GD.Print("You Got Caught! Returning to Gacha Shop...");
		GetTree().ChangeSceneToFile("res://scenes/gacha_shop.tscn");
	}

	//private Vector2 humanPathingAlgorithm(Vector2 humanPos, Vector2 dogPos) {
		//// Placeholder for a more complex pathfinding algorithm
		//return (dogPos - humanPos).Normalized();
	//}
}
