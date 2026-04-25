using Godot;
using System;

public partial class PremiumTreat : Area2D
{
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.Name == "DummyDog")
		{
			GD.Print("Premium treat collected!");

			RoguelikeMovement.PremiumTreats += 1;

			QueueFree();
		}
	}
}
