using System.Collections.Generic;
using System.Linq;
using Godot;

public class StatsComponent
{
    public float BaseDexterity { get; set; }
    public float BaseSpeed { get; set; }
    public float BaseVitality { get; set; }
    public float BaseAttack { get; set; }

    public float MaxHealth { get;  set; }
    public float CurrentHealth { get; private set; }

    private List<float> dexterityModifiers = new List<float>();
    private List<float> speedModifiers = new List<float>();
    private List<float> attackModifiers = new List<float>();

    public StatsComponent(float maxHealth, float moveSpeed, float dexterity, float vitality, float attack)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        this.BaseSpeed = moveSpeed;
        this.BaseDexterity = dexterity;
        this.BaseVitality = vitality;
        this.BaseAttack = attack;
    }

    public void Heal(float amount)
    {
        CurrentHealth = System.Math.Min(CurrentHealth + amount, MaxHealth);
    }

    /// <summary>
    /// Applies damage to the entity. Returns true if the entity's health drops to zero or below.
    /// </summary>
    /// <param name="amount">Amount of damage to apply</param>
    /// <returns>True if the entity's health is zero or below after taking damage, otherwise false</returns>
    public bool TakeDamage(float amount)
    {

        CurrentHealth = System.Math.Max(CurrentHealth - amount, 0);
        return CurrentHealth<=0;
    }

    public float CurrentDexterity => BaseDexterity + dexterityModifiers.Sum();
    public float CurrentSpeed => BaseSpeed + speedModifiers.Sum();
    public float CurrentAttack => BaseAttack + attackModifiers.Sum();

    public void AddDexterityModifier(float modifier) => dexterityModifiers.Add(modifier);
    public void RemoveDexterityModifier(float modifier) => dexterityModifiers.Remove(modifier);

    public void AddSpeedModifier(float modifier) => speedModifiers.Add(modifier);
    public void RemoveSpeedModifier(float modifier) => speedModifiers.Remove(modifier);

    public void AddAttackModifier(float modifier) => attackModifiers.Add(modifier);
    public void RemoveAttackModifier(float modifier) => attackModifiers.Remove(modifier);

    public void LevelUp(UpdateStatus status)
    {
        MaxHealth += 10;
        Heal(10);
		switch(status){
			case UpdateStatus.Attack:
				this.BaseAttack += 0.2f;
				break;
			case UpdateStatus.Dexterity:
				this.BaseDexterity += 0.1f;
				break;
			case UpdateStatus.Speed:
				this.BaseSpeed += this.BaseSpeed * 0.05f;
				break;
		}
        //GD.Print("Entity leveled up!");
    }
}