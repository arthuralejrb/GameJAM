using Godot;
using System;

namespace GameJAM.Scripts.Gameplay
{
	public partial class Player : Node
	{
		public int bankRoll {get; set;} = 300;
		public int actualDebt {get; set;} = 3000;
		public int actualBet {get; set;} = 50;

		// lista de itens
		
		public void AddBankRoll(int amount)
		{
			bankRoll += amount;
		}

	}

}
