using System;
using Godot;
public class Character
{
	public int Speed { get; set; } = 400;

	public int Health { get; set; } = 100;

	public AnimatedSprite2D Sprite;
	public CollisionShape2D CollisionShape;

	public bool IsDead() => Health <= 0 ;
}
