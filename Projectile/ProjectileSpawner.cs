using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ProjectileSpawner : Node
{
	public Dictionary<WeaponType, PackedScene> WeaponScenes;

	private int GameTime;
	public List<WeaponType> Weapons;
	public override void _Ready()
	{
		Weapons = new List<WeaponType>();
		WeaponScenes = new Dictionary<WeaponType, PackedScene>();
	}

	protected void AddWeaponScene(WeaponType weaponType)
	{
		if (!WeaponScenes.ContainsKey(weaponType))
		{
			var scene = WeaponFactory.GetWeaponScene(weaponType);
			WeaponScenes.Add(weaponType, (PackedScene)ResourceLoader.Load(scene));
		}
		Weapons.Add(weaponType);
	}

	public void OnWeaponAdded(int weaponId)
	{
		WeaponType type = (WeaponType)weaponId;
		AddWeaponScene(type);
	}

	public void UpdateTimer(bool start)
	{
		var timer = GetNode<Timer>("AutoFireTimer");
		if(start)
		{
			timer.Start();
			GameTime = 0;
		}
		else
		{
			timer.Stop();
			Weapons.Clear();
		}
	}
	
	private void FireProjectile()
	{
		GameTime += 1;

		var parent = GetParent<Player>();

		Vector2 nearestEnemy = Vector2.Zero;
		float nearest = float.MaxValue;
		foreach(Node2D enemy in GetTree().GetNodesInGroup(Constants.MobGroup))
		{
			var distance = parent.Position.DistanceTo(enemy.Position);
			if (distance<nearest)
			{
				nearest = distance;
				nearestEnemy = enemy.Position;
			}
		}

		foreach(WeaponType weaponType in Weapons)
		{
			var scene = WeaponScenes.GetValueOrDefault(weaponType);
			var freq = WeaponFactory.GetFireRate(weaponType);
			if(GameTime % freq != 0)
			{
				continue;
			}
			Projectile projectile = scene.Instantiate<Projectile>();

			projectile.Position = parent.Position + new Vector2(10*GD.Randf() - 5, 10 * GD.Randf() - 5);

			if (projectile.WeaponFireType == WeaponFireType.PlayerMovement)
			{
				projectile.Direction = parent.LastMove;
			} else if(projectile.WeaponFireType == WeaponFireType.Enemy)
			{
				projectile.Direction = (nearestEnemy - projectile.Position);
			}


			AddChild(projectile);
			projectile.AddToGroup(Constants.ProjectileGroup);
		}
	}
}
