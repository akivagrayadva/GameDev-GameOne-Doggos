using Godot;
using System;

/**
 * MainMenu handles:
 * - Start button (go to game/shop scene)
 * - Exit button (close application)
 * - Connecting UI signals to functions
 */
public partial class MainMenu : Control
{
	Button startButton;
	Button exitButton;

	
	/**
	 * Called when the scene loads.
	 * - Gets references to UI buttons
	 * - Connects button signals
	 * - Prints debug info
	 */
	public override void _Ready() {
		GD.Print("Title screen ready.");
		startButton = GetNode<Button>("VBoxContainer/Start");
		exitButton = GetNode<Button>("VBoxContainer/Exit");

		GD.Print("Start button: " + startButton);
		GD.Print("Exit button: " + exitButton);

		startButton.Pressed += OnStartPressed;
		exitButton.Pressed += OnExitPressed;

		GD.Print("Connections: " + startButton.GetSignalConnectionList("pressed").Count);
		GD.Print("Connections: " + exitButton.GetSignalConnectionList("pressed").Count);
	}

	/**
	 * Called when Start button is pressed.
	 * Changes scene to the gacha shop.
	 */
	public void OnStartPressed() {
		GD.Print("Starting game...");
		GetTree().ChangeSceneToFile("res://scenes/gacha_shop.tscn");
	}

	/**
	 * Called when Exit button is pressed.
	 * Closes the game.
	 */
	public void OnExitPressed() {
		GD.Print("Exiting game...");
		GetTree().Quit();
	}
}
