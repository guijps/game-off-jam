using Godot;

public partial class TargetedWeapon : Weapon
{
    public ProjectileStatsComponent Stats;
    
    protected Node2D Target;
    public override void _Ready()
    {
        Stats = new ProjectileStatsComponent(10, 150f, 3, false);		
        
    }
	protected virtual Vector2 GetTargetPosition()
    {
		throw new System.NotImplementedException();
    }
    public override void Fire(ProjectileStatsComponent projectileStats)
    {
        var root = GetTree().CurrentScene;
 		Vector2 direction = (GetTargetPosition() - GlobalPosition).Normalized();
		var bullet =  WeaponProjectileScene.Instantiate<Projectile>();
		float rotation = Mathf.Atan2(direction.Y, direction.X);
        bullet.SetStats(projectileStats);
		bullet.Rotation = rotation;
		bullet.SetVelocity(direction);
		bullet.GlobalPosition = GlobalPosition;  // Spawn at player's position
		if(GetTree().CurrentScene != null)
        {
            GD.Print("Current Scene: " + root.Name);
            root.AddChild(bullet);
            return;
        }
          GD.Print("GetParent Scene: " +  GetParent().Name);
        GetParent().AddChild(bullet);
        
    }
}