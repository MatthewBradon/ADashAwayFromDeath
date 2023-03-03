using Godot;
using System;

public class Reaper : KinematicBody2D
{
	const int MAX_FPS = 60;

	public KinematicBody2D leader;
	public float moveOffsetTime =  2f;

	private Vector2[] _positionBuffer;
	private float[] _timeBuffer;
	private int _oldestIndex;
	private int _newestIndex;
	private int bufferLength;
	milliSecTimer moveTimer;
	private Timer collisionTimer;

	Area2D area;
	CollisionShape2D collisionShape;
	AnimationPlayer animationPlayer;
	Sprite sprite;
	LevelCount levelCount;
	public override void _Ready()
	{
		//Disable collision for 3 seconds
		area = GetNode<Area2D>("Area2D");
		collisionShape = area.GetNode<CollisionShape2D>("CollisionShape2D");
		collisionShape.Disabled = true;
		collisionTimer = new Timer();
		collisionTimer.Connect("timeout", this, "EnableCollision");
		collisionTimer.WaitTime = 1.5f;
		collisionTimer.OneShot = true;
		AddChild(collisionTimer);
		collisionTimer.Start();

		
		
		levelCount = (LevelCount)GetNode("/root/LevelCountSingleton");
		moveOffsetTime = levelCount.getMoveOffsetTime();
		if(levelCount.getLevelCount() % 3 == 0 && moveOffsetTime > 0.5f){
			moveOffsetTime -= 0.1f;
			levelCount.setMoveOffsetTime(moveOffsetTime);
		}

		//Set up the buffers
		leader = GetNode<KinematicBody2D>("../Player");
		bufferLength = (int)Mathf.Ceil(moveOffsetTime * MAX_FPS);
		_positionBuffer = new Vector2[bufferLength];
		_timeBuffer = new float[bufferLength];

		_positionBuffer[0] = _positionBuffer[1] = leader.Position;
		_timeBuffer[0] = _timeBuffer[1] = 0;

		_oldestIndex = 0;
		_newestIndex = 1;


		//Play animation
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		sprite = GetNode<Sprite>("Sprite");
		animationPlayer.Play("spawn");

		moveTimer = new milliSecTimer();
		moveTimer.startTimer();
	}
	public override void _Process(float delta)
	{
		moveTimer.updateTimer(delta);
		// Insert newest position into our cache.
		// If the cache is full, overwrite the latest sample.
		int newIndex = (_newestIndex + 1) % _positionBuffer.Length;
		if (newIndex != _oldestIndex)
			_newestIndex = newIndex;

		_positionBuffer[_newestIndex] = leader.Position;
		_timeBuffer[_newestIndex] = moveTimer.timer / 1000.0f;

		// Skip ahead in the buffer to the segment containing our target time.
		float targetTime = moveTimer.timer / 1000.0f - moveOffsetTime;
		int nextIndex;
		while (_timeBuffer[nextIndex = (_oldestIndex + 1) % _timeBuffer.Length] < targetTime)
			_oldestIndex = nextIndex;

		// Interpolate between the two samples on either side of our target time.
		float span = _timeBuffer[nextIndex] - _timeBuffer[_oldestIndex];
		float progress = 0f;
		if (span > 0f)
		{
			progress = (targetTime - _timeBuffer[_oldestIndex]) / span;
		}

		Position = _positionBuffer[_oldestIndex].LinearInterpolate(_positionBuffer[nextIndex], progress);

	}
	public void EnableCollision(){
		animationPlayer.Play("Idle");
		collisionShape.Disabled = false;
	}
	public void _on_Area2D_body_entered(Node body)
	{
		if(body.Name == "Player"){
			GetTree().ChangeScene("res://Scenes/GameOver.tscn");
		}
	}
}
  
   
