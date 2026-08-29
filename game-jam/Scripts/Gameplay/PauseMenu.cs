using Godot;

namespace GameJAM.Scripts.UI
{
	public partial class PauseMenu : Control
	{
		[Export] public Button ResumeButton;
		[Export] public Button OptionsButton;
		[Export] public Button QuitButton;

		private const string OptionsScenePath = "res://Scenes/OptionsMenuScene.tscn";
		private Node _optionsInstance = null;

		public override void _Ready()
		{
			Visible = false;

			if (ResumeButton != null)
				ResumeButton.Pressed += OnResumeButtonPressed;

			if (OptionsButton != null)
				OptionsButton.Pressed += OnOptionsButtonPressed;

			if (QuitButton != null)
				QuitButton.Pressed += OnQuitButtonPressed;
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			if (@event.IsActionPressed("ui_cancel"))
			{
				// Se a tela de opções estiver aberta como filha, fecha ela primeiro
				if (_optionsInstance != null && IsInstanceValid(_optionsInstance))
				{
					_optionsInstance.QueueFree();
					_optionsInstance = null;
				}
				else
				{
					TogglePause();
				}
			}
		}

		public void TogglePause()
		{
			bool isPaused = !GetTree().Paused;
			GetTree().Paused = isPaused;
			Visible = isPaused;

			// Se fechar o pause, garante que fecha as opções junto
			if (!isPaused && _optionsInstance != null && IsInstanceValid(_optionsInstance))
			{
				_optionsInstance.QueueFree();
				_optionsInstance = null;
			}
		}

		private void OnResumeButtonPressed()
		{
			GetTree().Paused = false;
			Visible = false;
		}

		private void OnOptionsButtonPressed()
		{
			// Se já existir uma instância de opções aberta, não recria
			if (_optionsInstance != null && IsInstanceValid(_optionsInstance)) return;

			PackedScene optionsScene = GD.Load<PackedScene>(OptionsScenePath);
			if (optionsScene != null)
			{
				_optionsInstance = optionsScene.Instantiate();
				// ProcessMode = Always permite interagir com as opções com o jogo pausado
				_optionsInstance.ProcessMode = ProcessModeEnum.Always;
				AddChild(_optionsInstance);
			}
			else
			{
				GD.PrintErr($"[PauseMenu] Não foi possível carregar a cena em: {OptionsScenePath}");
			}
		}

		private void OnQuitButtonPressed()
		{
			GetTree().Paused = false;
			GetTree().ChangeSceneToFile("res://Scenes/MainMenuScene.tscn");
		}
	}
}
