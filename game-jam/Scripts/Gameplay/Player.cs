using Godot;

namespace GameJAM.Scripts.Gameplay
{
	public partial class Player : Node
	{
		public int bankRoll { get; set; } = 300;
		public int actualDebt { get; set; } = 3000;
		public int actualBet { get; set; } = 50;

		public void AddBankRoll(int amount)
		{
			bankRoll += amount;
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
