using Godot;

/**
 * Door handles:
 * - Listening for room completion events
 * - Opening when conditions are met
 * - Disabling collision so player can pass through
 */
public partial class Door : StaticBody2D
{
	
	public enum DoorRoom { LivingRoom, Kitchen } // add more rooms as needed

	[Export] public DoorRoom Room { get; set; }
	[Export] public Texture2D DoorClosedTexture { get; set; }
	[Export] public Texture2D DoorOpenTexture { get; set; }

	/**
	 * Called when the node is added to the scene.
	 * Subscribes the door to the correct event based on its room.
	 */
	public override void _Ready()
	{
		switch (Room)
		{
			case DoorRoom.LivingRoom:
				RoguelikeMovement.Instance.Room1TreatsCollected += OpenKitchenDoor;
				break;
			case DoorRoom.Kitchen:
				RoguelikeMovement.Instance.Room2TreatsCollected += OpenBedroomDoor;
				break;
		}
	}

	/**
	 * Opens the door to the kitchen.
	 * - Changes texture to open
	 * - Adjusts sprite scale/offset for visual alignment
	 * - Disables collision so player can pass
	 */
	public void OpenKitchenDoor(){
		var sprite = GetNode<Sprite2D>("Sprite2D");
		var collision = GetNode<CollisionShape2D>("CollisionShape2D");
		GetNode<Sprite2D>("Sprite2D").Texture = DoorOpenTexture;
		sprite.Scale = new Vector2(2.0f, -1.662f);
		sprite.Offset = new Vector2(11, 0);
		collision.SetDeferred("disabled", true);

	}
	/**
	 * Opens the door to the bedroom. *Note this is the door to get outside, not bedroom
	 * 
	 */
	public void OpenBedroomDoor(){
		var sprite = GetNode<Sprite2D>("Sprite2D");
		var collision = GetNode<CollisionShape2D>("CollisionShape2D");
		GetNode<Sprite2D>("Sprite2D").Texture = DoorOpenTexture;
		sprite.Scale = new Vector2(2.0f, -1.662f);
		sprite.Offset = new Vector2(11, 0);
		collision.SetDeferred("disabled", true);

	}
}
