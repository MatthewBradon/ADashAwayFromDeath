using Godot;
using System;

public class PauseMenu : Control
{
    [Signal] public delegate void pause();
    Button resumeButton;
	VBoxContainer menuContainer;
    public override void  _Ready()
    {
        Visible = false;
        menuContainer = GetNode<VBoxContainer>("MenuContainer");
        resumeButton = menuContainer.GetNode<Button>("ResumeButton");
        
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            GetTree().Paused = !GetTree().Paused;
            Visible = !Visible;
            if(Visible){
                EmitSignal("pause");
                resumeButton.GrabFocus();
            }
        }
    }
    public void _on_ResumeButton_pressed()
    {
        GetTree().Paused = false;
        Visible = false;    
    }

    public void _on_QuitButton_pressed()
    {
        GetTree().Quit();
    }
}
