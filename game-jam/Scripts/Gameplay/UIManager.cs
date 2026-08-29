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
	
	private const string _overlayPath = "res://Scenes/OptionsMenuScene.tscn";

	private Card _selectedCardData;
	private TextureButton _selectedCardButton;
	private readonly float _yOffset = -20f; // Quantos pixels a carta vai subir

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
		GetTree().ChangeSceneToFile("res://Scenes/MainMenuScene.tscn");

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
			TextureButton cardButton = new TextureButton();
			cardButton.IgnoreTextureSize = true;
			cardButton.CustomMinimumSize = new Vector2(100, 140);
			cardButton.StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered;

			// As variáveis de caminho precisam ser declaradas antes de configurar a textura
			string valToLoad = hideSecretValues ? card.visibleValue.ToString() : card.realValue.ToString();
			string spritePath = $"res://Assets/Cartas/{card.cardSuit}/{valToLoad}.png";

			if (ResourceLoader.Exists(spritePath))
			{
				cardButton.TextureNormal = GD.Load<Texture2D>(spritePath);
			}

			// Injeção de dependência via lambda conectando o clique à carta específica
			cardButton.Pressed += () => OnCardSelected(card, cardButton);

			if (!hideSecretValues && card.cardType == CardType.Illusory)
			{
				Label trapTag = new Label();
				trapTag.Text = $"REAL: {card.realValue}";
				trapTag.AddThemeColorOverride("font_color", Colors.Red);
				cardButton.AddChild(trapTag);
			}

			container.AddChild(cardButton);
		}
	}

	private void OnCardSelected(Card cardData, TextureButton buttonNode)
	{
		// Se clicar na mesma carta que já está selecionada, desmarque-a
		if (_selectedCardData == cardData)
		{
			buttonNode.Position = new Vector2(buttonNode.Position.X, buttonNode.Position.Y - _yOffset);
			_selectedCardData = null;
			_selectedCardButton = null;
			return;
		}

		// Se havia uma carta diferente selecionada antes, desça ela de volta ao normal
		if (_selectedCardButton != null)
		{
			_selectedCardButton.Position = new Vector2(_selectedCardButton.Position.X, _selectedCardButton.Position.Y - _yOffset);
		}

		// Marca a nova carta e faça ela subir
		_selectedCardData = cardData;
		_selectedCardButton = buttonNode;
		buttonNode.Position = new Vector2(buttonNode.Position.X, buttonNode.Position.Y + _yOffset);
	}

	public Card GetSelectedCard()
	{
		return _selectedCardData;
	}

	public void ClearSelection()
	{
		_selectedCardData = null;
		_selectedCardButton = null;
	}
}
}
