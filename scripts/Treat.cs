using Godot;
using System;

public partial class Treat : Area2D
{
	private bool collected = false;

	public override void _Ready()
	{
		GD.Print("TREAT CREATED at: " + GlobalPosition); 
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
{
	if (collected) return;

	if (body.Name == "DummyDog")
	{
		collected = true;

		BodyEntered -= OnBodyEntered; //  stop future triggers immediately

		RoguelikeMovement.Instance.TreatCollected();

		QueueFree();
	}
}
}
