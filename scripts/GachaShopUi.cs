using Godot;

public partial class GachaShopUi : CanvasLayer {
	Button fetchButton;
	Button titleButton;

	
	public override void _Ready() {
		GD.Print("Gacha Shop ready.");
		fetchButton = GetNode<Button>("Fetch");
		titleButton = GetNode<Button>("Title");

		GD.Print("Fetch button: " + fetchButton);
		GD.Print("Exit button: " + titleButton);

		fetchButton.Pressed += OnFetchPressed;
		titleButton.Pressed += OnTitlePressed;

		GD.Print("Connections: " + fetchButton.GetSignalConnectionList("pressed").Count);
		GD.Print("Connections: " + titleButton.GetSignalConnectionList("pressed").Count);
	}

	public void OnFetchPressed() {
		GD.Print("Starting Roguelike Round...");
		GetTree().ChangeSceneToFile("res://scenes/roguelike.tscn");
	}

	public void OnTitlePressed() {
		GD.Print("Returning to Title Screen...");
		GetTree().ChangeSceneToFile("res://scenes/title_screen.tscn");
	}
}
