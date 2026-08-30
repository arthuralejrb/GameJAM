using Godot;

namespace GameJAM.Scripts.Gameplay
{
	public class Item
	{
		public string Name { get; private set; }
		public float Price { get; private set; }
		public ItemType Type { get; private set; }
		public bool IsPassive { get; private set; }
		public string IconPath { get; private set; }

		public Item(string name, float price, ItemType type, string iconPath, bool isPassive = false)
		{
			Name = name;
			Price = price;
			Type = type;
			IconPath = iconPath;
			IsPassive = isPassive;
		}

		public bool Use(MatchManager match, Player player, UIManager ui)
		{
			switch (Type)
			{
				case ItemType.JokerCard:
					match.hasUsedJokerThisRound = true;
					return true;

				case ItemType.Pinga:
					match.isPingaActive = true;
					return true;

				case ItemType.Navalha:
					match.DiscardFullHand();
					return true;

				default:
					return false;
			}
		}
	}
}
