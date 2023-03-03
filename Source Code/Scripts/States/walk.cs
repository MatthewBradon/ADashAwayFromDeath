using Godot;
using System;
using Scripts.Patterns;

public class walk : State<PlayerController>
{
    public override void Enter()
    {
        node.setDashCounter(node.MAX_DASH);
        node.setCanClimb(true);
        node.setAnim("walk");
    }
    public override int PhyicsProcess(float delta)
    {
        node.setDirection(0);
        int direction = node.getDirection();
        Vector2 velocity = node.getVelocity();

        if (Input.IsActionPressed("ui_right"))
        {
            direction = 1;
            velocity.x = Mathf.Lerp(velocity.x, PlayerController.moveSpeed * direction, PlayerController.acceleration);
            node.flipSprite();
        }
        else if (Input.IsActionPressed("ui_left"))
        {
            direction = -1;
            velocity.x = Mathf.Lerp(velocity.x, PlayerController.moveSpeed * direction, PlayerController.acceleration);
            node.flipSprite();
        }

        velocity.y = 0;

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
        if (Input.IsActionJustPressed("jump"))
        {
            return (int)PlayerController.playerStates.JUMP;
        }
        if (direction == 0)
        {
            return (int)PlayerController.playerStates.IDLE;
        }
        if (!node.onFloor())
        {
            node.wasOnFloor = true;
            return (int)PlayerController.playerStates.FALL;
        }


        return (int)PlayerController.playerStates.WALK;
    }
}
