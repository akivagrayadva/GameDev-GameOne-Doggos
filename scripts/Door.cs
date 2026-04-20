using Godot;

public partial class Door : StaticBody2D
{
	public enum DoorRoom { LivingRoom, Kitchen } // add more rooms as needed

	[Export] public DoorRoom Room { get; set; }
	[Export] public Texture2D DoorClosedTexture { get; set; }
	[Export] public Texture2D DoorOpenTexture { get; set; }

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

	public void OpenKitchenDoor(){
		var sprite = GetNode<Sprite2D>("Sprite2D");
		var collision = GetNode<CollisionShape2D>("CollisionShape2D");
		GetNode<Sprite2D>("Sprite2D").Texture = DoorOpenTexture;
		sprite.Scale = new Vector2(2.0f, -1.662f);
		sprite.Offset = new Vector2(11, 0);
		collision.SetDeferred("disabled", true);

	}
	
	public void OpenBedroomDoor(){
		var sprite = GetNode<Sprite2D>("Sprite2D");
		var collision = GetNode<CollisionShape2D>("CollisionShape2D");
		GetNode<Sprite2D>("Sprite2D").Texture = DoorOpenTexture;
		sprite.Scale = new Vector2(2.0f, -1.662f);
		sprite.Offset = new Vector2(11, 0);
		collision.SetDeferred("disabled", true);

	}
}
