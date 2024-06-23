using Godot;
using DomainEnums;
using System;

public partial class MainMenu : Control
{
	private PanelContainer _mainMenuPanel;
	private VBoxContainer _mainMenuVerticalBox;
	private PanelContainer _CPULevelPanel;
	private VBoxContainer _CPULevelVerticalBox;
	private MenuItem _vsCpu;
	private MenuItem _vs2P;
	private MenuItem _exit;

	private MenuItem _easy;
	private MenuItem _medium;
	private MenuItem _hard;
	private MenuItem _back;
	private Cursor _cursor;
	private PackedScene _gameplayScene;

	public override void _Ready()
	{
		_gameplayScene = (PackedScene)ResourceLoader.Load("res://Scenes/Gameplay.tscn");

		_mainMenuPanel = GetNode<PanelContainer>("MainMenuPanel");
		_mainMenuVerticalBox = (VBoxContainer)_mainMenuPanel.GetChild(0).GetChild(0);
		_CPULevelPanel = GetNode<PanelContainer>("CPULevelPanel");
		_CPULevelVerticalBox = (VBoxContainer)_CPULevelPanel.GetChild(0).GetChild(0);

		_mainMenuPanel.Visible = true;
		_CPULevelPanel.Visible = false;
		_cursor = GetNode<Cursor>("Cursor");

		_vsCpu = GetNode<MenuItem>("MainMenuPanel/MainMenuMargin/MainMenuVerticalBox/VsCPU");
		_vsCpu.Connect(nameof(MenuItem.CursorSelectedEventHandler), new Callable(this, nameof(OnVsCpuCursorSelected)));

		_vs2P = GetNode<MenuItem>("MainMenuPanel/MainMenuMargin/MainMenuVerticalBox/Vs2P");
		_vs2P.Connect(nameof(MenuItem.CursorSelectedEventHandler), new Callable(this, nameof(OnVs2PCursorSelected)));

		_exit = GetNode<MenuItem>("MainMenuPanel/MainMenuMargin/MainMenuVerticalBox/Exit");
		_exit.Connect(nameof(MenuItem.CursorSelectedEventHandler), new Callable(this, nameof(OnExitCursorSelected)));

		_easy = GetNode<MenuItem>("CPULevelPanel/CPULevelMargin/CPULevelVerticalBox/Easy");
		_easy.Connect(nameof(MenuItem.CursorSelectedEventHandler), new Callable(this, nameof(OnEasyCursorSelected)));

		_medium = GetNode<MenuItem>("CPULevelPanel/CPULevelMargin/CPULevelVerticalBox/Normal");
		_medium.Connect(nameof(MenuItem.CursorSelectedEventHandler), new Callable(this, nameof(OnNormalCursorSelected)));

		_hard = GetNode<MenuItem>("CPULevelPanel/CPULevelMargin/CPULevelVerticalBox/Hard");
		_hard.Connect(nameof(MenuItem.CursorSelectedEventHandler), new Callable(this, nameof(OnHardCursorSelected)));

		_back = GetNode<MenuItem>("CPULevelPanel/CPULevelMargin/CPULevelVerticalBox/Back");
		_back.Connect(nameof(MenuItem.CursorSelectedEventHandler), new Callable(this, nameof(OnBackCursorSelected)));
	}

	public void OnVsCpuCursorSelected()
	{
		if (!_mainMenuPanel.Visible) return;

		_mainMenuPanel.Visible = false;
		_CPULevelPanel.Visible = true;
		_cursor.UpdateMenuParent(_CPULevelVerticalBox.GetPath());
	}

	public void OnVs2PCursorSelected()
	{
		if (!_mainMenuPanel.Visible) return;

		LoadGameScene(GameplayMode.Versus);
	}

	public void OnExitCursorSelected()
	{
		if (!_mainMenuPanel.Visible) return;
		GetTree().Quit();
	}

	public void OnEasyCursorSelected()
	{
		if (!_CPULevelPanel.Visible) return;

		LoadGameScene(GameplayMode.Easy);
	}

	public void OnNormalCursorSelected()
	{
		if (!_CPULevelPanel.Visible) return;

		LoadGameScene(GameplayMode.Normal);
	}

	public void OnHardCursorSelected()
	{
		if (!_CPULevelPanel.Visible) return;

		LoadGameScene(GameplayMode.Hard);
	}

	public void OnBackCursorSelected()
	{
		if (!_CPULevelPanel.Visible) return;

		_CPULevelPanel.Visible = false;
		_mainMenuPanel.Visible = true;
		_cursor.UpdateMenuParent(_mainMenuVerticalBox.GetPath());
	}

	private async void LoadGameScene(GameplayMode mode)
	{
		var gameInstance = (GameplayController)_gameplayScene.Instantiate();

		gameInstance.SetGameMode(mode);

		GetTree().Root.AddChild(gameInstance);
		GetTree().CurrentScene = gameInstance;

		await ToSignal(GetTree(), "process_frame");

		var currentScene = GetTree().Root.GetChild(0);
		if (currentScene != gameInstance)
		{
			currentScene.QueueFree();
		}
	}
}
