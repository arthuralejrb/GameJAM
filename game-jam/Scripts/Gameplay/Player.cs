using System.Collections.Generic;
using Godot;

namespace GameJAM.Scripts.Gameplay
{
	public partial class Player : Node
	{
		public int bankRoll { get; set; } = 300;
		public int actualDebt { get; set; } = 3000;
		public int actualBet { get; set; } = 50;
		
		public List<Item> inventario { get; private set; } = new List<Item>();
		public int extraDiscardSlots { get; set; } = 0; // Ganho permanentemente com o Bolso

		public void AddBankRoll(int amount) => bankRoll += amount;

		public bool BuyItem(Item item)
		{
			if (bankRoll >= item.Price)
			{
				// Impede comprar o mesmo item consumível mais de uma vez na mesma rodada/partida
				if (inventario.Exists(i => i.Type == item.Type)) return false;

				bankRoll -= (int)item.Price;

				// Efeito imediato do item permanente Bolso
				if (item.Type == ItemType.Bolso)
				{
					extraDiscardSlots += 1;
				}
				else
				{
					inventario.Add(item);
				}
				return true;
			}
			return false;
		}

		public bool UseItem(ItemType itemType, MatchManager match, UIManager ui)
		{
			Item itemToUse = inventario.Find(i => i.Type == itemType);
			if (itemToUse != null && itemToUse.Use(match, this, ui))
			{
				inventario.Remove(itemToUse); // Consome o item após o uso
				return true;
			}
			return false;
		}

		public bool CheckGameEnd()
		{
			if (bankRoll >= actualDebt)
			{
				GetTree().ChangeSceneToFile("res://Scenes/WinScene.tscn");
				return true;
			}
			else if (bankRoll <= 0)
			{
				GetTree().ChangeSceneToFile("res://Scenes/LoseScene.tscn");
				return true;
			}
			return false;
		}
	}
}
