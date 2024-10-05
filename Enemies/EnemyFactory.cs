using System;
using System.Collections.Generic;
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

public abstract class EnemyMetadata
{
	public string Scene {get; set;}
	public float Speed {get; set;}
	public float Damage {get; set;}
	public float Health {get; set;}
	public int MinSpawnLevel {get; set;}

	public EnemyMetadata(string scene, float speed, float damage, float health, int minSpawnLevel)
	{
		Scene = scene;
		Speed = speed;
		Damage = damage;
		Health = health;
		MinSpawnLevel = minSpawnLevel;
	}

	public EnemyMetadata Instantiate()
	{
		return (EnemyMetadata)this.MemberwiseClone();
	}

	public class Seahorse : EnemyMetadata
	{
		public Seahorse() : base(
			scene: "res://Enemies/Tracking/Types/Seahorse.tscn",
			speed: 2f,
			damage: 10,
			health: 10,
			minSpawnLevel: 5
		){}
	}
	public class Whale : EnemyMetadata
	{
		public Whale(): base(
			scene: "res://Enemies/Tracking/Types/Whale.tscn",
			speed: 0.5f,
			damage: 10,
			health: 100,
			minSpawnLevel: 20
		){}
	}
	public class Fishy : EnemyMetadata
	{
		public Fishy() : base(
			scene: "res://Enemies/Tracking/Types/Fishy.tscn",
			speed: 2,
			damage: 10,
			health: 10,
			minSpawnLevel: 1
		) {}
	}

	public class Crab : EnemyMetadata
	{
		public Crab() : base(
			scene: "res://Enemies/Tracking/Types/Crab.tscn",
			speed: 3.5f,
			damage: 1,
			health: 5,
			minSpawnLevel: 1
		){}
	}
	public class Mermaid : EnemyMetadata
	{
		public Mermaid() : base(
			scene: "res://Enemies/Tracking/Types/Mermaid.tscn",
			speed: 1,
			damage: 10,
			health: 10,
			minSpawnLevel: 10
		){}
	}
	public class Jellyfish : EnemyMetadata
	{
		public Jellyfish() : base(
			scene: "res://Enemies/Tracking/Types/Jellyfish.tscn",
			speed: 4,
			damage: 20,
			health: 1,
			minSpawnLevel: 20
		) {}
	}

	public class Starfish : EnemyMetadata
	{
		public Starfish() : base(
			scene: "res://Enemies/Tracking/Types/Starfish.tscn",
			speed: 3.5f,
			damage: 1,
			health: 5,
			minSpawnLevel: 5
		) {}
	}
}


public class EnemyFactory
{
	public static EnemyType[] EnemyTypes = Enum.GetValues<EnemyType>();
	public static void Reinit()
	{
		AllowedEnemyTypes = new List<EnemyType>();
		EnemiesMetadata= new Dictionary<EnemyType, EnemyMetadata>()
		{
			{EnemyType.Crab, new EnemyMetadata.Crab()},
			{EnemyType.Fishy, new EnemyMetadata.Fishy()},
			{EnemyType.Jellyfish, new EnemyMetadata.Jellyfish()},
			{EnemyType.Mermaid, new EnemyMetadata.Mermaid()},
			{EnemyType.Seahorse, new EnemyMetadata.Seahorse()},
			{EnemyType.Starfish, new EnemyMetadata.Starfish()},
			{EnemyType.Whale, new EnemyMetadata.Whale()}
		};
		OnNextLevel(1);
	}
	public static Dictionary<EnemyType, EnemyMetadata> EnemiesMetadata { get; private set; }
	private static List<EnemyType> AllowedEnemyTypes;
	public static EnemyMetadata GetEnemyMetadata(EnemyType enemyType)
	{
		return EnemiesMetadata[enemyType];
	}

	public static void OnNextLevel(int level)
	{
		foreach(var (type, data) in EnemiesMetadata)
		{
			if(data.MinSpawnLevel == level)
			{
				AllowedEnemyTypes.Add(type);
			}
		}
	}

	public static EnemyType GetRandomEnemyType()
	{
		var randIndex = (int)(GD.Randi() % AllowedEnemyTypes.Count);
		EnemyType type = AllowedEnemyTypes[randIndex];
		return type;
	}
}
