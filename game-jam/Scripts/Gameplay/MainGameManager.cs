using Godot;

namespace GameJAM.Scripts.Gameplay
{
	public partial class MainGameManager : Node2D
	{
		[Export] public UIManager UI;
		
		private MatchManager _match = new MatchManager();
		private double _trapChance = 0.21;

		private int _bankRoll = 300;
		private int _totalDebt = 3000;
		private int _actualBet = 300;

		public override void _Ready()
		{
			StartMatch();
		}

		public void StartMatch()
		{
			_match.StartMatch(_trapChance);
			UpdateUI();
		}

		public void StartRound()
		{
			_match.StartRound(_trapChance);
			UI.ToggleActionButtons(true);
			UI.ShowNextButton(false);
			UpdateUI();
		}

		public void OnHitButtonPressed()
		{
			_match.Hit(_trapChance);
			UpdateUI();
		}

		public void OnStandButtonPressed()
		{
			UI.ToggleActionButtons(false);
			_match.DealerTurn(_trapChance);

			int playerScore = _match.CalculateScore(_match.PlayerHand, true);
			int dealerScore = _match.CalculateScore(_match.DealerHand, true);

			string roundMessage = "";
			if (playerScore > 21)
			{
				_match.DealerWins++;
				roundMessage = "VOCÊ ESTOUROU! Derrota!";
			}
			else if (dealerScore > 21 || playerScore > dealerScore)
			{
				_match.PlayerWins++;
				roundMessage = "VOCÊ VENCEU A RODADA!";
			}
			else
			{
				_match.DealerWins++;
				roundMessage = "O DEALER VENCEU!";
			}

			UI.UpdateScores(playerScore, dealerScore, true);
			UI.UpdateEconomy(_bankRoll, _totalDebt, _actualBet, _match.PlayerWins, roundMessage);
			UI.RenderHand(_match.PlayerHand, UI.PlayerHandContainer, false);
			UI.RenderHand(_match.DealerHand, UI.DealerHandContainer, false);

			UI.ShowNextButton(true);
		}

		public void OnNextButtonPressed()
		{
			if (_match.PlayerWins == 2 || _match.DealerWins == 2)
			{
				StartMatch();
				return;
			}

			StartRound();
		}

		private void UpdateUI()
		{
			int pScore = _match.CalculateScore(_match.PlayerHand, false);
			int dScore = _match.CalculateScore(_match.DealerHand, false);

			UI.UpdateScores(pScore, dScore, false);
			UI.UpdateEconomy(_bankRoll, _totalDebt, _actualBet, _match.PlayerWins);
			UI.RenderHand(_match.PlayerHand, UI.PlayerHandContainer, true);
			UI.RenderHand(_match.DealerHand, UI.DealerHandContainer, true);
		}
	}
}
