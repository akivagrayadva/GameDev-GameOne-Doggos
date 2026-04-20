using Godot;

public partial class GachaShopUi : Control
{
	Button fetchButton;
	Button titleButton;
	Label treatLabel;

	Control tutorialPopup;
	Button startTutorialButton;
	Label titleLabel;
	Label bodyLabel;

	private static bool hasShownTutorial = false;

	public override void _Ready()
	{
		GD.Print("Gacha Shop ready.");

		fetchButton = GetNode<Button>("Split/RightPanel/ButtonRow/Fetch");
		titleButton = GetNode<Button>("Split/RightPanel/ButtonRow/Title");
		treatLabel = GetNode<Label>("Split/RightPanel/Currency/Treat/TreatAmount");

		tutorialPopup = GetNode<Control>("TutorialPopup");
		startTutorialButton = GetNode<Button>("TutorialPopup/PopupPanel/MarginContainer/VBoxContainer/StartButton");
		titleLabel = GetNode<Label>("TutorialPopup/PopupPanel/MarginContainer/VBoxContainer/TitleLabel");
		bodyLabel = GetNode<Label>("TutorialPopup/PopupPanel/MarginContainer/VBoxContainer/BodyLabel");

		fetchButton.Pressed += OnFetchPressed;
		titleButton.Pressed += OnTitlePressed;
		startTutorialButton.Pressed += OnTutorialClosed;

		UpdateTreatUI();
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
			"→ Arrow Keys to move\n\n" +
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
}
