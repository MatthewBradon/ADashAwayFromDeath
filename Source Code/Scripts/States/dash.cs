using Godot;
using System;
using Scripts.Patterns;

public class dash : State<PlayerController>
{
    private bool isDashing = false;
    private float dashTimer = 0.2f;
    private float dashTimerReset = 0.2f;
    private float dashSpeed = 400f;
    
    public override void Enter()
    {
        

        Vector2 velocity = node.getVelocity();
        velocity = Vector2.Zero;

        node.MoveAndSlide(velocity);

        node.setVelocity(velocity);
        node.dashSound.Play();
        node.setAnim("dash");
        //isDashing = true;

    }
    public override int PhyicsProcess(float delta)
    {
        if (node.getDashCounter() > 0)
        {

            processDash(delta);
        }

        dashTimerHandler(delta);

        node.MoveAndSlide(node.getVelocity(), Vector2.Up);

        if (!isDashing)
        {
            return (int)PlayerController.playerStates.FALL;
        }

        return (int)PlayerController.playerStates.DASH;
    }

    private void dashTimerHandler(float delta)
    {

        dashTimer -= delta;
        
        if (dashTimer <= 0)
        {
            isDashing = false;
            node.setVelocity(Vector2.Zero);
        }
    }
    private void processDash(float delta)
    {
        Vector2 velocity = node.getVelocity();
        int dashCounter = node.getDashCounter();

        if (!Input.IsActionPressed("dash"))
        {
            isDashing = false;
        }
        if (Input.IsActionPressed("ui_up") && Input.IsActionPressed("ui_left"))
        {
            velocity.y -= dashSpeed;
            velocity.x -= dashSpeed;
        }
        else if (Input.IsActionPressed("ui_up") && Input.IsActionPressed("ui_right"))
        {
            velocity.y -= dashSpeed;
            velocity.x += dashSpeed;
        }
        else if (Input.IsActionPressed("ui_left"))
        {
            velocity.x -= dashSpeed;

        }
        else if (Input.IsActionPressed("ui_right"))
        {
            velocity.x += dashSpeed;
        }
        else if (Input.IsActionPressed("ui_up"))
        {
            velocity.y -= dashSpeed;

        }
        else
        {
            velocity.x += dashSpeed * (node.getFlipH() ? -1 : 1);
        }

        dashCounter--;
        isDashing = true;
        //node.setAnim("dash");
        node.setDashCounter(dashCounter);

        node.setVelocity(velocity);

        dashTimer = dashTimerReset;

    }
}
