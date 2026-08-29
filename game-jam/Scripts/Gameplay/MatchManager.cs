using System.Collections.Generic;

namespace GameJAM.Scripts.Gameplay
{
	public class MatchManager
	{
		private Deck _deck = new Deck();
		public List<Card> playerHand { get; private set; } = new List<Card>();
		public List<Card> dealerHand { get; private set; } = new List<Card>();

		public Stack<Card> discardPile { get; private set; } = new Stack<Card>();

		public void DiscardCard(Card cardToDiscard)
		{
			if (playerHand.Contains(cardToDiscard))
			{
				playerHand.Remove(cardToDiscard);
				discardPile.Push(cardToDiscard);
			}
		}
		
		public int playerWins { get; set; } = 0;
		public int dealerWins { get; set; } = 0;
		public int totalRounds {get; set;} = 0;


		public void StartMatch(double trapChance)
		{
			playerWins = 0;
			dealerWins = 0;
			_deck.CreateDeck(trapChance);
			StartRound(trapChance);
		}


		public void StartRound(double trapChance)
		{

			playerHand.Clear();
			dealerHand.Clear();
			discardPile.Clear();

			playerHand.Add(_deck.DrawCard(trapChance));
			playerHand.Add(_deck.DrawCard(trapChance));
			dealerHand.Add(_deck.DealerDraw(trapChance));
		
		}


		public void Hit(double trapChance)
		{
			playerHand.Add(_deck.DrawCard(trapChance));
			
		}


		public void DealerTurn(double trapChance)
		{
			int playerScore = CalculateScore(playerHand, true);
			int dealerScore = CalculateScore(dealerHand, true);

			while (dealerScore < 17 && dealerScore < playerScore && playerScore <= 21)
			{
				dealerHand.Add(_deck.DealerDraw(trapChance));
				dealerScore = CalculateScore(dealerHand, true);
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
