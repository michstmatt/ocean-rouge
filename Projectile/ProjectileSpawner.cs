using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class ProjectileSpawner : Node
{
	public Dictionary<WeaponType, PackedScene> WeaponScenes;
	public List<WeaponState> Weapons;

	public double TotalTime;

	private int GameTime;
	//public List<WeaponType> Weapons;
	public override void _Ready()
	{
		base._Ready();
		Weapons = new List<WeaponState>();
		WeaponScenes = new Dictionary<WeaponType, PackedScene>();
		SignalManager.Instance.PauseGame += UpdateTimer;
		TotalTime = 0;
	}

	protected void AddWeaponScene(WeaponType weaponType)
	{
		if (!WeaponScenes.ContainsKey(weaponType))
		{
			var scene = WeaponFactory.GetWeaponMetadata(weaponType).WeaponScene;
			WeaponScenes.Add(weaponType, (PackedScene)ResourceLoader.Load(scene));

		}
		var metadata = WeaponFactory.GetWeaponMetadata(weaponType);
		Weapons.Add(new WeaponState(weaponType, metadata));
	}

	public void OnWeaponAdded(int weaponId)
	{
		WeaponType type = (WeaponType)weaponId;
		AddWeaponScene(type);
	}

	public void UpdateTimer(bool pause)
	{
		var timer = GetNode<Timer>("AutoFireTimer");
		if (pause == false)
		{
			timer.Start();
			GameTime = 0;
		}
		else
		{
			timer.Stop();
			//Weapons.Clear();
		}
	}
	/*
		private async void FireWeaponTypes()
		{
			GameTime += 1;

			var parent = GetParent<Player>();

			Vector2 nearestEnemy = Vector2.Zero;
			float nearest = float.MaxValue;
			foreach (Node2D enemy in GetTree().GetNodesInGroup(Constants.MobGroup))
			{
				var distance = parent.Position.DistanceTo(enemy.Position);
				if (distance < nearest)
				{
					nearest = distance;
					nearestEnemy = enemy.Position;
				}
			}

			foreach (WeaponType weaponType in Weapons)
			{
				var scene = WeaponScenes.GetValueOrDefault(weaponType);
				var weaponMetadata = WeaponFactory.GetWeaponMetadata(weaponType);
				if (GameTime % weaponMetadata.CoolDown != 0)
				{
					continue;
				}
				await FireWeapon(weaponMetadata, scene, parent, nearestEnemy);
			}
		}

		public async Task FireWeapon(WeaponMetadata weaponMetadata, PackedScene scene, Player parent, Vector2 nearestEnemy)
		{

			for (int projectileCount = 0; projectileCount < weaponMetadata.WeaponCount; projectileCount++)
			{
				Projectile projectile = scene.Instantiate<Projectile>();

				projectile.Position = parent.Position + new Vector2(50 * GD.Randf() - 25, 50 * GD.Randf() - 25);

				if (projectile.WeaponFireType == WeaponFireType.PlayerMovement)
				{
					projectile.Direction = parent.LastMove;
				}
				else if (projectile.WeaponFireType == WeaponFireType.Enemy)
				{
					projectile.Direction = (nearestEnemy - projectile.Position);
				}
				AddChild(projectile);
				projectile.AddToGroup(Constants.ProjectileGroup);

				await ToSignal(GetTree().CreateTimer(weaponMetadata.FireRate), "timeout");

			}
				await ToSignal(GetTree().CreateTimer(weaponMetadata.CoolDown), "timeout");
		}
	*/

	public override void _Process(double delta)
	{
		base._Process(delta);
		TotalTime += delta;
		if (TotalTime > double.MaxValue)
		{
			TotalTime = 0;
		}
		var (player, nearestEnemy) = GetInformation();
		FireWeapons(player, nearestEnemy);
	}

	public (Player, Vector2) GetInformation()
	{
		var parent = GetParent<Player>();
		Vector2 nearestEnemy = Vector2.Zero;
		float nearest = float.MaxValue;
		foreach (Node2D enemy in GetTree().GetNodesInGroup(Constants.MobGroup))
		{
			var distance = parent.Position.DistanceTo(enemy.Position);
			if (distance < nearest)
			{
				nearest = distance;
				nearestEnemy = enemy.Position;
			}
		}

		return (parent, nearestEnemy);
	}

	public void FireWeapons(Player player, Vector2 nearestEnemy)
	{
		foreach (var weaponState in Weapons)
		{
			var currentState = weaponState.UpdateState(TotalTime);
			if (currentState == WeaponFireState.Fire)
			{
				// fire
				FireWeapon(weaponState, player, nearestEnemy);
			}
			else
			{
				// do nothing	
			}
		}
	}
	public void FireWeapon(WeaponState weaponState, Player parent, Vector2 nearestEnemy)
	{
		var scene = WeaponScenes.GetValueOrDefault(weaponState.Type);
		Projectile projectile = scene.Instantiate<Projectile>();

		projectile.Position = parent.Position + new Vector2(50 * GD.Randf() - 25, 50 * GD.Randf() - 25);

		if (projectile.WeaponFireType == WeaponFireType.PlayerMovement)
		{
			projectile.Direction = parent.LastMove;
		}
		else if (projectile.WeaponFireType == WeaponFireType.Enemy)
		{
			projectile.Direction = (nearestEnemy - projectile.Position);
		}
		AddChild(projectile);
		projectile.AddToGroup(Constants.ProjectileGroup);
	}
}
