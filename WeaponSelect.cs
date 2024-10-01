using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class WeaponSelect : CanvasLayer
{
	ItemList WeaponList;
	// Called when the node enters the scene tree for the first time.

	[Signal]
	public delegate void WeaponSelectedEventHandler(WeaponType type);

	[Signal]
	public delegate void MenuShowEventHandler(bool menuShowing);

	protected List<WeaponType> Weapons;
	protected int SelectedIndex = 0;
	public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.Always;
		WeaponList = GetNode<ItemList>("WeaponList");

		SignalManager.Instance.NewWeaponAvailable += ShowMenu;
		SignalManager.Instance.GameOver += (isOver) =>
		{
			if (isOver == false)
			{
				CreateList();
			}
		};
	}

	public void ShowMenu()
	{
		Visible = true;
		CreateList();
		SignalManager.Instance.EmitSignal(SignalManager.SignalName.PauseGame, true);
	}

	public void CreateList()
	{
		Weapons = new List<WeaponType>();
		WeaponList.Clear();
		foreach(var (weaponType, weapon) in WeaponFactory.WeaponsMetadata)
		{
			var t2d = GetNode<AnimatedSprite2D>("Art")?.SpriteFrames.GetFrameTexture(weaponType.ToString(), 0);
			string text = $"{weaponType} Level: {weapon.WeaponLevel + 1}";
			WeaponList.AddItem(text, selectable: true, icon: t2d);
			Weapons.Add(weaponType);
		}
	}
	public void ItemSelected(int index)
	{
		SelectedIndex = index;
	}
	public void OnSelectButton()
	{
		var weaponType = Weapons[SelectedIndex];
		var weapon = WeaponFactory.GetWeaponMetadata(weaponType);
		weapon.LevelUp();
		EmitSignal(SignalName.WeaponSelected, (int)weaponType);
		SignalManager.Instance.EmitSignal(SignalManager.SignalName.PauseGame, false);
		Visible = false;
	}

}
