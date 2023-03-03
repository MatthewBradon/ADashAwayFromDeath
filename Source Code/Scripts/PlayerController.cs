using Godot;
using System;
using Scripts.Patterns;

public class PlayerController : KinematicBody2D
{

	public enum playerStates { IDLE, WALK, JUMP, FALL, DASH, WALLJUMP, CLIMB };
	public const float moveSpeed = 200f;
	private int direction = 0;
	public float jumpVelocity;
	public float jumpGravity;
	public float fallGravity;
	private float JumpHeight = 150f;
	private float JumpTimeToPeak = 0.5f;
	private float JumpTimeToDescent = 0.4f;
	public const float friction = 0.2f;
	public const float acceleration = 0.5f;
	public const float gravityAcceleration = 10f;
	public int MAX_DASH = 1;
	private int dashCounter = 1;
	private bool canClimb = true;
	public bool onfloor = false;
	public bool wasOnFloor = false;
	private Vector2 velocity = new Vector2();

	private AnimationPlayer animationPlayer;
	private Sprite sprite;
	private StateManager<PlayerController> stateManager;

	public RayCast2D floorRaycast;
	public RayCast2D floorRaycast2;
	public RayCast2D wallRaycast;

	public AudioStreamPlayer jumpSound;
	public AudioStreamPlayer jump2Sound;
	public AudioStreamPlayer dashSound;
	public AudioStreamPlayer fallSound;

	private CollisionShape2D collisionShape;



	public override void _Ready()
	{
		//Initialize raycasts
		wallRaycast = GetNode<RayCast2D>("WallJumpRaycast");
		floorRaycast = GetNode<RayCast2D>("FloorRaycast");
		floorRaycast2 = GetNode<RayCast2D>("FloorRaycast2");

		//Calculations for jump and gravity
		jumpVelocity = ((2.0f * JumpHeight) / JumpTimeToPeak) * -1.0f;
		jumpGravity = ((-2.0f * JumpHeight) / (JumpTimeToPeak * JumpTimeToPeak)) * -1.0f;
		fallGravity = ((-2.0f * JumpHeight) / (JumpTimeToDescent * JumpTimeToDescent)) * -1.0f;

		//Initialize animated sprite
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		sprite = GetNode<Sprite>("PlayerSpriteSheet");


		//Collsion shape
		collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");

		//Initialize audio
		jumpSound = GetNode<AudioStreamPlayer>("Jump1Sound");
		jump2Sound = GetNode<AudioStreamPlayer>("Jump2Sound");
		dashSound = GetNode<AudioStreamPlayer>("DashSound");
		fallSound = GetNode<AudioStreamPlayer>("FallSound");

		//Initialize state manager
		stateManager = new StateManager<PlayerController>(this);

		//Add states to state manager
		stateManager.AddState((int)playerStates.WALK, new walk());
		stateManager.AddState((int)playerStates.IDLE, new idle());
		stateManager.AddState((int)playerStates.JUMP, new jump());
		stateManager.AddState((int)playerStates.FALL, new fall());
		stateManager.AddState((int)playerStates.DASH, new dash());
		stateManager.AddState((int)playerStates.WALLJUMP, new walljump());
		stateManager.AddState((int)playerStates.CLIMB, new climb());

		stateManager.SetCurrentState((int)playerStates.IDLE);
	}



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		stateManager.executeStatePhysics(delta);
		collisionShape.Scale *= sprite.FlipH ? 1 : -1;
		GD.Print("Current State: " + stateManager.GetCurrentState());
	}
	

	public bool nextToWall()
	{
		return wallRaycast.IsColliding();
	}
	public bool onFloor()
	{
		return floorRaycast.IsColliding() || floorRaycast2.IsColliding();





		
	}
	public void setAnim(String animName)
	{
		//animatedSprite.Play(animName);
		animationPlayer.Play(animName);
	}
	public void playAnim()
	{
		//animatedSprite.Play();
		animationPlayer.Play();
	}
	public void pauseAnim()
	{
		//animatedSprite.Stop();
		animationPlayer.Stop();
	}
	public void flipSprite()
	{
		if (velocity.x > 0)
		{
			//animatedSprite.FlipH = false;
			sprite.FlipH = false;
			GetNode<RayCast2D>("WallJumpRaycast").RotationDegrees = -90;
		}
		else if (velocity.x < 0)
		{
			//animatedSprite.FlipH = true;
			sprite.FlipH = true;
			GetNode<RayCast2D>("WallJumpRaycast").RotationDegrees = 90;
		}
	}
	public void setXvelocity(float x)
	{
		velocity.x = x;
	}
	public void setYvelocity(float y)
	{
		velocity.y = y;
	}
	public void setDirection(int dir)
	{
		direction = dir;
	}
	public Vector2 getVelocity()
	{
		return velocity;
	}
	public int getDirection()
	{
		return direction;
	}
	public float getGravity()
	{
		return velocity.y > 0 ? jumpGravity : fallGravity;
	}
	public void setVelocity(Vector2 vel)
	{
		velocity = vel;
	}
	public bool getFlipH()
	{
		//return animatedSprite.FlipH;
		return sprite.FlipH;
	}

	public int getDashCounter()
	{
		return dashCounter;
	}
	public void setDashCounter(int dash)
	{
		dashCounter = dash;
	}
	public bool getCanClimb()
	{
		return canClimb;
	}
	public void setCanClimb(bool climb)
	{
		canClimb = climb;
	}	
}



