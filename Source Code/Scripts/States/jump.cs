using Godot;
using System;
using Scripts.Patterns;
using Scripts.LevelGenerationFunctions;

public class jump : State<PlayerController>
{
    private RandomMethods rand = new RandomMethods();
    public override void Enter()
    {
        rand.createRandom();
        if(rand.random(0, 1) == 0)
        {
            node.jumpSound.Play();
        }
        else
        {
            node.jump2Sound.Play();
        }
        node.setAnim("jump");
        node.setYvelocity(node.jumpVelocity);
        node.MoveAndSlide(node.getVelocity(), Vector2.Up);
    }

    public override int PhyicsProcess(float delta)
    {
        int direction = 0;
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
            velocity.x = Mathf.Lerp(velocity.x, PlayerController.moveSpeed * direction, PlayerController.acceleration);
            //velocity.x = PlayerController.moveSpeed * direction;
            node.flipSprite();
        }
        else {
            velocity.x = Mathf.Lerp(velocity.x, 0, PlayerController.friction);
        }
        if (node.IsOnCeiling())
        {
            velocity.y = 0;
        }
        //velocity.y = node.jumpVelocity;
        velocity.y += node.getGravity() * delta;

        if (Input.IsActionPressed("ui_down"))
        {
            velocity.y += 20.0f;
        }



        node.MoveAndSlide(velocity, Vector2.Up);


        node.setDirection(direction);
        node.setVelocity(velocity);

        if (node.nextToWall() && Input.IsActionJustPressed("jump"))
        {
            return (int)PlayerController.playerStates.WALLJUMP;
        }
        if (Input.IsActionPressed("climb") && node.nextToWall() && node.getCanClimb())
        {
            return (int)PlayerController.playerStates.CLIMB;
        }
        if (Input.IsActionJustPressed("dash") && node.getDashCounter() > 0)
        {
            return (int)PlayerController.playerStates.DASH;
        }
        if (velocity.y > 0)
        {
            return (int)PlayerController.playerStates.FALL;
        }
        if (node.IsOnFloor())
        {
            if (direction != 0)
            {
                return (int)PlayerController.playerStates.WALK;
            }
            return (int)PlayerController.playerStates.IDLE;
        }
        return (int)PlayerController.playerStates.JUMP;
    }
}
