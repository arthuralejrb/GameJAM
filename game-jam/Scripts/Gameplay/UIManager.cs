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
	[Export] public Button	OptionsButton;
	[Export] public Button QuitButton;
	 
	[Export] public ColorRect OverlayDimmer;
	[Export] public Button NextButton;

	private const string _overlayPath = "res://Scenes/OptionsMenuScene.tscn"; 


	public void UpdateScores(int playerScore, int dealerScore, bool hideSecretValues)
	{
		if (PlayerScoreLabel != null)
		{
			// Exibe apenas o número limpo (ex: "18" ou "PLAYER: 18")
			PlayerScoreLabel.Text = $"{playerScore}";
		
		}

		if (DealerScoreLabel != null)
		{
			DealerScoreLabel.Text = $"{dealerScore}";
		
		}

	}

	public void UpdateEconomy(int bankroll, int debt, int bet, int wins, string message = "")
	{
		BankRollLabel.Text = $"Bankroll: {bankroll}";
		DebtLabel.Text = $"Total Debt: {debt}";
		BetLabel.Text = $"Actual Bet: {bet}";
		
		if (string.IsNullOrEmpty(message))
			ResultLabel.Text = $"Wins: {wins}";
		else
			ResultLabel.Text = $"{message} (Wins: {wins})";
	}


	public void ToggleActionButtons(bool enabled)
	{
		HitButton.Disabled = !enabled;
		StandButton.Disabled = !enabled;
		if (TrashButton != null) TrashButton.Disabled = !enabled;
	}



	public void ShowNextButton(bool show)
	{
		if (NextButton != null)
		{
			NextButton.Visible = show;
		}

		if (OverlayDimmer != null)
		{
			// Ativa/desativa a tela escura quando o botão NEXT aparece/some
			OverlayDimmer.Visible = show;
		}
	}


	public void ShowOverlay()
	{
		PackedScene overlayScene = GD.Load<PackedScene>(_overlayPath);

		if(overlayScene != null)
		{
			var overlayInstance = overlayScene.Instantiate<Node>();
			overlayInstance.ProcessMode = ProcessModeEnum.Always;
			AddChild(overlayInstance);		
		
		}
	}


	// Arraste a cena CardView.tscn para este campo no Inspector do MainUI!
	[Export] public PackedScene CardScene;

	public void RenderHand(List<Card> hand, HBoxContainer container, bool hideSecretValues)
	{
		if (container == null || CardScene == null) return;

		// Limpa a mão antiga
		foreach (Node child in container.GetChildren())
		{
			child.QueueFree();
		}

		// Calcula a separação dinâmica: quanto mais cartas, mais elas se sobrepõem
		int baseSeparation = -40; // Espaçamento padrão para 2 ou 3 cartas
		if (hand.Count > 3)
		{
			// Reduz ainda mais a separação para compensar o número de cartas (ex: -60, -80...)
			baseSeparation = -40 - ((hand.Count - 3) * 15);
		}

		// Aplica o novo espaçamento no container
		container.AddThemeConstantOverride("separation", baseSeparation);

		// Instancia as cartas
		foreach (Card card in hand)
		{
			CardView cardInstance = CardScene.Instantiate<CardView>();
			container.AddChild(cardInstance);
			cardInstance.SetupCard(card, hideSecretValues);
		}
	}
}
}
