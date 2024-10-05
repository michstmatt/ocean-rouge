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

public abstract class WeaponMetadata
{
	public float FireRate { get; private set; }
	public float CoolDown { get; private set; }
	public string WeaponScene { get; private set; }
	public int WeaponLevel { get; private set; }
	public int WeaponCount { get; private set; }
	public float DamageAmount { get; set; }
	public float Speed { get; private set; }
	public int HitCount { get; set; }
	public WeaponFireType FireType { get; set; } = WeaponFireType.Direction;
	public List<WeaponLevelUp> WeaponLevelUps { get; set; }

	public WeaponMetadata(float fireRate, float coolDown, string weaponScene, float speed, float damageAmount, WeaponFireType fireType, WeaponLevelUp[] levelUps, int weaponLevel = 0, int weaponCount = 1, int hitCount = 1)
	{
		FireRate = fireRate;
		CoolDown = coolDown;
		WeaponScene = weaponScene;
		Speed = speed;
		FireType = fireType;
		WeaponLevel = weaponLevel;
		WeaponCount = weaponCount;
		DamageAmount = damageAmount;
		HitCount = hitCount;
		WeaponLevelUps = new List<WeaponLevelUp>(){
			new WeaponLevelUp(){ Type= WeaponLevelUp.WeaponLevelUpType.Noop}
		};
		WeaponLevelUps.AddRange(levelUps);
	}

	public WeaponMetadata Instantiate()
	{
		return this.MemberwiseClone() as WeaponMetadata;
	}


	private void HandleLevelUp(WeaponLevelUp weaponLevelUp)
	{
		GD.Print($"Weapon Level Up {WeaponCount}");
		switch (weaponLevelUp.Type)
		{
			case WeaponLevelUp.WeaponLevelUpType.AnotherProjectile:
				this.WeaponCount++;
				break;
			case WeaponLevelUp.WeaponLevelUpType.CooldownReduction:
				this.CoolDown *= 1-weaponLevelUp.Amount;
				break;
			case WeaponLevelUp.WeaponLevelUpType.FireRateReduction:
				this.FireRate *= 1-weaponLevelUp.Amount;
				break;
			default:
				return;
		}
		GD.Print($"Weapon Level Up {WeaponCount}");
	}

	public void LevelUp()
	{
		this.WeaponLevel++;
		if (WeaponLevel >= this.WeaponLevelUps.Count)
		{
			this.WeaponLevel--;
			return;
		}
		var levelUpType = this.WeaponLevelUps[WeaponLevel];
		HandleLevelUp(levelUpType);
	}

