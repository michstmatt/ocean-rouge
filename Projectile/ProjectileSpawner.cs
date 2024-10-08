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

	public bool IsPaused = false;
	private int GameTime;
	//public List<WeaponType> Weapons;
	public override void _Ready()
	{
		base._Ready();
		Weapons = new List<WeaponState>();
		WeaponScenes = new Dictionary<WeaponType, PackedScene>();
		SignalManager.Instance.PauseGame += UpdateTimer;
		SignalManager.Instance.GameOver += Reset;

		TotalTime = 0;
	}

	protected void Reset(bool isOver)
	{
		if (isOver)
		{
			TotalTime = 0;
			Weapons.Clear();
		}
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
		IsPaused = pause;
		if (pause == false)
		{
			timer.Start();
			GameTime = 0;
		}
		else
		{
			timer.Stop();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (IsPaused)
		{
			return;
		}
		TotalTime += delta;
		if (TotalTime > double.MaxValue)
		{
			TotalTime = 0;
		}
		var (player, enemeies) = GetInformation();
		FireWeapons(player,enemeies);
	}

	public (Player, List<Vector2>) GetInformation()
	{
		var parent = GetParent<Player>();
		var sorted = GetTree().GetNodesInGroup(Constants.MobGroup)
			.Select(n => 
			{
				var enemyPos = (n as Node2D).Position;
				var distance = parent.Position.DistanceTo(enemyPos);
				return (enemyPos, distance);
			})
			.OrderBy(item => item.distance)
			.Select(item => item.enemyPos);
			

		return (parent, sorted.ToList());
	}

	public void FireWeapons(Player player, List<Vector2> nearestEnemies)
	{
		foreach (var weaponState in Weapons)
		{
			var currentState = weaponState.UpdateState(TotalTime);
			if (currentState == WeaponFireState.Fire)
			{
				var nearestEnemy = nearestEnemies[weaponState.ProjectileCount];
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

		var metadata = WeaponFactory.WeaponsMetadata[projectile.WeaponType];

		if (metadata.FireType == WeaponFireType.PlayerMovement)
		{
			projectile.Direction = parent.LastMove;
		}
		else if (metadata.FireType == WeaponFireType.Enemy)
		{
			projectile.Direction = (nearestEnemy - projectile.Position);
		}
		AddChild(projectile);
		projectile.AddToGroup(Constants.ProjectileGroup);
	}
}
