using Godot;
using System;

/**
 * PremiumTreat represents a collectible premium item.
 *
 * Responsibilities:
 * - Detect when the dog touches it
 * - Prevent duplicate collection
 * - Notify the game that a premium treat was collected
 * - Remove itself from the scene
 */
public partial class PremiumTreat : Area2D
{
	private bool collected = false;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	/**
	 * Triggered when another body enters this area.
	 *
	 * @param body The object that collided with the treat
	 */
	private void OnBodyEntered(Node2D body)
{
	if (collected) return;

	if (body.Name == "DummyDog")
	{
		collected = true;

		BodyEntered -= OnBodyEntered; 

		GD.Print("Premium treat collected!");

		RoguelikeMovement.Instance.PremiumTreatCollected();

		QueueFree();
	}
}}
