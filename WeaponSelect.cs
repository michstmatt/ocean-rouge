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
		SignalManager.Instance.EmitSignal(SignalManager.SignalName.PauseGame, true);
	}

	public void CreateList()
	{
		Weapons = Enum.GetValues<WeaponType>().ToList();
		WeaponList.Clear();
		foreach(var weaponType in Weapons)
		{
			var t2d = GetNode<AnimatedSprite2D>("Art")?.SpriteFrames.GetFrameTexture(weaponType.ToString(), 0);
			WeaponList.AddItem(weaponType.ToString(), selectable: true, icon: t2d);
		}
	}

	public void ItemSelected(int index)
	{
		SelectedIndex = index;
	}
	public void OnSelectButton()
	{
		var weapon = Weapons[SelectedIndex];
		EmitSignal(SignalName.WeaponSelected,(int)weapon);
		Weapons.RemoveAt(SelectedIndex);
		WeaponList.RemoveItem(SelectedIndex);
		SignalManager.Instance.EmitSignal(SignalManager.SignalName.PauseGame, false);
		Visible = false;
	}

}
