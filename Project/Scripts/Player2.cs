using Godot;

public partial class Player2 : Player
{
	public override void _Ready()
	{
		_stageHeight = GetViewportRect().Size.Y;
		PaddleHeight = GetNode<ColorRect>("Player2Rect").Size.Y;
		_up = "p2_ui_up";
		_down = "p2_ui_down";
	}
}