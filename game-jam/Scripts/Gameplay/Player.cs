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


		// public void GameEnd()
		// {
		// 	if (bankRoll >= actualDebt)
		// 	{
		// 		GetTree().ChangeSceneToFile("res://Scenes/WinScene.tscn");
				
		// 	}else if(bankRoll <= 0)
		// 	{
		// 		GetTree().ChangeSceneToFile("res://Scenes/LoseScene.tscn");

		// 	}else
		// 	{
		// 		return;
		// 	}

		// }

	}

}
