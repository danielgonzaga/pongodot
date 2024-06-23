using Godot;

public partial class Player : StaticBody2D
{
	public float PaddleHeight = 0;
	private const int PADDLE_SPEED = 500;
	protected float _stageHeight = 0;
	protected string _up;
	protected string _down;

	public override void _Process(double delta)
	{
		Vector2 position = Position;

		if (Input.IsActionPressed(_up) && position.Y > 0)
		{
			position.Y -= PADDLE_SPEED * (float)delta;
			Position = position;
		}

		if (Input.IsActionPressed(_down) && position.Y < (_stageHeight - PaddleHeight))
		{
			position.Y += PADDLE_SPEED * (float)delta;
			Position = position;
		}
	}
}
