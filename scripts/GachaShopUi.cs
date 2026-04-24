using Godot;
using System;

public partial class GachaShopUi : Control
{
	Button fetchButton;
	Button titleButton;
	Label treatLabel;
	Label premiumTreatLabel;
	
	Random rand = new Random();


	Control tutorialPopup;
	Button startTutorialButton;
	Label titleLabel;
	Label bodyLabel;
	AnimatedSprite2D shopCat;

	TextureRect dogPhoto;
	Label dogName;
	TextureButton prevButton;
	TextureButton nextButton;
	Button standardPullButton;
	Button premiumPullButton;

	const int STANDARD_PULL_COST = 10;
	const int PREMIUM_PULL_COST  = 2; // premium currency is rarer
	const int PREMIUM_TREAT_COST = 5; //normal treats also required
	
	private static bool hasShownTutorial = false;
	public static RoguelikeMovement.DogBreed SelectedDog = RoguelikeMovement.DogBreed.GoldenRetriever;

	//testing to see if photos show up. will change code
	private RoguelikeMovement.DogBreed[] ownedDogs =
	{
	RoguelikeMovement.DogBreed.GoldenRetriever,
	
	};

	private int selectedDogIndex = 0;

	public override void _Ready()
{
	GD.Print("Gacha Shop ready.");
	
	shopCat = GetNode<AnimatedSprite2D>("MarginContainer/Split/RightPanel/CatHud/ShopCat");
	shopCat.Play("talk");

	fetchButton = GetNode<Button>("MarginContainer/Split/RightPanel/ButtonRow/Fetch");
	titleButton = GetNode<Button>("MarginContainer/Split/RightPanel/ButtonRow/Title");
	treatLabel = GetNode<Label>("MarginContainer/Split/RightPanel/Currency/Treat/TreatAmount");
	premiumTreatLabel = GetNode<Label>("MarginContainer/Split/RightPanel/Currency/PremiumTreat/AmountOfTreats");


	tutorialPopup = GetNode<Control>("TutorialPopup");
	startTutorialButton = GetNode<Button>("TutorialPopup/PopupPanel/MarginContainer/VBoxContainer/StartButton");
	titleLabel = GetNode<Label>("TutorialPopup/PopupPanel/MarginContainer/VBoxContainer/TitleLabel");
	bodyLabel = GetNode<Label>("TutorialPopup/PopupPanel/MarginContainer/VBoxContainer/BodyLabel");

	dogPhoto = GetNode<TextureRect>("MarginContainer/Split/LeftPanel/CurrentDog/DogPhoto");
	dogName = GetNode<Label>("MarginContainer/Split/LeftPanel/CurrentDog/DogName");
	prevButton = GetNode<TextureButton>("MarginContainer/Split/LeftPanel/CurrentDog/PrevButton");
	nextButton = GetNode<TextureButton>("MarginContainer/Split/LeftPanel/CurrentDog/NextButton");
	
	standardPullButton = GetNode<Button>("MarginContainer/Split/LeftPanel/StdPull");
	premiumPullButton = GetNode<Button>("MarginContainer/Split/LeftPanel/PremPull");

	GD.Print("Prev button found: " + prevButton);
	GD.Print("Next button found: " + nextButton);

	fetchButton.Pressed += OnFetchPressed;
	titleButton.Pressed += OnTitlePressed;
	startTutorialButton.Pressed += OnTutorialClosed;
	prevButton.Pressed += OnPrevPressed;
	nextButton.Pressed += OnNextPressed;
	standardPullButton.Pressed += OnStandardPull;
	premiumPullButton.Pressed += OnPremiumPull;

	UpdateTreatUI();
	UpdateDogUI();
	SetupTutorialText();

	if (!hasShownTutorial)
	{
		tutorialPopup.Visible = true;
		hasShownTutorial = true;
	}
	else
	{
		tutorialPopup.Visible = false;
	}
}
	private void UpdateTreatUI()
	{
		treatLabel.Text = "x" + RoguelikeMovement.TotalTreats;
		premiumTreatLabel.Text = "x" + RoguelikeMovement.PremiumTreats;
	}

	private void UpdateDogUI()
	{
		RoguelikeMovement.DogBreed currentDog = ownedDogs[selectedDogIndex];
	SelectedDog = currentDog;

	dogName.Text = GetDogDisplayName(currentDog);
	dogPhoto.Texture = GetDogTexture(currentDog);
	}

	private string GetDogDisplayName(RoguelikeMovement.DogBreed dog)
	{
		switch (dog)
		{
			case RoguelikeMovement.DogBreed.GoldenRetriever:
				return "Golden Retriever";
			case RoguelikeMovement.DogBreed.Akita:
				return "Akita";
			case RoguelikeMovement.DogBreed.GreatDane:
				return "Great Dane";
			case RoguelikeMovement.DogBreed.Schnauzer:
				return "Schnauzer";
			case RoguelikeMovement.DogBreed.SaintBernard:
				return "Saint Bernard";
			case RoguelikeMovement.DogBreed.SiberianHusky:
				return "Siberian Husky";
			case RoguelikeMovement.DogBreed.FrostDog:
				return "Frosty";
			default:
				return "Dog";
		}
	}

