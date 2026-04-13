using Godot;

public partial class TitleScreen : Control {
	Button startButton;
	Button exitButton;
	

	
	public override void _Ready() {
		GD.Print("Title screen ready.");
		startButton = GetNode<Button>("Start");
		exitButton = GetNode<Button>("Exit");
	

		GD.Print("Start button: " + startButton);
		GD.Print("Exit button: " + exitButton);
		

		startButton.Pressed += OnStartPressed;
		exitButton.Pressed += OnExitPressed;
		

		GD.Print("Connections: " + startButton.GetSignalConnectionList("pressed").Count);
		GD.Print("Connections: " + exitButton.GetSignalConnectionList("pressed").Count);
	}

	public void OnStartPressed() {
		GD.Print("Starting game...");
		GetTree().ChangeSceneToFile("res://scenes/gacha_shop.tscn");
	}

	public void OnExitPressed() {
		GD.Print("Exiting game...");
		GetTree().Quit();
	}
	
}
