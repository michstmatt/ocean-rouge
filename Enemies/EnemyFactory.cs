using System;
using Godot;

public enum EnemyType
{
	Seahorse,
	Whale,
	Fishy,
	Crab,
	Mermaid,
	Jellyfish,
	Starfish
}


public class EnemyFactory
{
	public static EnemyType[] EnemyTypes = Enum.GetValues<EnemyType>();

	public static string GetEnemyScene(EnemyType enemyType)
	{
		switch(enemyType)
		{
			case EnemyType.Seahorse:
				return "res://Enemies/Tracking/Types/Seahorse.tscn";
			case EnemyType.Whale:
				return "res://Enemies/Tracking/Types/Whale.tscn";
			case EnemyType.Fishy:
				return "res://Enemies/Tracking/Types/Fishy.tscn";
			case EnemyType.Crab:
				return "res://Enemies/Tracking/Types/Crab.tscn";
			case EnemyType.Mermaid:
				return "res://Enemies/Tracking/Types/Mermaid.tscn";
			case EnemyType.Jellyfish:
				return "res://Enemies/Tracking/Types/Jellyfish.tscn";
			case EnemyType.Starfish:
				return "res://Enemies/Tracking/Types/Starfish.tscn";

			default:
				throw new ArgumentOutOfRangeException(nameof(enemyType), enemyType, "Unsupported enemy type");
		}
	}

	public static EnemyType GetRandomEnemyType()
	{
		var randIndex = (int)(GD.Randi() % EnemyFactory.EnemyTypes.Length);
		EnemyType type = EnemyFactory.EnemyTypes[randIndex];
		return type;
	}
}
