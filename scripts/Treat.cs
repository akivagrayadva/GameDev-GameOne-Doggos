using Godot;
using System;

public partial class Treat : Area2D
{
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		if (body.Name == "DummyDog")
		{
			GD.Print("Dog touched the treat!");
			QueueFree();
			
		}
	}
}
