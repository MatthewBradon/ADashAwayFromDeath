using Godot;
using System;
using Scripts.Patterns;
public class idle : State<PlayerController>
{

    public override void Enter()
    {
        node.setDashCounter(node.MAX_DASH);
        node.setCanClimb(true);
        node.setAnim("idle");
    }
    public override int PhyicsProcess(float delta)
    {
        Vector2 velocity = Vector2.Zero;
        node.MoveAndSlide(velocity, Vector2.Up);
        node.setVelocity(velocity);


        if (Input.IsActionJustPressed("dash") && node.getDashCounter() > 0)
        {
            return (int)PlayerController.playerStates.DASH;
        }
        if (Input.IsActionPressed("climb") && node.nextToWall() && node.getCanClimb())
        {
            return (int)PlayerController.playerStates.CLIMB;
        }
        if (!node.onFloor())
        {
            node.wasOnFloor = true;
            node.setYvelocity(0);
            return (int)PlayerController.playerStates.FALL;
        }
        if (Input.IsActionJustPressed("jump"))
        {
            return (int)PlayerController.playerStates.JUMP;
        }
        if (Input.IsActionJustPressed("ui_left") || Input.IsActionJustPressed("ui_right"))
        {
            return (int)PlayerController.playerStates.WALK;
        }


        return (int)PlayerController.playerStates.IDLE;
    }
}
