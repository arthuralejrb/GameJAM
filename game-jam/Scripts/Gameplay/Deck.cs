using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace GameJAM.Scripts.Gameplay
{

	// Class that represents any card 
	public class Card
	{ 
		public int visibleValue;
		public int realValue; // ace == 1 // J == 11 // Q == 12 // K == 13;
		public Suits cardSuit;
		public CardType cardType;

		public Card(int visibleValue, int realValue, Suits cardSuit, CardType cardType)
		{
			this.visibleValue = visibleValue;
			this.realValue = realValue;
			this.cardSuit = cardSuit;
			this.cardType = cardType;

		}

	}

	public class Deck
	{
		private int _deckSize = 52;
		private List<Card> _deck = new List<Card>();
		private Random _rng = new Random();

		// create a new deck
		public void CreateDeck(double trapChance)
		{
			_deck.Clear();

			foreach (Suits suit in Enum.GetValues<Suits>())
			{	
				
				for(int i = 1; i <= 13;i++)
				{
					int realValue = i;
					int visibleValue = realValue;
					Suits cardSuit = suit;
					CardType cardType = CardType.Normal;

					// checks if its a cheated card
					if(_rng.NextDouble() < trapChance)
					{ 
						CardType[] allTypes = Enum.GetValues<CardType>();
						CardType trapType = allTypes[_rng.Next(1, allTypes.Length)];
						cardType = trapType;

						if(trapType ==  CardType.Illusory)
						{
							while(realValue == visibleValue)
							{
								realValue = _rng.Next(1,11);
								
							}

						}

					}

					_deck.Add(new Card(visibleValue, realValue, cardSuit, cardType));

				}

			}

			Shuffle(); 
		}

		// function to shuffle the deck
		public void Shuffle()
		{
			int n = _deckSize;

			while( n > 1)
			{
				n--;
				int k = _rng.Next(n + 1);
				Card aux = _deck[k];
				_deck[k] = _deck[n];
				_deck[n] = aux;
			}

		}

		// function to draw a card from the deck
		public Card DrawCard(double trapChance)	
		{
			int n = _deck.Count - 1 ;

			// empty deck
			if(n == 0)
			{
				CreateDeck(trapChance);
				
			}

			Card draw = _deck[n];
			_deck.RemoveAt(n);

			return draw;
		}
	}
}