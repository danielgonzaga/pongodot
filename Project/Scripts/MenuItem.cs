using Godot;

public partial class MenuItem : Label
{
	[Signal]
	public delegate void CursorSelectedEventHandler();

	public override void _Ready()
	{
		if (!HasSignal(nameof(CursorSelectedEventHandler)))
		{
			AddUserSignal(nameof(CursorSelectedEventHandler));
		}
	}

	public void CursorSelect()
	{
		EmitSignal(nameof(CursorSelectedEventHandler));
	}
}