using Godot;
using System;
using Scripts.Patterns;
public class climb : State<PlayerController>
{
    private float climbSpeed = 200f;

    private bool isClimbing = false;

    private float climbTimer = 4f;
    private float climbTimerReset = 4f;

    public override void Enter()
    {
        node.setAnim("climb");
    }

    public override int PhyicsProcess(float delta)
    {

        processClimbing(delta);

        if (isClimbing)
        {
            climbTimerHandler(delta);

            if (Input.IsActionJustPressed("jump"))
            {
                return (int)PlayerController.playerStates.WALLJUMP;
            }

            return (int)PlayerController.playerStates.CLIMB;
        }
        if (Input.IsActionPressed("dash") && node.getDashCounter() > 0)
        {
            return (int)PlayerController.playerStates.DASH;
        }
        if(node.onFloor())
        {
            return (int)PlayerController.playerStates.IDLE;
        }
        return (int)PlayerController.playerStates.FALL;
    }

    private void climbTimerHandler(float delta)
    {
        climbTimer -= delta;
        //When timer is up player is and can no longer climb and the timer is reset for the next time
        if (climbTimer <= 0)
        {
            node.setCanClimb(false);
            isClimbing = false;
            climbTimer = climbTimerReset;
        }
    }
    private void processClimbing(float delta)
    {
        if (Input.IsActionPressed("climb") && node.getCanClimb() && node.nextToWall())
        {
            node.setAnim("climb");
            isClimbing = true;
            var velocity = node.getVelocity();

            if (Input.IsActionPressed("ui_up"))
            {
                velocity.y = -climbSpeed;
                node.playAnim();

            }
            else if (Input.IsActionPressed("ui_down"))
            {
                velocity.y = climbSpeed;
                
                node.playAnim();
            }
            else
            {
                velocity = Vector2.Zero;
                node.setAnim("climb");
                node.pauseAnim();
            }
            velocity.x = climbSpeed * 2 * (node.getFlipH() ? -1 : 1);
            node.MoveAndSlide(velocity, Vector2.Up);
            node.setVelocity(velocity);
        }
        else
        {
            isClimbing = false;
        }
    }
}
