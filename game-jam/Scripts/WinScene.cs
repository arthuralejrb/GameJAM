using Godot;
using System;

public partial class WinScene : Node2D
{
	public void OnMenuButtonPressed() {
		GetTree().ChangeSceneToFile("res://Scenes/MainMenuScene.tscn");
		
	}
}
