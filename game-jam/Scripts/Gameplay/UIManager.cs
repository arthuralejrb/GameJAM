using Godot;
using System.Collections.Generic;

namespace GameJAM.Scripts.Gameplay
{
	public partial class UIManager : CanvasLayer
	{
		[Export] public Label PlayerScoreLabel;
		[Export] public Label DealerScoreLabel;
		[Export] public Label BankRollLabel;
		[Export] public Label DebtLabel;
		[Export] public Label BetLabel;
		[Export] public Label ResultLabel;
		
		[Export] public Button HitButton;
		[Export] public Button StandButton;
		[Export] public Button TrashButton;
		[Export] public Button NextRoundButton;

		[Export] public HBoxContainer PlayerHandContainer;
		[Export] public HBoxContainer DealerHandContainer;
		
		[Export] public Control PauseMenuControl;
		[Export] public Button ResumeButton;
		[Export] public Button OptionsButton;
		[Export] public Button QuitButton;
		 
		[Export] public ColorRect OverlayDimmer;
		[Export] public Button NextButton;

		[Export] public PackedScene CardScene;

		private const string _overlayPath = "res://Scenes/OptionsMenuScene.tscn"; 
		private Card _selectedCardData;
		private CardView _selectedCardNode;
		private readonly float _yOffset = -20f;

		public void UpdateScores(int playerScore, int dealerScore, bool hideSecretValues)
		{
			if (PlayerScoreLabel != null) PlayerScoreLabel.Text = $"{playerScore}";
			if (DealerScoreLabel != null) DealerScoreLabel.Text = $"{dealerScore}";
		}

		public void UpdateEconomy(int bankroll, int debt, int bet, int wins, string message = "")
		{
			if (BankRollLabel != null) BankRollLabel.Text = $"Bankroll: {bankroll}";
			if (DebtLabel != null) DebtLabel.Text = $"Total Debt: {debt}";
			if (BetLabel != null) BetLabel.Text = $"Actual Bet: {bet}";
			
			if (ResultLabel != null)
			{
				if (string.IsNullOrEmpty(message))
					ResultLabel.Text = $"Wins: {wins}";
				else
					ResultLabel.Text = $"{message} (Wins: {wins})";
			}
		}

		public void ToggleActionButtons(bool enabled)
		{
			if (HitButton != null) HitButton.Disabled = !enabled;
			if (StandButton != null) StandButton.Disabled = !enabled;
			if (TrashButton != null) TrashButton.Disabled = !enabled;
		}

		public void ShowNextButton(bool show)
		{
			if (NextButton != null) NextButton.Visible = show;
			if (OverlayDimmer != null) OverlayDimmer.Visible = show;
		}

		public void ShowOverlay()
		{
			PackedScene overlayScene = GD.Load<PackedScene>(_overlayPath);
			if (overlayScene != null)
			{
				var overlayInstance = overlayScene.Instantiate<Node>();
				overlayInstance.ProcessMode = ProcessModeEnum.Always;
				AddChild(overlayInstance);
			}
		}

		public void RenderHand(List<Card> hand, HBoxContainer container, bool hideSecretValues)
		{
			if (container == null || CardScene == null) return;

			foreach (Node child in container.GetChildren())
			{
				child.QueueFree();
			}

			int baseSeparation = -40;
			if (hand.Count > 3)
			{
				baseSeparation = -40 - ((hand.Count - 3) * 15);
			}
			container.AddThemeConstantOverride("separation", baseSeparation);

			foreach (Card card in hand)
			{
				CardView cardInstance = CardScene.Instantiate<CardView>();
				container.AddChild(cardInstance);
				cardInstance.SetupCard(card, hideSecretValues);

				if (container == PlayerHandContainer && hideSecretValues)
				{
					cardInstance.OnCardClicked = (cardData, node) => OnCardSelected(cardData, node);
				}
			}
		}

		private void OnCardSelected(Card cardData, CardView cardNode)
		{
			if (_selectedCardData == cardData)
			{
				cardNode.Position = new Vector2(cardNode.Position.X, cardNode.Position.Y - _yOffset);
				ClearSelection();
				return;
			}

			if (_selectedCardNode != null && GodotObject.IsInstanceValid(_selectedCardNode))
			{
				_selectedCardNode.Position = new Vector2(_selectedCardNode.Position.X, _selectedCardNode.Position.Y - _yOffset);
			}

			_selectedCardData = cardData;
			_selectedCardNode = cardNode;
			cardNode.Position = new Vector2(cardNode.Position.X, cardNode.Position.Y + _yOffset);
		}

		// --- MÉTODOS SOLICITADOS PELO MAINGAMEMANAGER ---
		public Card GetSelectedCard()
		{
			return _selectedCardData;
		}

		public void ClearSelection()
		{
			_selectedCardData = null;
			_selectedCardNode = null;
		}

		[Export] public HBoxContainer ItemsContainer;

		public void RenderInventory(Player player, System.Action<ItemType> onUseItem)
		{
			if (ItemsContainer == null) return;

			// Limpa os ícones antigos
			foreach (Node child in ItemsContainer.GetChildren())
			{
				child.QueueFree();
			}

			foreach (Item item in player.inventario)
			{
				// Cria um botão com textura para renderizar o sprite bonitinho
				TextureButton itemBtn = new TextureButton();
				itemBtn.IgnoreTextureSize = true;
				itemBtn.CustomMinimumSize = new Vector2(64, 64);
				itemBtn.StretchMode = TextureButton.StretchModeEnum.KeepAspectCentered;

				if (!string.IsNullOrEmpty(item.IconPath) && ResourceLoader.Exists(item.IconPath))
				{
					itemBtn.TextureNormal = GD.Load<Texture2D>(item.IconPath);
				}

				// Tooltip opcional ao passar o mouse por cima do item
				itemBtn.TooltipText = item.Name;

				// Conecta a ação de clique para usar o item
				itemBtn.Pressed += () => onUseItem?.Invoke(item.Type);

				ItemsContainer.AddChild(itemBtn);
			}
		}
	}
}
