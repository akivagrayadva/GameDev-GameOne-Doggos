using Godot;
using System;
using System.Linq;


/**
 * GachaShopUi handles:
 * - Shop UI interactions (buttons, labels)
 * - Dog selection and display
 * - Gacha pulling system (standard + premium)
 * - Difficulty selection
 * - Tutorial popup
 * - Scene transitions (play / title)
 */
public partial class GachaShopUi : Control
{
	Button fetchButton;
	Button titleButton;
	Label treatLabel;
	Label premiumTreatLabel;
	Label difficultyInfo;
	
	Random rand = new Random();


	Control tutorialPopup;
	Button startTutorialButton;
	Label titleLabel;
	Label bodyLabel;
	AnimatedSprite2D shopCat;
	
	
	Button easyButton;
	Button normalButton;
	Button hardButton;

	TextureRect dogPhoto;
	Label dogName;
	Label abilityLabel;
	TextureButton prevButton;
	TextureButton nextButton;
	Button standardPullButton;
	Button premiumPullButton;

	const int STANDARD_PULL_COST = 10;
	const int PREMIUM_PULL_COST  = 2; // premium currency is rarer
	const int PREMIUM_TREAT_COST = 10; //normal treats also required
	
	private static bool hasShownTutorial = false;
	public static RoguelikeMovement.DogBreed SelectedDog = RoguelikeMovement.DogBreed.GoldenRetriever;

	public enum Difficulty { Easy, Normal, Hard }
	public static Difficulty SelectedDifficulty = Difficulty.Normal;
	
	/**
	 * Returns multiplier for treat rewards based on difficulty.
	 */
	public static float GetTreatMultiplier() => SelectedDifficulty switch
		{
			Difficulty.Easy => 0.5f,
			Difficulty.Hard => 2.0f,
			_ => 1.0f
		};

		/**
	 	* Returns multiplier for human speed based on difficulty.
		*/
		public static float GetHumanSpeedMultiplier() => SelectedDifficulty switch
		{
			Difficulty.Easy => 1.0f,
			Difficulty.Hard => 1.5f,
			_ => 1.25f
		};
	
	private int selectedDogIndex = 0;


