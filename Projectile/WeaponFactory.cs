using System;
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
	Anchor
}


public class WeaponFactory
{
	public static int GetFireRate(WeaponType type)
	{
		switch(type)
		{
			case WeaponType.Anchor: return 4;
			case WeaponType.Torpedo: return 6;
			case WeaponType.Sonar: return 1;
			default: return 10;
		}
	}
	public static WeaponType[] WeaponTypes = Enum.GetValues<WeaponType>();
	public static string GetWeaponScene(WeaponType weaponType)
	{
		switch(weaponType)
		{
			case WeaponType.Torpedo:
				return "res://Projectile/Types/Torpedo.tscn";

			case WeaponType.Sonar:
				return "res://Projectile/Types/Sonar.tscn";

			case WeaponType.Anchor:
				return "res://Projectile/Types/Anchor.tscn";
			default:
				throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, "Unsupported enemy type");
		}
	}
}

