using System.Collections.Generic;

namespace GameJAM.Scripts.Gameplay
{
	public class MatchManager
	{
		private Deck _deck = new Deck();
		public List<Card> playerHand { get; private set; } = new List<Card>();
		public List<Card> dealerHand { get; private set; } = new List<Card>();
		public Stack<Card> discardPile { get; private set; } = new Stack<Card>();

		public int discardsUsedThisRound { get; set; } = 0;
		public bool hasUsedJokerThisRound { get; set; } = false;
		public bool isPingaActive { get; set; } = false;

		public int playerWins { get; set; } = 0;
		public int dealerWins { get; set; } = 0;
		public int totalRounds { get; set; } = 0;
		public int maxHandSize { get; set; } = 5;

		public void DiscardCard(Card cardToDiscard)
		{
			if (playerHand.Contains(cardToDiscard))
			{
				playerHand.Remove(cardToDiscard);
				discardPile.Push(cardToDiscard);
			}
		}

		public void DiscardFullHand()
		{
			foreach (var card in new List<Card>(playerHand))
			{
				DiscardCard(card);
			}
		}

		public void StartMatch(double trapChance)
		{
			playerWins = 0;
			dealerWins = 0;
			_deck.CreateDeck(trapChance);
			StartRound(trapChance);
		}

		public void StartRound(double trapChance)
		{
			discardsUsedThisRound = 0;
			hasUsedJokerThisRound = false;
			isPingaActive = false;

			playerHand.Clear();
			dealerHand.Clear();
			discardPile.Clear();

			playerHand.Add(_deck.DrawCard(trapChance));
			playerHand.Add(_deck.DrawCard(trapChance));
			dealerHand.Add(_deck.DealerDraw(trapChance));
		}

		public void Hit(double trapChance)
		{
			if (playerHand.Count >= maxHandSize) return;
			playerHand.Add(_deck.DrawCard(trapChance));
		}

		public void DealerTurn(double trapChance)
		{
			int playerScore = CalculateScore(playerHand, true, hasUsedJokerThisRound);
			int dealerScore = CalculateScore(dealerHand, true, false);

			while (dealerScore < 17 && dealerScore < playerScore && playerScore <= 21)
			{
				dealerHand.Add(_deck.DealerDraw(trapChance));
				dealerScore = CalculateScore(dealerHand, true, false);
			}
		}

		public int CalculateScore(List<Card> hand, bool useRealValue, bool usouJoker = false)
		{
			if (usouJoker) return 21;

			int score = 0;
			List<Card> aces = new List<Card>();

			foreach (Card card in hand)
			{
				int val = (useRealValue || isPingaActive) ? card.realValue : card.visibleValue;

				if (val == 1) aces.Add(card);
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
