using Godot;
using System.Collections.Generic;

namespace GameJAM.Scripts.Gameplay
{
public partial class UIManager : CanvasLayer
{
	[Export] public Label PlayerScoreLabel;
	[Export] public Label DealerScoreLabel;
	[Export] public Label BankRollLabel;
	[Export] public Label DebtLabel;
	[Export] public Label BetLabel;
	[Export] public Label ResultLabel;
	
	[Export] public Button HitButton;
	[Export] public Button StandButton;
	[Export] public Button TrashButton;
	[Export] public Button NextRoundButton;

	[Export] public HBoxContainer PlayerHandContainer;
	[Export] public HBoxContainer DealerHandContainer;
	
	[Export] public Control PauseMenuControl;
	[Export] public Button ResumeButton;
	[Export] public Button	OptionsButton;
	[Export] public Button QuitButton;
	
	private const string _overlayPath = "res://Scenes/options.tscn"; 

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			TogglePause();
			GetViewport().SetInputAsHandled();

		}
	}


	public void TogglePause()
	{
	
		PauseMenuControl.Visible = !PauseMenuControl.Visible;
		GetTree().Paused = PauseMenuControl.Visible;
	
	}


	public void OnResumeButtonPressed()
	{
		TogglePause();

	}

	
	public void OnOptionsButtonPressed()
	{
		ShowOverlay();
		
	}


	public void OnQuitButtonPressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://Scenes/menu_principal.tscn");

	}


	public void UpdateScores(int playerScore, int dealerScore, bool isReal = false)
	{
		string prefix = isReal ? "REAL: " : "";
		PlayerScoreLabel.Text = $"Player Score {prefix}{playerScore}";
		DealerScoreLabel.Text = $"Dealer Score {prefix}{dealerScore}";
	}


	public void UpdateEconomy(int bankroll, int debt, int bet, int wins, string message = "")
	{
		BankRollLabel.Text = $"Bankroll: {bankroll}";
		DebtLabel.Text = $"Total Debt: {debt}";
		BetLabel.Text = $"Actual Bet: {bet}";
		
		if (string.IsNullOrEmpty(message))
			ResultLabel.Text = $"Wins: {wins}";
		else
			ResultLabel.Text = $"{message} (Wins: {wins})";
	}


	public void ToggleActionButtons(bool enabled)
	{
		HitButton.Disabled = !enabled;
		StandButton.Disabled = !enabled;
		if (TrashButton != null) TrashButton.Disabled = !enabled;
	}


	public void ShowNextButton(bool show)
	{
		if (NextRoundButton != null)
		{
			NextRoundButton.Visible = show;
			NextRoundButton.Disabled = !show;
	
		}
	}


	public void ShowOverlay()
	{
		PackedScene overlayScene = GD.Load<PackedScene>(_overlayPath);

		if(overlayScene != null)
		{
			var overlayInstance = overlayScene.Instantiate<Node>();
			overlayInstance.ProcessMode = ProcessModeEnum.Always;
			AddChild(overlayInstance);		
		
		}
	}


	public void RenderHand(List<Card> hand, HBoxContainer container, bool hideSecretValues)
	{
		foreach (Node child in container.GetChildren())
		{
			child.QueueFree();
		}

		foreach (Card card in hand)
		{
			TextureRect cardSprite = new TextureRect();
			cardSprite.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
			cardSprite.CustomMinimumSize = new Vector2(100, 140);
			cardSprite.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;

			string valToLoad = hideSecretValues ? card.visibleValue.ToString() : card.realValue.ToString();
			string spritePath = $"res://Assets/Cartas/{card.cardSuit}/{valToLoad}.png";

			if (ResourceLoader.Exists(spritePath))
			{
				cardSprite.Texture = GD.Load<Texture2D>(spritePath);
			}

			if (!hideSecretValues && card.cardType == CardType.Illusory)
			{
				Label trapTag = new Label();
				trapTag.Text = $"REAL: {card.realValue}";
				trapTag.AddThemeColorOverride("font_color", Colors.Red);
				cardSprite.AddChild(trapTag);
			}

			container.AddChild(cardSprite);
		}
	}
}
}