	private Texture2D GetDogTexture(RoguelikeMovement.DogBreed dog)
	{
		switch (dog)
		{
			case RoguelikeMovement.DogBreed.GoldenRetriever:
				return GD.Load<Texture2D>("res://assets/Pet Dogs Pack/golden.png");
			case RoguelikeMovement.DogBreed.Akita:
				return GD.Load<Texture2D>("res://assets/Pet Dogs Pack/akita.png");
			case RoguelikeMovement.DogBreed.GreatDane:
				return GD.Load<Texture2D>("res://assets/Pet Dogs Pack/greatdane.png");
			case RoguelikeMovement.DogBreed.Schnauzer:
				return GD.Load<Texture2D>("res://assets/Pet Dogs Pack/schnauzer.png");
			case RoguelikeMovement.DogBreed.SaintBernard:
				return GD.Load<Texture2D>("res://assets/Pet Dogs Pack/saintbernard.png");
			case RoguelikeMovement.DogBreed.SiberianHusky:
				return GD.Load<Texture2D>("res://assets/Pet Dogs Pack/siberianhusky.png");
			case RoguelikeMovement.DogBreed.FrostDog:
				return GD.Load<Texture2D>("res://assets/Pet Dogs Pack/Frost.png");
			default:
				return null;
		}
	}

	private void OnPrevPressed()
	{
		GD.Print("Prev button clicked");
		selectedDogIndex--;

		if (selectedDogIndex < 0)
		{
			selectedDogIndex = ownedDogs.Length - 1;
		}

		UpdateDogUI();
	}

	private void OnNextPressed()
	{
		GD.Print("Next button clicked");
		selectedDogIndex++;

		if (selectedDogIndex >= ownedDogs.Length)
		{
			selectedDogIndex = 0;
		}

		UpdateDogUI();
	}

	private void SetupTutorialText()
	{
		titleLabel.Text = "How to Play";

		bodyLabel.Text =
			"Welcome to Dogcha!\n\n" +
			"You are a dog.\n" +
			"Get the treats.\n" +
			"Don't get caught.\n\n" +
			"Controls:\n" +
			"→ WASD Keys or Arrow Keys to move\n\n" +
			"Goal:\n" +
			"→ Collect all treats\n" +
			"→ Avoid the human\n\n" +
			"Shop:\n" +
			"→ Treats = normal currency\n" +
			"→ Premium treats = better pulls\n\n" +
			"Buttons:\n" +
			"→ Fetch = play a run\n" +
			"→ Title = main menu";
	}

	private void OnTutorialClosed()
	{
		tutorialPopup.Visible = false;
	}

	public void OnFetchPressed()
	{
		GD.Print("Starting Roguelike Round...");
		GetTree().ChangeSceneToFile("res://scenes/roguelike.tscn");
	}

	public void OnTitlePressed()
	{
		GD.Print("Returning to Title Screen...");
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
	
	private void OnStandardPull()
{
	if (RoguelikeMovement.TotalTreats < STANDARD_PULL_COST)
	{
		GD.Print("Not enough treats!");
		return;
	}

	RoguelikeMovement.TotalTreats -= STANDARD_PULL_COST;

	var dog = RollDog(false);
	GD.Print("Pulled: " + dog);

	AddDog(dog);
	selectedDogIndex = ownedDogs.Length - 1;
	UpdateDogUI();
	UpdateTreatUI();
}
private void OnPremiumPull()
{
	if (RoguelikeMovement.PremiumTreats < PREMIUM_PULL_COST || RoguelikeMovement.TotalTreats < PREMIUM_TREAT_COST)
	{
		GD.Print("Not enough premium treats!");
		return;
	}

	RoguelikeMovement.PremiumTreats -= PREMIUM_PULL_COST;
	RoguelikeMovement.TotalTreats -= PREMIUM_TREAT_COST;

	var dog = RollDog(true);
	GD.Print(" PREMIUM: " + dog);

	AddDog(dog);
	selectedDogIndex = ownedDogs.Length - 1; 
	UpdateDogUI();
	UpdateTreatUI();
}
private RoguelikeMovement.DogBreed RollDog(bool premium)
{
	int roll = rand.Next(0, 100);

	if (premium)
	{
		// PREMIUM POOL (no common dogs)
		if (roll < 40) return RoguelikeMovement.DogBreed.SiberianHusky;
		if (roll < 75) return RoguelikeMovement.DogBreed.FrostDog;
		if (roll < 90) return RoguelikeMovement.DogBreed.GreatDane;
		return RoguelikeMovement.DogBreed.SaintBernard; // rarest
	}
	else
	{
		// STANDARD POOL (common only)
		if (roll < 80) return RoguelikeMovement.DogBreed.Akita;
		return RoguelikeMovement.DogBreed.Schnauzer;
	}
}
private void AddDog(RoguelikeMovement.DogBreed dog)
{
	foreach (var d in ownedDogs)
	{
		if (d == dog)
		{
			GD.Print("Duplicate!");
			return;
		}
	}

	Array.Resize(ref ownedDogs, ownedDogs.Length + 1);
	ownedDogs[ownedDogs.Length - 1] = dog;

	GD.Print("New dog added!");
}
}
