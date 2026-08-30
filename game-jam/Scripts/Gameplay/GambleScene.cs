using Godot;
using System;
using System.ComponentModel;

namespace GameJAM.Scripts.Gameplay
{	
public partial class GambleScene : Node2D
{
	[Export] public Label TotalDebtLabel;
	[Export] public Label TotalBankRollLabel;
	[Export] public Label ActualBetLabel;	
	
	private Player _player;
	private int _currentBet = 50;
	private const int minBet = 50;

	public override void _Ready()
	{ 
		var audio = GetNode<AudioManager>("/root/AudioManager");
		if (audio != null) audio.PlayMusic(audio.GambleBarMusic);
		// pega o nó global do tipo player
		_player = GetNode<Player>("/root/Player");

		if(_player != null)
		{
			_currentBet = Math.Min(minBet, _player.bankRoll);

		}
		UpdateUI();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
	public void OnPlus50Pressed() => AdjustBet(50);
	public void OnPlus100Pressed() => AdjustBet(100);
	public void OnMinus50Pressed() => AdjustBet(-50);
	public void OnMinus100Pressed() => AdjustBet(-100);

	
	public void OnMinimalPressed()
	{
		_currentBet = Math.Min(minBet, _player.bankRoll);
		UpdateUI();

	}


	public void OnAllInPressed()
	{
		if(_player == null) return;

		_currentBet = _player.bankRoll;
		UpdateUI();		

	}


	public void AdjustBet(int amount)
	{
		if(_player == null) return;

		// Mathf.Clamp é uma função que limita um valor a um mínimo e um máximo
		_currentBet = Mathf.Clamp(_currentBet + amount, minBet, _player.bankRoll);
		UpdateUI();

	}


	public void UpdateUI()
	{
		if(_player == null) return;

		// altera o texto das labels na cena
		TotalDebtLabel.Text = $"Your debt: {_player.actualDebt}";
		TotalBankRollLabel.Text = $"Your bankroll: {_player.bankRoll}";
		ActualBetLabel.Text = $"BET: {_currentBet}";

	}


	public void OnConfirmButtonPressed()
	{
		if(_player == null) return;

		_player.actualBet = _currentBet;

		// vai para a cena da partida, não precisa verificar se o valor da aposta é valido
		// por causa do Mathf.clamp
		GetTree().ChangeSceneToFile("res://Scenes/GameScene.tscn");

	}


	public void OnMenuButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/MainMenuScene.tscn");		
	
	}


}
}
