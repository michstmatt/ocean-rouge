using System;
using System.Linq.Expressions;
using Godot;


public class EnemyType
{
	public int Health {get; set;}
	public float Speed {get; set;}
	public string AnimationName {get; set;}
	public float Scale {get; set;} = 1.0f;
	public int Damage {get; set;} = 10;

	public static EnemyType Seahorse()
	{
		return new EnemyType
		{
			AnimationName = "Seahorse",
			Speed = 100,
			Health = 10,
		};
	}
	public static EnemyType Whale()
	{
		return new EnemyType
		{
			AnimationName = "Whale",
			Speed = 10,
			Health = 100,
			Scale = 3.0f
		};
	}

}

public class EnemyStateManager
{
	public Func<EnemyType>[] EnemyTypes = new Func<EnemyType>[]{
		EnemyType.Seahorse,
		EnemyType.Whale
	};

	public EnemyType GetRandomEnemy()
	{
		int index = (int)(GD.Randi() % EnemyTypes.Length);
		return EnemyTypes[index]();
	}
}
