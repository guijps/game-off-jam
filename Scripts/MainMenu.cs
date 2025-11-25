using Godot;
using System;

public partial class MainMenu : Control
{
	[Export] PackedScene GameScene;

	public void OnButtonPresset()
    {
		GetTree().ChangeSceneToPacked(GameScene);
    }
}
