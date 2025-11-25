using System.Runtime.ExceptionServices;
using Godot;

public abstract partial class Weapon:Node2D
{	[Export] public PackedScene WeaponProjectileScene;
	[Export] public CSharpScript WeaponProjectileScript;

    public abstract void Fire(ProjectileStatsComponent projectileStats);
}