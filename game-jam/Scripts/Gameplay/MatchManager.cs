using System.Collections.Generic;

namespace GameJAM.Scripts.Gameplay
{
	public class MatchManager
	{
		private Deck _deck = new Deck();
		public List<Card> playerHand { get; private set; } = new List<Card>();
		public List<Card> dealerHand { get; private set; } = new List<Card>();
		
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
			int playerScore = CalculateScore(playerHand, true.false);
			int dealerScore = CalculateScore(dealerHand, true,false);

			while (dealerScore < 17 && dealerScore < playerScore && playerScore <= 21)
			{
				dealerHand.Add(_deck.DealerDraw(trapChance));
				dealerScore = CalculateScore(dealerHand, true,false);
			}

		}

		// Essa variavel UsouJoker n vai ser assim pra sempre, vai ser mais teste (por enquanto)
		public int CalculateScore(List<Card> hand, bool useRealValue, bool UsouJoker)
		{
			if(UsouJoker) return 21;

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
