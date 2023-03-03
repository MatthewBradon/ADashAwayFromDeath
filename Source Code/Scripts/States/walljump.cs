using Godot;
using System;
using Scripts.Patterns;
public class walljump : State<PlayerController>
{
	private bool isWallJumping = false;
	private float xjumpSpeed = 250f;
	private float yjumpSpeed = 200f;
	private float wallJumpTimer = 0.2f;
	private float wallJumpTimerReset = 0.2f;
	
	public override void Enter()
	{
		node.jumpSound.Play();
		node.setAnim("jump");
		processWallJump();
		node.MoveAndSlide(node.getVelocity(), Vector2.Up);
	}

	public override int PhyicsProcess(float delta)
	{

		if (isWallJumping)
		{
			wallJumpTimerHandler(delta);
			return (int)PlayerController.playerStates.WALLJUMP;
		}

		if (Input.IsActionPressed("dash") && node.getDashCounter() > 0)
		{
			return (int)PlayerController.playerStates.DASH;
		}
		if (Input.IsActionPressed("climb") && node.nextToWall() && node.getCanClimb())
		{
			return (int)PlayerController.playerStates.CLIMB;
		}

		return (int)PlayerController.playerStates.FALL;
	}

	private void processWallJump()
	{
		var velocity = node.getVelocity();
		isWallJumping = true;

		if (node.GetNode<RayCast2D>("WallJumpRaycast").RotationDegrees == 90)
		{
			velocity.x = xjumpSpeed;
		}
		else
		{
			velocity.x = -xjumpSpeed;
		}

		velocity.y = -yjumpSpeed;
		node.setAnim("jump");
		node.setVelocity(velocity);
		node.flipSprite();



	}
	private void wallJumpTimerHandler(float delta)
	{
		wallJumpTimer -= delta;
		node.MoveAndSlide(node.getVelocity(), Vector2.Up);
		if (wallJumpTimer <= 0)
		{
			isWallJumping = false;
			wallJumpTimer = wallJumpTimerReset;
		}
	}
}
