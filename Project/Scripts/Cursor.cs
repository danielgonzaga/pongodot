using Godot;

public partial class Cursor : TextureRect
{
	[Export] public NodePath MenuParentPath { get; set; }
	[Export] public Vector2 CursorOffset { get; set; }

	private Control _menuParent;
	private int _cursorIndex = 0;

	public override void _Ready()
	{
		_menuParent = GetNode<Control>(MenuParentPath);
	}

	public override void _Process(double delta)
	{
		Vector2 input = Vector2.Zero;

		if (Input.IsActionJustPressed("ui_up"))
		{
			input.Y -= 1;
		}
		if (Input.IsActionJustPressed("ui_down"))
		{

			input.Y += 1;
		}
		if (Input.IsActionJustPressed("ui_left"))
		{
			input.X -= 1;
		}
		if (Input.IsActionJustPressed("ui_right"))
		{
			input.X += 1;
		}

		if (_menuParent is VBoxContainer)
		{
			SetCursorFromIndex(_cursorIndex + (int)input.Y);
		}
		else if (_menuParent is HBoxContainer)
		{
			SetCursorFromIndex(_cursorIndex + (int)input.X);
		}
		else if (_menuParent is GridContainer gridContainer)
		{
			SetCursorFromIndex(_cursorIndex + (int)input.X + (int)input.Y * gridContainer.Columns);
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			Control currentMenuItem = GetMenuItemAtIndex(_cursorIndex);

			if (currentMenuItem != null && currentMenuItem.HasMethod("CursorSelect"))
			{
				currentMenuItem.Call("CursorSelect");
			}
		}
	}

	private Control GetMenuItemAtIndex(int index)
	{
		if (_menuParent == null)
		{
			return null;
		}

		if (index >= _menuParent.GetChildCount() || index < 0)
		{
			return null;
		}

		return _menuParent.GetChild<Control>(index);
	}

	private void SetCursorFromIndex(int index)
	{
		Control menuItem = GetMenuItemAtIndex(index);

		if (menuItem == null)
		{
			return;
		}

		Vector2 position = menuItem.GlobalPosition;
		Vector2 size = menuItem.Size;

		GlobalPosition = new Vector2(position.X, position.Y + size.Y / 2.0f) - (Size / 2.0f) - CursorOffset;

		_cursorIndex = index;
	}

	public void UpdateMenuParent(NodePath newMenuParentPath)
	{
		MenuParentPath = newMenuParentPath;
		_menuParent = GetNode<Control>(MenuParentPath);
		_cursorIndex = 0;
		SetCursorFromIndex(0);
	}
}
