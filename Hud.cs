using Godot;
using System;

public partial class Hud : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

	public uint Coins = 0;

	public override void _Ready()
	{
		SignalManager.Instance.ItemPickup += (PickupType t, uint amount) =>
		{
			if (t == PickupType.Coin)
			{
				Coins += amount;
				UpdateCoins((int)Coins);
			}
		};
	}

	public void ShowMessage(string text)
	{
		var message = GetNode<Label>("Message");
		message.Text = text;
		message.Show();

		GetNode<Timer>("MessageTimer").Start();
	}
	
	async public void ShowGameOver()
	{
		ShowMessage("Game Over");

		var messageTimer = GetNode<Timer>("MessageTimer");
		await ToSignal(messageTimer, Timer.SignalName.Timeout);

		var message = GetNode<Label>("Message");
		message.Text = "Dodge the Creeps!";
		message.Show();

		//await this.CreateAsyncTimer(1.0f);
		GetNode<Button>("StartButton").Show();
		Coins = 0;
		UpdateCoins(0);
	}
	
		
	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}
	
	public void UpdateCoins(int coins)
	{
		GetNode<Label>("Coins/CoinLabel").Text = coins.ToString();
	}
	
	// We also specified this function name in PascalCase in the editor's connection window.
	private void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide();
		EmitSignal(SignalName.StartGame);
	}

	// We also specified this function name in PascalCase in the editor's connection window.
	private void OnMessageTimerTimeout()
	{
		GetNode<Label>("Message").Hide();
	}

}
