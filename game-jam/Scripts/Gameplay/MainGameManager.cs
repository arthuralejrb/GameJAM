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
			_playerHand.Clear();
			_dealerHand.Clear();

			_trashedCard = null;
		
			_playerHand.Add(_deck.DrawCard(_trapChance));
			_playerHand.Add(_deck.DrawCard(_trapChance));

			_dealerHand.Add(_deck.DrawCard(_trapChance));

			_playerScore = 0;
			_dealerScore = 0;
	
		}


		public void OnHitButtonPressed()
		{
			_playerHand.Add(_deck.DrawCard(_trapChance));

		}


		public void OnStandButtonPressed()
		{

			foreach(Card card in _playerHand)
			{
				_playerScore += card.realValue;

			}

			do{

				foreach(Card card in _dealerHand)
				{
					_dealerScore += card.realValue;

				}

				_dealerHand.Add(_deck.DrawCard(_trapChance));

			
			}while(_dealerScore < 17 || _dealerScore < _playerScore);


			if(_dealerScore > _playerScore && _dealerScore <= 21 || _playerScore > 21)
			{
				_dealerWins += 1;

			}else
			{
				_playerWins += 1;

			}

			if(_playerWins == 2)
			{
				// player won the match
				_bankRoll = _actualBet * 2;

			}else if(_dealerWins == 2)
			{
				// dealert won the match
				_bankRoll -= _actualBet;

			}else
			{
				StartRound();

			}

		}

		public void UpdateUI()
		{
			PlayerScoreLabel.Text = $"Player Score: {_playerScore}";
			DealerScoreLabel.Text = $"Dealer Score: {_dealerScore}";
		
			DebtLabel.Text = $"Total Debt: {_totalDebt}";
			BankRollLabel.Text = $"Bankroll: {_bankRoll}";
			
			ResultLabel.Text = $"Wins: {_playerWins}";
			
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	}
}
