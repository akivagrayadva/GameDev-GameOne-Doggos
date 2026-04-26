using Godot;
using System;

public partial class PremiumTreat : Area2D
{
	private bool collected = false;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (collected) return;

		if (body.Name == "DummyDog")
		{
			collected = true;

			GD.Print("Premium treat collected!");

			RoguelikeMovement.Instance.PremiumTreatCollected();

			QueueFree();
		}
	}
}
