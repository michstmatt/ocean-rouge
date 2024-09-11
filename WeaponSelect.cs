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

	protected WeaponType[] Weapons;
	protected int SelectedIndex = 0;
	public override void _Ready()
	{
		WeaponList = GetNode<ItemList>("WeaponList");
		Weapons = Enum.GetValues<WeaponType>();
		CreateList();
	}

	public void ShowMenu()
	{
		Visible = true;
		EmitSignal(SignalName.MenuShow, true);
	}

	public void CreateList()
	{
		WeaponList.Clear();

		foreach(var weaponType in Weapons)
		{
			WeaponList.AddItem(weaponType.ToString(), selectable: true);
		}
	}

	public void ItemSelected(int index)
	{
		SelectedIndex = index;
	}
	public void OnSelectButton()
	{
		EmitSignal(SignalName.MenuShow, false);
		EmitSignal(SignalName.WeaponSelected,(int)Weapons[SelectedIndex]);
		Visible = false;
	}

}
