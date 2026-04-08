using Godot;
using GdUnit4;
using static GdUnit4.Assertions;

[TestSuite, RequireGodotRuntime]
public class RoguelikeMovementTests {
    RoguelikeMovement roguelikeMovement;

    [Before, RequireGodotRuntime]
    public void Setup() {
        var scene = ResourceLoader.Load<PackedScene>("res://scenes/roguelike.tscn");
        var instance = scene.Instantiate<RoguelikeMovement>();
        roguelikeMovement = instance;
        roguelikeMovement._Ready();       // Manually call _Ready to initialize the scene and connect signals
    }

    [TestCase, RequireGodotRuntime]
    public void TestActiveDogExists() {
        var activeDog = roguelikeMovement.GetNode<CharacterBody2D>("DummyDog");
        AssertObject(activeDog).IsNotNull();
        AssertObject(activeDog).IsInstanceOf<CharacterBody2D>();
    }

    [TestCase, RequireGodotRuntime]
    public void TestActiveHumanExists() {
        var activeHuman = roguelikeMovement.GetNode<CharacterBody2D>("DummyHuman");
        AssertObject(activeHuman).IsNotNull();
        AssertObject(activeHuman).IsInstanceOf<CharacterBody2D>();
    }

    [TestCase, RequireGodotRuntime]
    public void TestGoalAreaExists() {
        var goalArea = roguelikeMovement.GetNode<Area2D>("DummyGoal");
        AssertObject(goalArea).IsNotNull();
        AssertObject(goalArea).IsInstanceOf<Area2D>();
    }

    [TestCase, RequireGodotRuntime]
    public void TestFailAreaExists() {
        var failArea = roguelikeMovement.GetNode<CharacterBody2D>("DummyHuman").GetNode<Area2D>("DogDetection");
        AssertObject(failArea).IsNotNull();
        AssertObject(failArea).IsInstanceOf<Area2D>();
    }

}