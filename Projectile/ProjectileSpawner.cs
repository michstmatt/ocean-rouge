using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ProjectileSpawner : Node
{
	public Dictionary<WeaponType, PackedScene> WeaponScenes;

	private int GameTime;

	public override void _Ready()
	{
		WeaponScenes = new Dictionary<WeaponType, PackedScene>();
		foreach(var weaponType in WeaponFactory.WeaponTypes)
		{
			var scene = WeaponFactory.GetWeaponScene(weaponType);
			WeaponScenes.Add(weaponType, (PackedScene)ResourceLoader.Load(scene));
		}
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

		foreach((WeaponType weaponType, PackedScene scene) in WeaponScenes)
		{
			var freq = WeaponFactory.GetFireRate(weaponType);
			if(GameTime % freq != 0)
			{
				continue;
			}
			Projectile projectile = scene.Instantiate<Projectile>();

			projectile.Position = parent.Position;

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
