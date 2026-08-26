using System.Collections.Generic;

namespace GameJAM.Scripts.Gameplay
{
	public class MatchManager
	{
		private Deck _deck = new Deck();
		public List<Card> PlayerHand { get; private set; } = new List<Card>();
		public List<Card> DealerHand { get; private set; } = new List<Card>();
		
		public int PlayerWins { get; set; } = 0;
		public int DealerWins { get; set; } = 0;

		public void StartMatch(double trapChance)
		{
			PlayerWins = 0;
			DealerWins = 0;
			_deck.CreateDeck(trapChance);
			StartRound(trapChance);
		}

		public void StartRound(double trapChance)
		{
			PlayerHand.Clear();
			DealerHand.Clear();

			PlayerHand.Add(_deck.DrawCard(trapChance));
			PlayerHand.Add(_deck.DrawCard(trapChance));
			DealerHand.Add(_deck.DrawCard(trapChance));
		}

		public void Hit(double trapChance)
		{
			PlayerHand.Add(_deck.DrawCard(trapChance));
		}

		public void DealerTurn(double trapChance)
		{
			int playerScore = CalculateScore(PlayerHand, true);
			int dealerScore = CalculateScore(DealerHand, true);

			while (dealerScore < 17 && dealerScore < playerScore && playerScore <= 21)
			{
				DealerHand.Add(_deck.DrawCard(trapChance));
				dealerScore = CalculateScore(DealerHand, true);
			}
		}

		public int CalculateScore(List<Card> hand, bool useRealValue)
		{
			int score = 0;
			List<Card> aces = new List<Card>();

			foreach (Card card in hand)
			{
				int val = useRealValue ? card.realValue : card.visibleValue;

				if (val == 1)
				{
					aces.Add(card);
				}
				else
				{
					if (val >= 11 && val <= 13) val = 10;
					score += val;
				}
			}

			foreach (Card ace in aces)
			{
				score += (score <= 10) ? 11 : 1;
			}

			return score;
		}
	}
}
