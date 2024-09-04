using Godot;
using System;

public partial class AbstractCharacter: Area2D
{
	[Export]
	public int Speed {get; set;} = 400;
	
	[Export]
	public int Heath {get; set;} = 100;
	
	protected AnimatedSprite2D sprite;
	
	[Signal]
	public delegate void HitEventHandler();
}
