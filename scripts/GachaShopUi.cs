using Godot;

public partial class GachaShopUi : Control {
	Button fetchButton;
	Button titleButton;

	
	public override void _Ready() {
		GD.Print("Gacha Shop ready.");
		fetchButton = GetNode<Button>("Split/RightPanel/ButtonRow/Fetch");
		titleButton = GetNode<Button>("Split/RightPanel/ButtonRow/Title");

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
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
}
