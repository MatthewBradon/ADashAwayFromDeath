using Godot;
using System;

public class Options : Control
{
   	VBoxContainer MenuContainer;
	Button KeyboardButton;
	Label KeyboardLabel;
	Label ControllerLabel;
	public override void _Ready()
	{
		MenuContainer = GetNode<VBoxContainer>("MenuContainer");
		KeyboardButton = MenuContainer.GetNode<Button>("KeyboardButton");
		KeyboardButton.GrabFocus();
		KeyboardLabel = GetNode<Label>("KeyboardLabel");
		ControllerLabel = GetNode<Label>("ControllerLabel");
		KeyboardLabel.Visible = true;
	}
	
	private void _on_MenuButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/Menu.tscn");
	}
	
	private void _on_KeyboardButton_pressed()
	{
		KeyboardLabel.Visible = true;
		ControllerLabel.Visible = false;
	}
	
	private void _on_ControllerButton_pressed()
	{
		KeyboardLabel.Visible = false;
		ControllerLabel.Visible = true;
	}
}









