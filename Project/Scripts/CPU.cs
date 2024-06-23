using System;
using Godot;

public partial class CPU : StaticBody2D
{
	public int PaddleSpeed = 500;
	public int MoveLimit = 30;
	public float PaddleHeight = 0;
	private float _stageHeight = 0;
	private float _moveBy;
	private float _distanceFromBall;
	private Ball _ball;
	private Timer _respawnTimer;

	public override void _Ready()
	{
		_stageHeight = GetViewportRect().Size.Y;
		PaddleHeight = GetNode<ColorRect>("CPURect").Size.Y;
		_ball = GetNode<Ball>("../Ball");
		_respawnTimer = GetNode<Timer>("../RespawnTimer");
	}
	public override void _Process(double delta)
	{
		Vector2 position = Position;

		if (!_respawnTimer.IsStopped() || _ball.BallDirection.X < 0)
		{
			return;
		}

		Vector2 ballPosition = _ball.Position;
		_distanceFromBall = ballPosition.Y - position.Y - (PaddleHeight / 2);
		float deltaPaddleSpeed = PaddleSpeed * (float)delta;

		if (_distanceFromBall / Math.Abs(_distanceFromBall) < deltaPaddleSpeed)
		{
			_moveBy = Mathf.Clamp(_distanceFromBall / Math.Abs(_distanceFromBall) * deltaPaddleSpeed, -deltaPaddleSpeed, deltaPaddleSpeed);
		}

		if (Math.Abs(_distanceFromBall - _moveBy) > MoveLimit)
		{
			position.Y += _moveBy;
			position.Y = Mathf.Clamp(position.Y, 0, _stageHeight - PaddleHeight);
			Position = position;
		}
	}
}
