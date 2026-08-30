using Godot;

namespace GameJAM.Scripts.Gameplay
{
	public partial class ShopManager : Node2D
	{
		[Export] public Label BankRollLabel;
		[Export] public Button BuyPingaBtn;
		[Export] public Button BuyJokerBtn;
		[Export] public Button BuyBolsoBtn;
		[Export] public Button BuyNavalhaBtn;
		[Export] public Button GoToGameBtn;

		private Player _player;

		private Item _pinga = new Item("Pinga", 150, ItemType.Pinga, "res://Assets/itens/cachaca.png");
		private Item _joker = new Item("Curinga", 1000, ItemType.JokerCard, "res://Assets/itens/joker.png");
		private Item _bolso = new Item("Bolso Secreto", 300, ItemType.Bolso, "res://Assets/itens/bolso.png", true);
		private Item _navalha = new Item("Navalha", 100, ItemType.Navalha, "res://Assets/itens/navalha.png");

		public override void _Ready()
		{
			_player = GetNode<Player>("/root/Player");

			if (BuyPingaBtn != null) BuyPingaBtn.Pressed += () => TryBuy(_pinga, BuyPingaBtn);
			if (BuyJokerBtn != null) BuyJokerBtn.Pressed += () => TryBuy(_joker, BuyJokerBtn);
			if (BuyBolsoBtn != null) BuyBolsoBtn.Pressed += () => TryBuy(_bolso, BuyBolsoBtn);
			if (BuyNavalhaBtn != null) BuyNavalhaBtn.Pressed += () => TryBuy(_navalha, BuyNavalhaBtn);

			if (GoToGameBtn != null) GoToGameBtn.Pressed += () => GetTree().ChangeSceneToFile("res://Scenes/GambleScene.tscn");

			UpdateUI();
		}

		private void TryBuy(Item item, Button btn)
		{
			if (_player != null && _player.BuyItem(item))
			{
				btn.Disabled = true; // Desativa o botão garantindo apenas 1 compra por item
				UpdateUI();
			}
		}

		private void UpdateUI()
		{
			if (_player != null && BankRollLabel != null)
				BankRollLabel.Text = $"Bankroll: ${_player.bankRoll}";
		}
	}
}
