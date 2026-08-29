using Godot;

namespace GameJAM.Scripts.Gameplay
{
public partial class MainGameManager : Node2D
{
	[Export] public UIManager UI;
	
	private MatchManager _match = new MatchManager();
	private double _trapChance {get; set;} = 0.2;
	private int _actualBet {get; set;} = 50;
	private int _bankRoll;
	private int _totalDebt; 

	public override void _Ready()
	{
		var player = GetNode<Player>("/root/Player");
		if(player == null) return;
		
		_actualBet = player.actualBet;
		_bankRoll = player.bankRoll;
		_totalDebt = player.actualDebt;
		
		_trapChance = CalculateDifficulty();

		StartMatch();
	}


	public void StartMatch()
	{
		_match.StartMatch(_trapChance);
		UpdateUI();
	}


	public void StartRound()
	{
		_match.StartRound(_trapChance);
		UI.ToggleActionButtons(true);
		UI.ShowNextButton(false);
		UpdateUI();
	}

	public void OnHitButtonPressed()
	{	
		_match.Hit(_trapChance);
		UpdateUI();
	}


	public void OnStandButtonPressed()
	{
		UI.ToggleActionButtons(false);
		_match.DealerTurn(_trapChance);

		int playerScore = _match.CalculateScore(_match.playerHand, true);
		int dealerScore = _match.CalculateScore(_match.dealerHand, true);

		string roundMessage = "";
		if (playerScore > 21)
		{
			_match.dealerWins++;
			roundMessage = "VOCÊ ESTOUROU! Derrota!";
		}
		else if (dealerScore > 21 || playerScore > dealerScore)
		{
			_match.playerWins++;
			roundMessage = "VOCÊ VENCEU A RODADA!";
		}
		else
		{
			_match.dealerWins++;
			roundMessage = "O DEALER VENCEU!";
		}

		UI.UpdateScores(playerScore, dealerScore, true);
		UI.UpdateEconomy(_bankRoll, _totalDebt, _actualBet, _match.playerWins, roundMessage);
		UI.RenderHand(_match.playerHand, UI.PlayerHandContainer, false);
		UI.RenderHand(_match.dealerHand, UI.DealerHandContainer, false);

		_match.totalRounds++;
	
		if(_match.playerWins != 2 && _match.dealerWins != 2)
		{
			UI.ShowNextButton(true);
				
		}else
		{
			var player = GetNode<Player>("/root/Player");
			if(player == null) return;

			// Faz a matemática de adicionar ou subtrair o saldo
			if(_match.playerWins > _match.dealerWins)
			{   
				player.AddBankRoll(_actualBet * 2);
			}
			else
			{   
				player.AddBankRoll(_actualBet * - 1);
			}

			// Passa pela barreira de verificação. 
			// Falso, o jogo continua.
			if (!CheckGameEndConditions(player))
			{
				GetTree().ChangeSceneToFile("res://Scenes/GambleScene.tscn");
			}
		}
	}

	public void OnTrashButtonPressed()
	{
		// Se a trava estiver ativada, encerra o método
		if (_match.hasDiscardedThisRound) return; 

		Card cardToDiscard = UI.GetSelectedCard();

		if (cardToDiscard != null)
		{
			_match.DiscardCard(cardToDiscard);
			
			// Ativa a trava para impedir novos descartes neste round
			_match.hasDiscardedThisRound = true; 
			
			UI.ClearSelection();
			UpdateUI(); 
		}
	}


	public void OnNextButtonPressed()
	{
		if (_match.playerWins == 2 || _match.dealerWins == 2)
		{
			StartMatch();
			return;
		}

		StartRound();
	}


	private void UpdateUI()
	{
		int pScore = _match.CalculateScore(_match.playerHand, false);
		int dScore = _match.CalculateScore(_match.dealerHand, false);

		UI.UpdateScores(pScore, dScore, false);
		UI.UpdateEconomy(_bankRoll, _totalDebt, _actualBet, _match.playerWins);
		UI.RenderHand(_match.playerHand, UI.PlayerHandContainer, true);
		UI.RenderHand(_match.dealerHand, UI.DealerHandContainer, true);
		
		// O botão liga e desliga automaticamente baseado no tamanho exato da mão neste frame
		UI.HitButton.Disabled = _match.playerHand.Count >= _match.maxHandSize;
	}


	public double CalculateDifficulty()
	{
		// calcula o limiar de dificuldade com base em quão proximo o jogador está de quitar a divida
		double paymentLeft = _bankRoll / _totalDebt;
		double difficultyThreshhold = 0.0;

		if(paymentLeft >= 0.75) difficultyThreshhold = 0.3;
		if(paymentLeft >= 0.25 && paymentLeft < 0.75) difficultyThreshhold = 0.2;
		if(paymentLeft < 0.25) difficultyThreshhold = 0.12;

		// calcula o multiplicador de dificuldade com base em quanto o jogador apostou
		double betPercent = _actualBet / _bankRoll;
		double difficultyMultiplier = 1.0;

		if(betPercent == 1.0) difficultyMultiplier = 2;
		if(betPercent >= 0.75 && betPercent < 1.0) difficultyMultiplier = 1.75;
		if(betPercent >= 0.5 && betPercent < 0.75) difficultyMultiplier = 1.5;
		if(betPercent >= 0.25 && betPercent < 0.5) difficultyMultiplier = 1.25;
		if(betPercent < 0.25) difficultyMultiplier = 1;

		return difficultyThreshhold * difficultyMultiplier;

	}

	private bool CheckGameEndConditions(Player player)
	{
		// Atualiza a variável local com o saldo exato após a transação
		_bankRoll = player.bankRoll;

		if (_bankRoll >= _totalDebt)
		{
			GetTree().ChangeSceneToFile("res://Scenes/WinScene.tscn");
			return true; 
		}
		else if (_bankRoll <= 0)
		{
			GetTree().ChangeSceneToFile("res://Scenes/LoseScene.tscn");
			return true; 
		}

		return false;
	}


}
}
