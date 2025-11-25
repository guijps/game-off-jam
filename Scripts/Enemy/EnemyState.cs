using Godot;
using System;

[Flags]
public enum EnemyState 
{
	Idle = 0,
	Move = 1 << 0,
	Chase = 1 << 1,
	Attack = 1 << 2,
	Flee = 1 << 3,
	Dead = 1 << 4
}
