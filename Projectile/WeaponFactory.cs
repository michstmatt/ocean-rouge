using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

public enum WeaponFireType
{
	PlayerMovement,
	Enemy,
	Direction
}

public enum WeaponType
{
	Torpedo,
	Sonar,
	Anchor,
	Seashell,
	Trident
}

public class WeaponMetadata
{
	public float FireRate { get; set; }
	public float CoolDown { get; set; }
	public string WeaponScene { get; set; }
	public int WeaponLevel { get; set; } = 1;
	public int WeaponCount { get; set; } = 1;
}

public enum WeaponFireState
{
	Fire,
	Delay,
	Cooldown,
}
public class WeaponState
{
	public WeaponFireState FireState;
	public int ProjectileCount;
	public double NextFireTime;

	public readonly WeaponMetadata Metadata;
	public readonly WeaponType Type;

	public WeaponState(WeaponType type, WeaponMetadata weaponMetadata)
	{
		FireState = WeaponFireState.Delay;
		NextFireTime = -1;
		ProjectileCount = 0;
		Type = type;
		Metadata = weaponMetadata;
	}

	public WeaponFireState UpdateState(double time)
	{
		if(FireState == WeaponFireState.Fire)
		{
			ProjectileCount++;
			if(ProjectileCount == Metadata.WeaponCount)
			{
				ProjectileCount = 0;
				NextFireTime = time + Metadata.CoolDown;
				FireState = WeaponFireState.Cooldown;
			}
			else
			{
				NextFireTime = time + Metadata.FireRate;
				FireState = WeaponFireState.Delay;
			}
		}
		else if (FireState == WeaponFireState.Delay)
		{
			if(time >= NextFireTime)
			{
				FireState = WeaponFireState.Fire;
			}
		}
		else if (FireState == WeaponFireState.Cooldown)
		{
			if(time >= NextFireTime)
			{
				FireState = WeaponFireState.Fire;
			}
		}
		return FireState;
	}
}


public class WeaponFactory
{
	public static WeaponType[] WeaponTypes = Enum.GetValues<WeaponType>();

	private static Dictionary<WeaponType, WeaponMetadata> WeaponsMetadata = new Dictionary<WeaponType, WeaponMetadata>()
	{
		{ WeaponType.Anchor, new WeaponMetadata()
		{
			CoolDown=4,
			WeaponScene = "res://Projectile/Types/Anchor.tscn"
		}},
		{ WeaponType.Seashell, new WeaponMetadata()
		{
			CoolDown=1,
			WeaponScene = "res://Projectile/Types/Seashell.tscn",
			WeaponCount=2,
			FireRate=0.1f,
		}},
		{ WeaponType.Sonar, new WeaponMetadata()
		{
			CoolDown=2,
			WeaponScene = "res://Projectile/Types/Sonar.tscn"
		}},
		{ WeaponType.Torpedo, new WeaponMetadata()
		{
			CoolDown=6,
			WeaponScene = "res://Projectile/Types/Torpedo.tscn"
		}},
		{ WeaponType.Trident, new WeaponMetadata()
		{
			CoolDown=6,
			WeaponScene = "res://Projectile/Types/Trident.tscn"
		}}
	};

	public static WeaponMetadata GetWeaponMetadata(WeaponType weaponType)
	{
		return WeaponsMetadata[weaponType];
	}
}

