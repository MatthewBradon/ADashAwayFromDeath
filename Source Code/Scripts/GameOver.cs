using Godot;
using System;

public class GameOver : Control
{
    private VBoxContainer menuContainer;
    private TimerText timerText;
    private LevelCount levelCount;
    private Label timerLabel;
    private Label levelCountLabel;
    AudioStreamPlayer BackgroundMusic;
    private AudioStreamPlayer DeathSound;
    public override void _Ready()
    {
        levelCount = (LevelCount)GetNode("/root/LevelCountSingleton");
        timerText = (TimerText)GetNode("/root/TimerTextSingleton");
        timerText.stopTimer();

        menuContainer = GetNode<VBoxContainer>("MenuContainer");
        menuContainer.GetNode<Button>("ContinueButton").GrabFocus();
        timerLabel = GetNode<Label>("TimerLabel");
        levelCountLabel = GetNode<Label>("LevelCountLabel");

        timerLabel.Text += timerText.timer[0].ToString("00") + ":" + timerText.timer[1].ToString("00");
        levelCountLabel.Text += levelCount.getLevelCount().ToString();

        BackgroundMusic = GetNode<AudioStreamPlayer>("/root/BackgroundMusic");
		BackgroundMusic.Stop();

        DeathSound = GetNode<AudioStreamPlayer>("DeathSound");
        DeathSound.Play();

        
    }

    public void _on_ContinueButton_pressed()
    {
        timerText.resetTimer();
        levelCount.resetLevelCount();
        GetTree().ChangeScene("res://Scenes/MainScene.tscn");
    }

    public void _on_QuitButton_pressed()
    {
        
        GetTree().ChangeScene("res://Scenes/Menu.tscn");
    }
}
