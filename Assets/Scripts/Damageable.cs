using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public ulong max_health, current_health;

    public ulong GetMaxHealth { get { return this.max_health; } set { this.max_health = value; } }
    public ulong GetCurrentHealth { get { return this.current_health; } set { this.current_health = value; } }

    public void Damage(ulong amount)
    {
        if (this.GetCurrentHealth < amount)
            this.GetCurrentHealth = 0;
        else
            this.GetCurrentHealth -= amount;
    }

    public void Heal(ulong amount)
    {
        if (this.GetMaxHealth - this.GetCurrentHealth < amount)
            this.GetCurrentHealth = this.GetMaxHealth;
        else
            this.GetCurrentHealth += amount;
    }

    public bool IsAlive()
    {
        return this.GetCurrentHealth > 0;
    }
}
