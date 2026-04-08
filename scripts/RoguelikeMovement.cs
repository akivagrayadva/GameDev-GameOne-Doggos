using Godot;
using System;

public partial class RoguelikeMovement : Node2D {
    CharacterBody2D activeDog;
    CharacterBody2D activeHuman;
    float dogSpeed = GLOBAL_CONSTANTS.DOG_SPEED;
    float humanSpeedModifier = GLOBAL_CONSTANTS.HUMAN_SPEED_Modifier;
    public override void _Ready(){
        activeDog = GetNode<CharacterBody2D>("DummyDog");       // Change to "ActiveDog" when the actual dog scene is ready
        activeHuman = GetNode<CharacterBody2D>("DummyHuman");   // Change to "ActiveHuman" when the actual human scene is ready
        var goalArea = GetNode<Area2D>("DummyGoal");             // This becomes the collision for the canna-biscuits that end a sucessful round
        var failArea = activeHuman.GetNode<Area2D>("DogDetection");                             // This becomes the collision for the human that ends a failed round
        
        goalArea.BodyEntered += (body) => {
            if (body == activeDog)
                OnGoalTouched();
        };

        failArea.BodyEntered += (body) => {
            if (body == activeDog)
                OnFailTouched();
        };

        GD.Print("Ready to run.");
    }

    public override void _PhysicsProcess(double delta) {
        Vector2 inputVector = Vector2.Zero;

        if (Input.IsActionPressed("ui_right"))
            inputVector.X +=  1;
        if (Input.IsActionPressed("ui_left"))
            inputVector.X += -1;
        if (Input.IsActionPressed("ui_down"))
            inputVector.Y +=  1;
        if (Input.IsActionPressed("ui_up"))
            inputVector.Y += -1;

        inputVector = inputVector.Normalized();

        activeDog.Velocity = inputVector * dogSpeed;           // speed needs to be a float to multiply with the direction vector 
        activeDog.MoveAndSlide();

        activeHuman.Velocity = humanPathingAlgorithm(activeHuman.Position, activeDog.Position) * dogSpeed * humanSpeedModifier;
        activeHuman.MoveAndSlide();
    }

    public void OnGoalTouched() {
        GD.Print("Goal touched! Returning to Gacha Shop...");
        GetTree().ChangeSceneToFile("res://scenes/gacha_shop.tscn");
    }

    public void OnFailTouched() {
        GD.Print("You Got Caught! Returning to Gacha Shop...");
        GetTree().ChangeSceneToFile("res://scenes/gacha_shop.tscn");
    }

    private Vector2 humanPathingAlgorithm(Vector2 humanPos, Vector2 dogPos) {
        // Placeholder for a more complex pathfinding algorithm
        return (dogPos - humanPos).Normalized();
    }

}