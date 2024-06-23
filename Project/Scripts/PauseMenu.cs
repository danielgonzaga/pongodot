using Godot;

public partial class PauseMenu : Control
{
	private MenuItem _resumeLabel;
	private MenuItem _quitLabel;

	public override void _Ready()
	{
		_resumeLabel = GetNode<MenuItem>("VerticalMenu/VerticalMenuMargin/VerticalMenuBox/ResumeLabel");
		_resumeLabel.Connect(nameof(MenuItem.CursorSelectedEventHandler), new Callable(this, nameof(OnResumeCursorSelected)));

		_quitLabel = GetNode<MenuItem>("VerticalMenu/VerticalMenuMargin/VerticalMenuBox/QuitLabel");
		_quitLabel.Connect(nameof(MenuItem.CursorSelectedEventHandler), new Callable(this, nameof(OnQuitCursorSelected)));
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ui_cancel"))
		{
			Pause();
		}
	}

	private void Pause()
	{
		GetParent<CanvasLayer>().Visible = true;
		GetTree().Paused = true;
	}

	public void Resume()
	{
		GetParent<CanvasLayer>().Visible = false;
		GetTree().Paused = false;
	}

	public void Quit()
	{
		if (GetTree().Paused)
		{
			LoadMainMenuScene();
		}
	}

	public void OnResumeCursorSelected()
	{
		Resume();
	}

	public void OnQuitCursorSelected()
	{
		Quit();
	}

	private void LoadMainMenuScene()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
	}
}
