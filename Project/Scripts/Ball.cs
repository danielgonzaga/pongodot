using System;
using Godot;

public partial class Ball : CharacterBody2D
{
	public Vector2 BallDirection;
	const int BALL_START_SPEED = 400;
	const int ACCELERATION = 50;
	const int BALL_MAX_SPEED = 1500;
	private static readonly Random _random = new Random();
	private float _ballHeight;
	private int _ballSpeed;
	private Player1 _player1;
	private Player2 _player2;
	private CPU _cpu;
	private Vector2 _stageSize;
	private AudioStreamPlayer2D _paddleSound;
	private AudioStreamPlayer2D _wallSound;

	public override void _Ready()
	{
		_stageSize = GetViewportRect().Size;
		_ballHeight = GetNode<ColorRect>("BallRect").Size.Y;
		_player1 = GetNode<Player1>("../Player1");
		_player2 = GetNode<Player2>("../Player2");
		_cpu = GetNode<CPU>("../CPU");
		_paddleSound = GetNode<AudioStreamPlayer2D>("../PaddleSound");
		_wallSound = GetNode<AudioStreamPlayer2D>("../WallSound");
	}

	public override void _PhysicsProcess(double delta)
	{
		KinematicCollision2D collision = MoveAndCollide(BallDirection * _ballSpeed * (float)delta);
		GodotObject collider = collision.GetCollider();
		if (collider == _player1 || collider == _player2 || collider == _cpu)
		{
			_paddleSound.Play();
			Vector2 position = Position;

			// Ball hit in the top or bottom part of Player1 Paddle	
			if (position.X <= 44)
			{
				position.X = 51;
				Position = position;
				BallDirection = NewDirection(collider);
			}
			// Ball hit in the top or bottom part of Player2/CPU Paddle
			else if (position.X >= 1108)
			{
				position.X = 1101;
				Position = position;
				BallDirection = NewDirection(collider);
			}
			// Ball hit in the front of any Paddle
			else
			{
				if (_ballSpeed <= BALL_MAX_SPEED)
				{
					_ballSpeed += ACCELERATION;
				}
				BallDirection = NewDirection(collider);
			}
		}
		// Ball hit a wall
		else
		{
			_wallSound.Play();
			BallDirection = BallDirection.Bounce(collision.GetNormal());
		}
	}

	public void BallRespawn()
	{
		Vector2 position = Position;

		_ballSpeed = BALL_START_SPEED;

		position.X = _stageSize.X / 2;
		float minY = _ballHeight / 2;
		float maxY = _stageSize.Y - (_ballHeight / 2);
		position.Y = (float)(_random.NextDouble() * (maxY - minY) + minY);
		Position = position;

		BallDirection = GenerateRandomDirection();
	}

	private Vector2 GenerateRandomDirection()
	{
		float x = _random.Next(2) == 0 ? 1 : -1;
		float y = (float)(_random.NextDouble() * 2 - 1);
		Vector2 newDir = new Vector2(x, y);
		return newDir.Normalized();
	}

	private Vector2 NewDirection(GodotObject collider)
	{
		Vector2 newDirection = BallDirection;
		float ballY = Position.Y;

		if (collider is Player1 colliderNodePlayer1)
		{
			float paddleY = colliderNodePlayer1.Position.Y;
			float distance = -1 * (paddleY + (colliderNodePlayer1.PaddleHeight / 2) - ballY);

			newDirection.X = BallDirection.X > 0 ? -1 : 1;
			newDirection.Y = distance / colliderNodePlayer1.PaddleHeight;
		}

		if (collider is Player2 colliderNodePlayer2)
		{
			float paddleY = colliderNodePlayer2.Position.Y;
			float distance = -1 * (paddleY + (colliderNodePlayer2.PaddleHeight / 2) - ballY);

			newDirection.X = BallDirection.X > 0 ? -1 : 1;
			newDirection.Y = distance / colliderNodePlayer2.PaddleHeight;
		}

		if (collider is CPU colliderNodeCPU)
		{
			float paddleY = colliderNodeCPU.Position.Y;
			float distance = -1 * (paddleY + (colliderNodeCPU.PaddleHeight / 2) - ballY);

			newDirection.X = BallDirection.X > 0 ? -1 : 1;
			newDirection.Y = distance / colliderNodeCPU.PaddleHeight;
		}

		return newDirection.Normalized();
	}
}
