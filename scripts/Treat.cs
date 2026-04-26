using Godot;
using System;

/**
 * Treat represents a standard collectible item.
 *
 * Responsibilities:
 * - Detect when the dog touches it
 * - Prevent duplicate collection
 * - Notify the game controller
 * - Remove itself after being collected
 */
public partial class Treat : Area2D
{
	private bool collected = false; // prevent the treat from being collected mutliple times



	/**
	 * Called when the node enters the scene.
	 * Connects the collision signal and logs spawn position.
	 */
	public override void _Ready()
	{
		GD.Print("TREAT CREATED at: " + GlobalPosition); 
		BodyEntered += OnBodyEntered;
	}

	/**
	 * Triggered when another body enters this area.
	 *
	 * @param body The object that touched the treat
	 */
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
