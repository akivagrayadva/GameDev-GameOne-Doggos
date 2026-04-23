using Godot;
using System;

public partial class DogController : Node
{
	public RoguelikeMovement.DogBreed CurrentDog;

	// stats
	public float dogSpeed;
	public float baseDogSpeed;

	// ability
	float abilityTimer = 0f;
	float abilityCooldown = 0f;
	bool isDashing = false;

	Vector2 lastInput = Vector2.Zero;

	//reference -> this will be set in teh main script
	public CharacterBody2D activeDog;
	public CharacterBody2D activeHuman;

	float baseHumanSpeed;
	public float humanSpeedModifier;

	public void Setup(
		RoguelikeMovement.DogBreed breed,
		float humanSpeed,
		CharacterBody2D dog,
		CharacterBody2D human)
	{
		CurrentDog = breed;
		DogStats stats = GetStatsFromBreed(breed);

		dogSpeed = stats.MoveSpeed;
		baseDogSpeed = stats.MoveSpeed;

		baseHumanSpeed = humanSpeed;
		humanSpeedModifier = humanSpeed;

		activeDog = dog;
		activeHuman = human;
	}
	private DogStats GetStatsFromBreed(RoguelikeMovement.DogBreed breed)
{
	switch (breed)
	{
		case RoguelikeMovement.DogBreed.SiberianHusky:
		case RoguelikeMovement.DogBreed.FrostDog:
			return new DogStats("Fast Dog", 250f, 5f);

		case RoguelikeMovement.DogBreed.GreatDane:
		case RoguelikeMovement.DogBreed.SaintBernard:
			return new DogStats("Heavy Dog", 160f, 12f);

		default:
			return new DogStats("Balanced Dog", 200f, 8f);
	}
}


	private void UseAbility()
{
	switch (CurrentDog)
	{
		case RoguelikeMovement.DogBreed.SiberianHusky:
			// Sprint
			dogSpeed = baseDogSpeed * 2f;
			abilityTimer = 1.5f;
			abilityCooldown = 15f;
			GD.Print("Husky Sprint!");
			break;

		case RoguelikeMovement.DogBreed.FrostDog:
			// Slow human
			humanSpeedModifier = baseHumanSpeed * 0.5f;
			abilityTimer = 20.0f;
			abilityCooldown = 20.0f;
			GD.Print("Frost Aura!");
			break;

		case RoguelikeMovement.DogBreed.Schnauzer:
			// Dash
			if (lastInput != Vector2.Zero)
			{
			 activeDog.Velocity = lastInput * 450f; // stronger dash
			 isDashing = true;
			 abilityTimer = 0.2f;
			}
				abilityCooldown = 20.0f;
				GD.Print("Dash!");
				break;
		case RoguelikeMovement.DogBreed.Akita:
			// Stun human

			humanSpeedModifier = 0f;
			abilityTimer = 1.5f;
			abilityCooldown = 20.0f;
			GD.Print("Bark Stun!");
			break;

		case RoguelikeMovement.DogBreed.GoldenRetriever:
		// Human gets distracted (petting the dog)
			humanSpeedModifier = baseHumanSpeed * 0.2f;
			abilityTimer = 2.5f;
			abilityCooldown = 20.0f;
			GD.Print("Who's a good boy??");
			break;

		case RoguelikeMovement.DogBreed.SaintBernard:
		// Drool mess confuses/slows human
			humanSpeedModifier = baseHumanSpeed * 0.4f;
			abilityTimer = 3.0f;
			abilityCooldown = 20.0f;
			GD.Print("Drool everywhere!");
			break;

		case RoguelikeMovement.DogBreed.GreatDane:
		// Human backs off instead of chasing
			humanSpeedModifier = baseHumanSpeed * 0.1f;
			abilityTimer = 2.0f;
			abilityCooldown = 20.0f;
			GD.Print("Intimidation!");
			break;
	}
}

		public void Update(double delta)
	{
		float dt = (float)delta;

		// timers
		if (abilityTimer > 0)
		{
			abilityTimer -= dt;

			if (abilityTimer <= 0)
			{
				dogSpeed = baseDogSpeed;
				humanSpeedModifier = baseHumanSpeed;
				isDashing = false;


			}
		}

		if (abilityCooldown > 0)
			abilityCooldown -= dt;
	}

		public void TryUseAbility()
	{
		if (abilityCooldown <= 0)
			UseAbility();
	}

		public void ApplyMovement(Vector2 input)
	{
		if (input != Vector2.Zero)
			lastInput = input;

		if (!isDashing)
			activeDog.Velocity = input * dogSpeed;
	}
}
