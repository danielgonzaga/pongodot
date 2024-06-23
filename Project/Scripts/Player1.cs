using Godot;

public partial class Player1 : Player
{
	public override void _Ready()
	{
		_stageHeight = GetViewportRect().Size.Y;
		PaddleHeight = GetNode<ColorRect>("Player1Rect").Size.Y;
		_up = "ui_up";
		_down = "ui_down";
	}
}