	public class Sonar : WeaponMetadata
	{
		public Sonar() : base(
			fireRate: 5,
			coolDown: 2,
			weaponScene: "res://Projectile/Types/Sonar.tscn",
			speed: 1,
			damageAmount: 5,
			fireType: WeaponFireType.Direction,
			levelUps: new WeaponLevelUp[]
			{
			new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.FireRateReduction,
				Amount = 0.05f
			},
			new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.CooldownReduction,
				Amount = 0.05f
			},
			new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.FireRateReduction,
				Amount = 0.05f
			},
			new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.AnotherProjectile,
				Amount = 1
			}
			},
			hitCount: 10
		)
		{ }

	}

	public class Anchor : WeaponMetadata
	{
		public Anchor() : base
		(
			fireRate: 1,
			coolDown: 4,
			weaponScene: "res://Projectile/Types/Anchor.tscn",
			speed: 800,
			damageAmount: 5,
			fireType: WeaponFireType.Direction,
			levelUps: new WeaponLevelUp[]{
		new WeaponLevelUp()
		{
			Type = WeaponLevelUp.WeaponLevelUpType.FireRateReduction,
			Amount = 0.1f
		},
		new WeaponLevelUp()
		{
			Type = WeaponLevelUp.WeaponLevelUpType.CooldownReduction,
			Amount = 0.1f
		},
		new WeaponLevelUp()
		{
			Type = WeaponLevelUp.WeaponLevelUpType.FireRateReduction,
			Amount = 0.1f
		},
		new WeaponLevelUp()
		{
			Type = WeaponLevelUp.WeaponLevelUpType.AnotherProjectile,
			Amount = 1
		}
			}
		)
		{ }
	}

	public class Seashell : WeaponMetadata
	{
		public Seashell() : base(
			fireRate: 0.1f,
			coolDown: 4,
			weaponScene: "res://Projectile/Types/Seashell.tscn",
			speed: 600,
			damageAmount: 5,
			fireType: WeaponFireType.PlayerMovement,
			levelUps: new WeaponLevelUp[]
			{
			new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.CooldownReduction,
				Amount = 0.05f
			},
			new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.AnotherProjectile,
				Amount = 1
			},
			new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.CooldownReduction,
				Amount = 0.05f
			},
			new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.AnotherProjectile,
				Amount = 1
			}
			},
			weaponCount: 2
		)
		{ }
	}

	public class Torpedo : WeaponMetadata
	{
		public Torpedo() : base(
			fireRate: 1f,
			coolDown: 6,
			weaponScene: "res://Projectile/Types/Torpedo.tscn",
			speed: 600,
			damageAmount: 10,
			fireType: WeaponFireType.Enemy,
			levelUps: new WeaponLevelUp[]{
		new WeaponLevelUp()
		{
			Type = WeaponLevelUp.WeaponLevelUpType.AnotherProjectile,
			Amount = 1
		},
		new WeaponLevelUp()
		{
			Type = WeaponLevelUp.WeaponLevelUpType.CooldownReduction,
			Amount = 0.05f
		},
		new WeaponLevelUp()
		{
			Type = WeaponLevelUp.WeaponLevelUpType.AnotherProjectile,
			Amount = 1
		}
			}
		)
		{ }
	}

	public class Trident : WeaponMetadata
	{
		public Trident() : base
		(
			fireRate: 1f,
			coolDown: 6,
			weaponScene: "res://Projectile/Types/Trident.tscn",
			speed: 550,
			damageAmount: 5,
			fireType: WeaponFireType.Enemy,
			levelUps: new WeaponLevelUp[] {
		new WeaponLevelUp()
		{
			Type = WeaponLevelUp.WeaponLevelUpType.FireRateReduction,
			Amount = 0.05f
		},
		new WeaponLevelUp()
		{
			Type = WeaponLevelUp.WeaponLevelUpType.AnotherProjectile,
			Amount = 1
		},
		new WeaponLevelUp()
		{
			Type = WeaponLevelUp.WeaponLevelUpType.CooldownReduction,
			Amount = 0.05f
		},
		new WeaponLevelUp()
		{
			Type = WeaponLevelUp.WeaponLevelUpType.FireRateReduction,
			Amount = 0.05f
		},
		new WeaponLevelUp()
		{
			Type = WeaponLevelUp.WeaponLevelUpType.AnotherProjectile,
			Amount = 1
		}
			}
		)
		{ }
	}
}

public enum WeaponFireState
{
	Fire,
	Delay,
	Cooldown,
	DamageIncrease
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
		if (FireState == WeaponFireState.Fire)
		{
			ProjectileCount++;
			if (ProjectileCount == Metadata.WeaponCount)
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
			if (time >= NextFireTime)
			{
				FireState = WeaponFireState.Fire;
			}
		}
		else if (FireState == WeaponFireState.Cooldown)
		{
			if (time >= NextFireTime)
			{
				FireState = WeaponFireState.Fire;
			}
		}
		return FireState;
	}
}

public class WeaponLevelUp
{
	public enum WeaponLevelUpType
	{
		AnotherProjectile,
		CooldownReduction,
		FireRateReduction,
		DamageIncrease,
		Noop
	}
	public WeaponLevelUpType Type;
	public float Amount;
}

public class WeaponFactory
{
	public static void Reinit()
	{
		WeaponsMetadata = new Dictionary<WeaponType, WeaponMetadata>()
		{
			{WeaponType.Anchor, new WeaponMetadata.Anchor()},
			{WeaponType.Seashell, new WeaponMetadata.Seashell()},
			{WeaponType.Sonar, new WeaponMetadata.Sonar()},
			{WeaponType.Torpedo, new WeaponMetadata.Torpedo()},
			{WeaponType.Trident, new WeaponMetadata.Trident()}
		};
	}
	public static Dictionary<WeaponType, WeaponMetadata> WeaponsMetadata { get; private set; }

	public static WeaponMetadata GetWeaponMetadata(WeaponType weaponType)
	{
		return WeaponsMetadata[weaponType];
	}

}