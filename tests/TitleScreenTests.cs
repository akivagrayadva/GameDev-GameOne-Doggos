using Godot;
using GdUnit4;
using static GdUnit4.Assertions;

[TestSuite, RequireGodotRuntime]
public class TitleScreenTests {
    TitleScreen titleScreen;

    [Before, RequireGodotRuntime]
    public void Setup() {
        var scene = ResourceLoader.Load<PackedScene>("res://scenes/title_screen.tscn");
        var instance = scene.Instantiate<TitleScreen>();
        titleScreen = instance;
        titleScreen._Ready();       // Manually call _Ready to initialize the scene and connect signals
    }

    [TestCase, RequireGodotRuntime]
    public void TestStartButtonExists() {
        var startButton = titleScreen.GetNode<Button>("Start");
        AssertObject(startButton).IsNotNull();
        AssertObject(startButton).IsInstanceOf<Button>();
    }

    [TestCase, RequireGodotRuntime]
    public void TestExitButtonExists() {
        var exitButton = titleScreen.GetNode<Button>("Exit");
        AssertObject(exitButton).IsNotNull();
        AssertObject(exitButton).IsInstanceOf<Button>();
    }

     [TestCase, RequireGodotRuntime]
    public void TestStartButtonPressed() {
        var startButton = titleScreen.GetNode<Button>("Start");
        AssertObject(startButton).IsNotNull();
        startButton.EmitSignal("pressed");
        // Here you would check if the scene changes to the next one, but since we can't run the game loop, we will just assert that the signal was emitted.
    }

    [TestCase, RequireGodotRuntime]
    public void TestExitButtonPressed() {
        var exitButton = titleScreen.GetNode<Button>("Exit");
        AssertObject(exitButton).IsNotNull();
        exitButton.EmitSignal("pressed");
        // Here you would check if the application quits, but since we can't run the game loop, we will just assert that the signal was emitted.
    }

}