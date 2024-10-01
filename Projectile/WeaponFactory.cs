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
	public float FireRate { get; set; }
	public float CoolDown { get; set; }
	public string WeaponScene { get; set; }
	public int WeaponLevel { get; set; } = 0;
	public int WeaponCount { get; set; } = 1;
	public float DamageAmount { get; set; } = 1;
	public float Speed {get; set;}
	public int HitCount {get; set;} = 1;

	public WeaponMetadata Instantiate()
	{
		return this.MemberwiseClone() as WeaponMetadata;
	}

	public List<WeaponLevelUp> WeaponLevelUps { get; set; } = new List<WeaponLevelUp>()
	{
		new WeaponLevelUp(){ Type= WeaponLevelUp.WeaponLevelUpType.Noop}
	};
	private void HandleLevelUp(WeaponLevelUp weaponLevelUp)
	{
		switch (weaponLevelUp.Type)
		{
			case WeaponLevelUp.WeaponLevelUpType.AnotherProjectile:
				this.WeaponCount++;
				break;
			case WeaponLevelUp.WeaponLevelUpType.CooldownReduction:
				if (this.CoolDown > weaponLevelUp.Amount)
					this.CoolDown -= weaponLevelUp.Amount;
				break;
			case WeaponLevelUp.WeaponLevelUpType.FireRateReduction:
				if (this.FireRate > weaponLevelUp.Amount)
					this.FireRate -= weaponLevelUp.Amount;
				break;
			default:
				return;
		}
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
		public Sonar()
		{
			CoolDown = 2;
			DamageAmount = 5;
			Speed = 1;
			HitCount = 10;
			WeaponScene = "res://Projectile/Types/Sonar.tscn";
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.FireRateReduction,
				Amount = 0.1f
			});
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.CooldownReduction,
				Amount = 0.1f
			});
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.FireRateReduction,
				Amount = 0.1f
			});
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.AnotherProjectile,
				Amount = 1
			});
		}
	}

	public class Anchor : WeaponMetadata
	{
		public Anchor()
		{
			CoolDown = 4;
			DamageAmount = 5;
			HitCount = int.MaxValue;
			Speed = 800;
			WeaponScene = "res://Projectile/Types/Anchor.tscn";
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.FireRateReduction,
				Amount = 0.1f
			});
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.CooldownReduction,
				Amount = 0.1f
			});
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.FireRateReduction,
				Amount = 0.1f
			});
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.AnotherProjectile,
				Amount = 1
			});
		}
	}

	public class Seashell : WeaponMetadata
	{
		public Seashell()
		{
			CoolDown = 4;
			DamageAmount = 5;
			Speed = 600;
			WeaponScene = "res://Projectile/Types/Seashell.tscn";
			WeaponCount = 2;
			FireRate = 0.1f;
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.CooldownReduction,
				Amount = 0.1f
			});
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.AnotherProjectile,
				Amount = 1
			});
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.CooldownReduction,
				Amount = 0.1f
			});
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.AnotherProjectile,
				Amount = 1
			});
		}
	}

	public class Torpedo : WeaponMetadata
	{
		public Torpedo()
		{
			CoolDown = 6;
			DamageAmount = 10;
			Speed = 600;
			WeaponScene = "res://Projectile/Types/Torpedo.tscn";
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.AnotherProjectile,
				Amount = 1
			});
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.CooldownReduction,
				Amount = 0.1f
			});
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.AnotherProjectile,
				Amount = 1
			});
		}
	}

	public class Trident : WeaponMetadata
	{
		public Trident()
		{
			CoolDown = 6;
			DamageAmount = 5;
			Speed = 550;
			HitCount = 10;
			WeaponScene = "res://Projectile/Types/Trident.tscn";
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.FireRateReduction,
				Amount = 0.1f
			});
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.AnotherProjectile,
				Amount = 1
			});
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.CooldownReduction,
				Amount = 0.1f
			});
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.FireRateReduction,
				Amount = 0.1f
			});
			WeaponLevelUps.Add(new WeaponLevelUp()
			{
				Type = WeaponLevelUp.WeaponLevelUpType.AnotherProjectile,
				Amount = 1
			});
		}
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