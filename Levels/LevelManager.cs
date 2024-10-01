using System.Collections.Generic;
using Godot;

public class LevelState
{
    public int NumEnemies {get; set;}
    public int NumRooms {get; set;}
}
public partial class LevelManager : Node
{
    private List<LevelState> LevelStates;

    private int CurrentLevel;

    public override void _Ready()
    {
        base._Ready();
        InitLevels();

        SignalManager.Instance.OnStairsEntered += OnNextLevel;
    }

    public void InitLevels()
    {
        CurrentLevel = 0;
        LevelStates = new List<LevelState>();
    }

    public void OnNextLevel()
    {
        CurrentLevel++;
        
    }
}