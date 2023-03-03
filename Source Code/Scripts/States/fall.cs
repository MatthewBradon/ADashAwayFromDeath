using Godot;
using System;
using Scripts.Patterns;
public class fall : State<PlayerController>
{


    private float coyoteTime = 0.1f;
    private float coyoteTimerReset = 0.1f;
    public bool coyoteTimeActive = false;

    public override void Enter()
    {
        node.setAnim("fall");
        node.MoveAndSlide(Vector2.Zero, Vector2.Up);
    }

    public override int PhyicsProcess(float delta)
    {
        
        if (node.wasOnFloor)
        {
            coyoteTimeActive = true;
        }
        coyoteTimer(delta);

        if (coyoteTimeActive && Input.IsActionJustPressed("jump"))
        {
            coyoteTime = 0;
            return (int)PlayerController.playerStates.JUMP;
        }


        int direction = node.getDirection();
        Vector2 velocity = node.getVelocity();
        if (Input.IsActionPressed("ui_right"))
        {
            direction = 1;
            velocity.x = Mathf.Lerp(velocity.x, PlayerController.moveSpeed * direction, PlayerController.acceleration);
            //velocity.x = PlayerController.moveSpeed * direction;
            node.flipSprite();
        }
        else if (Input.IsActionPressed("ui_left"))
        {
            direction = -1;
            velocity.x = PlayerController.moveSpeed * direction;
            //velocity.x = Mathf.Lerp(velocity.x, PlayerController.moveSpeed * direction, PlayerController.acceleration);
            node.flipSprite();
        }
        else {
            velocity.x = Mathf.Lerp(velocity.x, 0, PlayerController.friction);
        }


        //velocity.y += node.getGravity() * delta;
        velocity.y += node.getGravity() * delta;

        if (Input.IsActionPressed("ui_down"))
        {
            velocity.y += 20.0f;
        }
        node.MoveAndSlide(velocity, Vector2.Up);

        node.setDirection(direction);
        node.setVelocity(velocity);


        if (Input.IsActionJustPressed("dash") && node.getDashCounter() > 0)
        {
            return (int)PlayerController.playerStates.DASH;
        }
        if (Input.IsActionPressed("climb") && node.nextToWall() && node.getCanClimb())
        {
            return (int)PlayerController.playerStates.CLIMB;
        }
        if (node.onFloor())
        {
            node.fallSound.Play();
            if (Input.IsActionJustPressed("jump"))
            {
                return (int)PlayerController.playerStates.JUMP;
            }
            if (direction != 0)
            {
                return (int)PlayerController.playerStates.WALK;
            }
            return (int)PlayerController.playerStates.IDLE;
        }
        if (Input.IsActionJustPressed("jump") && node.nextToWall())
        {
            return (int)PlayerController.playerStates.WALLJUMP;
        }

        return (int)PlayerController.playerStates.FALL;
    }

    public void coyoteTimer(float delta)
    {
        node.wasOnFloor = false;
        if (!coyoteTimeActive) return;

        coyoteTime -= delta;
        if (coyoteTime <= 0)
        {
            coyoteTimeActive = false;
            coyoteTime = coyoteTimerReset;

        }
    }
}

