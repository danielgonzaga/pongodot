using Godot;
using DomainEnums;

public partial class GameplayController : Node2D
{
	public GameplayMode GameMode;
	private int[] _score = { 0, 0 };
	private bool _hasPointed = false;
	private Timer _respawnTimer;
	private Ball _ball;
	private Area2D _leftPointArea;
	private Area2D _rightPointArea;
	private Label _leftScore;
	private Label _rightScore;
	private CPU _cpu;
	private Player2 _player2;
	private AudioStreamPlayer2D _scoreSound;

	public override void _Ready()
	{
		_ball = GetNode<Ball>("Ball");

		_respawnTimer = GetNode<Timer>("RespawnTimer");
		_respawnTimer.Timeout += OnRespawnTimerTimeout;

		_leftPointArea = GetNode<Area2D>("LeftPointArea");
		_leftScore = GetNode<Label>("Score/LeftScore");
		_leftPointArea.BodyEntered += OnLeftPointAreaBodyEntered;

		_rightPointArea = GetNode<Area2D>("RightPointArea");
		_rightScore = GetNode<Label>("Score/RightScore");
		_rightPointArea.BodyEntered += OnRightPointAreaBodyEntered;

		_scoreSound = GetNode<AudioStreamPlayer2D>("ScoreSound");
		_cpu = GetNode<CPU>("CPU");
		_player2 = GetNode<Player2>("Player2");

		SetGameMode();
	}

	private void SetGameMode()
	{
		switch (GameMode)
		{
			case GameplayMode.Versus:
				_cpu.SetProcess(false);
				_cpu.Visible = false;
				_cpu.Position = new Vector2(1280, 274);
				_player2.SetProcess(true);
				_player2.Visible = true;
				_player2.Position = new Vector2(1107, 274);
				break;
			case GameplayMode.Easy:
				_player2.Visible = false;
				_player2.SetProcess(false);
				_player2.Position = new Vector2(1280, 274);
				_cpu.PaddleSpeed = 200;
				_cpu.MoveLimit = 5;
				_cpu.SetProcess(true);
				_cpu.Visible = true;
				_cpu.Position = new Vector2(1107, 274);
				break;
			case GameplayMode.Normal:
				_player2.Visible = false;
				_player2.SetProcess(false);
				_player2.Position = new Vector2(1280, 274);
				_cpu.PaddleSpeed = 350;
				_cpu.MoveLimit = 10;
				_cpu.SetProcess(true);
				_cpu.Visible = true;
				_cpu.Position = new Vector2(1107, 274);
				break;
			case GameplayMode.Hard:
				_player2.Visible = false;
				_player2.SetProcess(false);
				_player2.Position = new Vector2(1280, 274);
				_cpu.PaddleSpeed = 500;
				_cpu.MoveLimit = 30;
				_cpu.SetProcess(true);
				_cpu.Visible = true;
				_cpu.Position = new Vector2(1107, 274);
				break;
		}
	}

	private void OnRespawnTimerTimeout()
	{
		_hasPointed = false;
		_ball.BallRespawn();
	}

	private void OnLeftPointAreaBodyEntered(Node2D body)
	{
		if (!_hasPointed)
		{
			_scoreSound.Play();
			IncrementPoint(1, _rightScore);
		}
	}

	private void OnRightPointAreaBodyEntered(Node2D body)
	{
		if (!_hasPointed)
		{
			_scoreSound.Play();
			IncrementPoint(0, _leftScore);
		}
	}

	private void IncrementPoint(int index, Label label)
	{
		_hasPointed = true;
		_score[index] += 1;
		label.Text = _score[index].ToString();
		_respawnTimer.Start();
	}

	public void SetGameMode(GameplayMode mode)
	{
		GameMode = mode;
	}
}
