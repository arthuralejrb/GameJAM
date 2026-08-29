using Godot;
using System;

namespace GameJAM.Scripts.Gameplay
{
	public partial class Player : Node
	{
		public int bankRoll {get; set;} = 300;
		public int actualDebt {get; set;} = 3000;
		public int actualBet {get; set;} = 50;

		// adicionar: lista de itens
		
		public void AddBankRoll(int amount)
		{
			bankRoll += amount;
			// GameEnd();

		}

		public void CheckGameEnd()
		{
			if (bankRoll >= actualDebt)
			{
				// Dívida paga, Transição para tela de vitória
				GetTree().ChangeSceneToFile("res://Scenes/WinScene.tscn");
			}
			else if (bankRoll <= 0)
			{
				// Saldo zerado/negativo, Transição para tela de derrota
				GetTree().ChangeSceneToFile("res://Scenes/LoseScene.tscn");
			}
		}

	}

}
