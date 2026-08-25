using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameJAM.Scripts.Gameplay
{
public partial class MainGameManager : Node2D
{
	private double _trapChance = 0.3;

	private Deck _deck = new Deck();
	private List<Card> _playerHand = new List<Card>();
	private List<Card> _dealerHand = new List<Card>();
	private Card _trashedCard = null;

	private int _playerScore = 0;
	private int _dealerScore = 0;

	private int _playerWins = 0;
	private int _dealerWins = 0;

	private int _bankRoll = 300;
	private int _totalDebt = 3000;
	private int _actualBet = 0;

	[Export] public Control Score;
	[Export] public Label PlayerScoreLabel;
	[Export] public Label DealerScoreLabel;
	[Export] public Control StatusControl;
	[Export] public Label BankRollLabel;
	[Export] public Label DebtLabel;
	[Export] public Label BetLabel;
	[Export] public Label ResultLabel;
	[Export] public Control ButtonsControl;
	[Export] public Button HitButton;
	[Export] public Button StandButton;
	[Export] public Button TrashButton;
	[Export] public HBoxContainer PlayerHandContainer;
	[Export] public HBoxContainer DealerHandContainer;	
	[Export] public Button NextRoundButton; 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		StartMatch();
		
	}


	public void StartMatch()
	{
		_deck.CreateDeck(_trapChance);
		StartRound();

	}

	public void StartRound()
	{
		_playerScore = 0;
		_dealerScore = 0;
		_playerHand.Clear();
		_dealerHand.Clear();

		_trashedCard = null;
	
		_playerHand.Add(_deck.DrawCard(_trapChance));
		_playerHand.Add(_deck.DrawCard(_trapChance));

		_dealerHand.Add(_deck.DrawCard(_trapChance));

		UpdateUI();

	}


	public void OnHitButtonPressed()
	{
		_playerHand.Add(_deck.DrawCard(_trapChance));
		UpdateUI();

	}

	public void OnStandButtonPressed()
	{
		HitButton.Disabled = true;
		StandButton.Disabled = true;
		if (TrashButton != null) TrashButton.Disabled = true;

		_playerScore = 0;
		foreach (Card card in _playerHand)
		{
			_playerScore += card.realValue;
		}

		_dealerScore = 0;
		foreach (Card card in _dealerHand)
		{
			_dealerScore += card.realValue;
		}

		while (_dealerScore < 17)
		{
			Card newCard = _deck.DrawCard(_trapChance);
			_dealerHand.Add(newCard);
			_dealerScore += newCard.realValue;
		}

		string roundMessage = "";
		if (_playerScore > 21)
		{
			_dealerWins++;
			roundMessage = "VOCÊ ESTOUROU! Derrota!";
		}
		else if (_dealerScore > 21 || _playerScore > _dealerScore)
		{
			_playerWins++;
			roundMessage = "VOCÊ VENCEU A RODADA!";
		}
		else if (_dealerScore > _playerScore)
		{
			_dealerWins++;
			roundMessage = "O DEALER VENCEU!";
		}
		else
		{
			roundMessage = "EMPATE!";
		}

		PlayerScoreLabel.Text = $"Player Score REAL: {_playerScore}";
		DealerScoreLabel.Text = $"Dealer Score REAL: {_dealerScore}";
		ResultLabel.Text = $"{roundMessage} (Wins: {_playerWins})";

		RenderHand(_playerHand, PlayerHandContainer, false); 
		RenderHand(_dealerHand, DealerHandContainer, false);
		
		if (NextRoundButton != null)
		{
			NextRoundButton.Visible = true;
			NextRoundButton.Disabled = false;
		}
	}

	public void OnNextButtonPressed()
	{

		if(NextRoundButton != null)
		{
			NextRoundButton.Visible = false;

		}

		if(_playerWins == 2 || _dealerWins == 2)
		{
			StartMatch();

		}

		HitButton.Disabled = false;
		StandButton.Disabled = false;
		if (TrashButton != null) TrashButton.Disabled = false;

		StartRound();
	}

	public void UpdateUI()
	{
		PlayerScoreLabel.Text = $"Player Score: {_playerScore}";
		DealerScoreLabel.Text = $"Dealer Score: {_dealerScore}";
	
		DebtLabel.Text = $"Total Debt: {_totalDebt}";
		BankRollLabel.Text = $"Bankroll: {_bankRoll}";
		
		ResultLabel.Text = $"Wins: {_playerWins}";
		RenderHand(_playerHand, PlayerHandContainer, true);
		RenderHand(_dealerHand, DealerHandContainer, true);
		
	}


	private void RenderHand(List<Card> hand, HBoxContainer container, bool hideSecretValues)
	{

		foreach (Node child in container.GetChildren())
		{
			child.QueueFree();
		}

		foreach (Card card in hand)
		{
			TextureRect cardSprite = new TextureRect();
			cardSprite.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
			cardSprite.CustomMinimumSize = new Vector2(160,200);
			cardSprite.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;

			container.AddChild(cardSprite);
		
			string spritePath = $"res://Assets/Cartas/{card.cardSuit}/{card.visibleValue}.png";

		if (ResourceLoader.Exists(spritePath))
		{
			cardSprite.Texture = GD.Load<Texture2D>(spritePath);
		}
		else
		{
			GD.PrintErr($"[RenderHand] Sprite não encontrado em: {spritePath}");
		
		}		

		if (!hideSecretValues && card.cardType == CardType.Illusory)
		{
			Label trapTag = new Label();
			trapTag.Text = $"REAL: {card.realValue}";
			trapTag.Set("theme_override_colors/font_color", Colors.Red);
			cardSprite.AddChild(trapTag);	
		}

		container.AddChild(cardSprite);
		
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
}