	/**
	 * Called when UI loads.
	 * - Gets all node references
	 * - Connects button signals
	 * - Initializes UI state
	 */
	public override void _Ready()
{
	
	
	GD.Print("Gacha Shop ready.");
	
	shopCat = GetNode<AnimatedSprite2D>("MarginContainer/Split/RightPanel/CatHud/ShopCat");
	shopCat.Play("talk");

	fetchButton = GetNode<Button>("MarginContainer/Split/RightPanel/ButtonRow/Fetch");
	titleButton = GetNode<Button>("MarginContainer/Split/RightPanel/ButtonRow/Title");
	treatLabel = GetNode<Label>("MarginContainer/Split/RightPanel/Currency/Treat/TreatAmount");
	premiumTreatLabel = GetNode<Label>("MarginContainer/Split/RightPanel/Currency/PremiumTreat/AmountOfTreats");

	easyButton = GetNode<Button>("MarginContainer/Split/RightPanel/DifficultyContainer/EasyButton");
	normalButton = GetNode<Button>("MarginContainer/Split/RightPanel/DifficultyContainer/NormalButton");
	hardButton = GetNode<Button>("MarginContainer/Split/RightPanel/DifficultyContainer/HardButton");
	difficultyInfo = GetNode<Label>("MarginContainer/Split/RightPanel/DifficultyInfo");

	easyButton.Pressed += () => OnDifficultySelected(Difficulty.Easy);
	normalButton.Pressed += () => OnDifficultySelected(Difficulty.Normal);
	hardButton.Pressed += () => OnDifficultySelected(Difficulty.Hard);
	
	// set text color to black
	easyButton.AddThemeColorOverride("font_color", new Color(0, 0, 0));
	normalButton.AddThemeColorOverride("font_color", new Color(0, 0, 0));
	hardButton.AddThemeColorOverride("font_color", new Color(0, 0, 0));

	UpdateDifficultyUI();

	tutorialPopup = GetNode<Control>("TutorialPopup");
	startTutorialButton = GetNode<Button>("TutorialPopup/PopupPanel/MarginContainer/VBoxContainer/StartButton");
	titleLabel = GetNode<Label>("TutorialPopup/PopupPanel/MarginContainer/VBoxContainer/TitleLabel");
	bodyLabel = GetNode<Label>("TutorialPopup/PopupPanel/MarginContainer/VBoxContainer/BodyLabel");

	dogPhoto = GetNode<TextureRect>("MarginContainer/Split/LeftPanel/CurrentDog/DogPhoto");
	dogName = GetNode<Label>("MarginContainer/Split/LeftPanel/CurrentDog/DogName");
	prevButton = GetNode<TextureButton>("MarginContainer/Split/LeftPanel/CurrentDog/PrevButton");
	nextButton = GetNode<TextureButton>("MarginContainer/Split/LeftPanel/CurrentDog/NextButton");
	abilityLabel = GetNode<Label>("MarginContainer/Split/LeftPanel/CurrentDog/DogAbilityLabel");
	
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

	GD.Print("SHOP LOAD TotalTreats: " + RoguelikeMovement.TotalTreats);

	
	UpdateTreatUI();
	selectedDogIndex = RoguelikeMovement.LastSelectedDogIndex;

if (selectedDogIndex >= RoguelikeMovement.OwnedDogs.Length)
{
	selectedDogIndex = 0;
}

	
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
	/**
	 * Updates treat and premium treat display.
	 */
	private void UpdateTreatUI()
	{
		treatLabel.Text = "x" + Math.Floor(RoguelikeMovement.TotalTreats);
		premiumTreatLabel.Text = "x" + RoguelikeMovement.PremiumTreats;
	}
	
	/**
	 * Updates currently selected dog UI.
	 */
	private void UpdateDogUI()
	{
	var currentDog = RoguelikeMovement.OwnedDogs[selectedDogIndex];
	SelectedDog = currentDog;

	dogName.Text = GetDogDisplayName(currentDog);
	dogPhoto.Texture = GetDogTexture(currentDog);
	abilityLabel.Text = GetDogAbilityDescription(currentDog);
	 GD.Print("Ability text set to: " + abilityLabel.Text);
	}
	
	
	private string GetDogDisplayName(RoguelikeMovement.DogBreed dog)
	{
		switch (dog)
		{
			case RoguelikeMovement.DogBreed.GoldenRetriever:
				return "Goodest Boy";
			case RoguelikeMovement.DogBreed.Akita:
				return "Pulled Bork";
			case RoguelikeMovement.DogBreed.GreatDane:
				return "Big Boy Supreme";
			case RoguelikeMovement.DogBreed.Schnauzer:
				return "Smol King";
			case RoguelikeMovement.DogBreed.SaintBernard:
				return "Slobber Chonk";
			case RoguelikeMovement.DogBreed.SiberianHusky:
				return "Certified Zoomer";
			case RoguelikeMovement.DogBreed.FrostDog:
				return "Ice Gremlin";
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

	/**
	 * Moves to previous dog in list (wraps around).
	 */
	private void OnPrevPressed()
	{
		GD.Print("Prev button clicked");
		selectedDogIndex--;

		if (selectedDogIndex < 0)
		{
			selectedDogIndex = RoguelikeMovement.OwnedDogs.Length - 1;
			
		}
	
		RoguelikeMovement.LastSelectedDogIndex = selectedDogIndex;

		UpdateDogUI();
	}
	
	/**
	 * Moves to next dog in list (wraps around).
	 */
	private void OnNextPressed()
	{
		GD.Print("Next button clicked");
		selectedDogIndex++;

		if (selectedDogIndex >= RoguelikeMovement.OwnedDogs.Length)
		{
			selectedDogIndex = 0;
		}

		RoguelikeMovement.LastSelectedDogIndex = selectedDogIndex;
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
			"→ WASD Keys or Arrow Keys to move\n" +
			"→ Space key to use dog's ability\n\n" +
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
	
	/**
	 * Handles standard gacha pull.
	 * - Costs normal treats
	 * - Gives common dogs
	 */
	private void OnStandardPull()
{
	if (Math.Floor(RoguelikeMovement.TotalTreats) < STANDARD_PULL_COST)
	{
		GD.Print("Not enough treats!");
		return;
	}

	RoguelikeMovement.TotalTreats -= STANDARD_PULL_COST;

	var dog = RollDog();
	GD.Print("Pulled: " + dog);

	AddDog(dog);
	selectedDogIndex = RoguelikeMovement.OwnedDogs.Length - 1;
	RoguelikeMovement.LastSelectedDogIndex = selectedDogIndex;
	UpdateDogUI();
	UpdateTreatUI();
}

/**
 * Handles premium gacha pull.
 * - Costs premium + normal treats
 * - Only gives rare dogs
 * - Prevents duplicates
 */
private void OnPremiumPull()
{
	if (RoguelikeMovement.PremiumTreats < PREMIUM_PULL_COST || RoguelikeMovement.TotalTreats < PREMIUM_TREAT_COST)
	{
		GD.Print("Not enough treats or premium treats!");
		return;
	}

	RoguelikeMovement.PremiumTreats -= PREMIUM_PULL_COST;
	RoguelikeMovement.TotalTreats -= PREMIUM_TREAT_COST;

	var premiumDogs = new[]
	{
		RoguelikeMovement.DogBreed.SiberianHusky,
		RoguelikeMovement.DogBreed.FrostDog,
		RoguelikeMovement.DogBreed.GreatDane,
		RoguelikeMovement.DogBreed.SaintBernard
	};

	//  only dogs you DON'T own
	var availableDogs = premiumDogs
		.Where(d => !RoguelikeMovement.OwnedDogs.Contains(d))
		.ToList();

	// if all owned
	if (availableDogs.Count == 0)
	{
		GD.Print("All premium dogs owned! Giving bonus instead.");
		RoguelikeMovement.PremiumTreats += 1;
		UpdateTreatUI();
		return;
	}

	// pick clean random
	var dog = availableDogs[rand.Next(availableDogs.Count)];

	GD.Print("PREMIUM: " + dog);

	AddDog(dog);

	selectedDogIndex = RoguelikeMovement.OwnedDogs.Length - 1;
	RoguelikeMovement.LastSelectedDogIndex = selectedDogIndex;

	UpdateDogUI();
	UpdateTreatUI();
}


private RoguelikeMovement.DogBreed RollDog()
{
	int roll = rand.Next(0, 100);

	// STANDARD ONLY
	if (roll < 50) return RoguelikeMovement.DogBreed.Akita;
	return RoguelikeMovement.DogBreed.Schnauzer;
}
private void AddDog(RoguelikeMovement.DogBreed dog)
{
	foreach (var d in RoguelikeMovement.OwnedDogs)
	{
		if (d == dog)
		{
			GD.Print("Duplicate!");
			return;
		}
	}

	Array.Resize(ref RoguelikeMovement.OwnedDogs, RoguelikeMovement.OwnedDogs.Length + 1);
	RoguelikeMovement.OwnedDogs[RoguelikeMovement.OwnedDogs.Length - 1] = dog;

	GD.Print("New dog added!");
}

private void OnDifficultySelected(Difficulty difficulty)
{
	SelectedDifficulty = difficulty;
	UpdateDifficultyUI();
}
/**
  * Updates difficulty button visuals + description text.
  */
private void UpdateDifficultyUI()
{
	// reset style - white background, no border
	var normalStyle = new StyleBoxFlat();
	normalStyle.BgColor = new Color(1, 1, 1); // white

	normalStyle.CornerRadiusTopLeft = 8;
	normalStyle.CornerRadiusTopRight = 8;
	normalStyle.CornerRadiusBottomLeft = 8;
	normalStyle.CornerRadiusBottomRight = 8;

	easyButton.AddThemeStyleboxOverride("normal", normalStyle);
	normalButton.AddThemeStyleboxOverride("normal", normalStyle);
	hardButton.AddThemeStyleboxOverride("normal", normalStyle);

	// selected style - white background with gold border
	var selectedStyle = new StyleBoxFlat();
	selectedStyle.BgColor = new Color(1, 1, 1); // white
	selectedStyle.BorderColor = new Color(1, 0.8f, 0); // gold
	selectedStyle.SetBorderWidthAll(3);
	
	selectedStyle.CornerRadiusTopLeft = 8;
	selectedStyle.CornerRadiusTopRight = 8;
	selectedStyle.CornerRadiusBottomLeft = 8;
	selectedStyle.CornerRadiusBottomRight = 8;

	switch (SelectedDifficulty)
	{
		case Difficulty.Easy:
			easyButton.AddThemeStyleboxOverride("normal", selectedStyle);
			difficultyInfo.Text = "Easy - Earn half a treat per pickup, slower human";
			break;
		case Difficulty.Normal:
			normalButton.AddThemeStyleboxOverride("normal", selectedStyle);
			difficultyInfo.Text = "Normal - Earn one full treat per pickup";
			break;
		case Difficulty.Hard:
			hardButton.AddThemeStyleboxOverride("normal", selectedStyle);
			difficultyInfo.Text = "Hard - Earn double treats per pickup, faster human";
			break;
	}
}

private string GetDogAbilityDescription(RoguelikeMovement.DogBreed dog)
{
	switch (dog)
	{
		case RoguelikeMovement.DogBreed.GoldenRetriever:
			return "Ability: Who's a good boy?\nDistracts human for 2.5s\nCooldown: 20s";
		case RoguelikeMovement.DogBreed.Akita:
			return "Ability: Bark Stun\nStops human completely for 1.5s\nCooldown: 20s";
		case RoguelikeMovement.DogBreed.Schnauzer:
			return "Ability: Dash\nBurst forward at high speed\nCooldown: 20s";
		case RoguelikeMovement.DogBreed.SiberianHusky:
			return "Ability: Sprint\nDoubles speed for 1.5s\nCooldown: 15s";
		case RoguelikeMovement.DogBreed.FrostDog:
			return "Ability: Frost Aura\nSlows human for 10s\nCooldown: 30s";
		case RoguelikeMovement.DogBreed.GreatDane:
			return "Ability: Intimidation\nHuman backs off for 2s\nCooldown: 20s";
		case RoguelikeMovement.DogBreed.SaintBernard:
			return "Ability: Drool Mess\nSlows human for 3s\nCooldown: 20s";
		default:
			return "";
	}
}


}
