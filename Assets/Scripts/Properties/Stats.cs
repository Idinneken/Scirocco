using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float startHealth;
    public float health;
    public float maxHealth;
    public float minHealth;
    [Space]
    public float startMana;
    public float mana;
    public float maxMana;
    public float minMana;

    void Start()
    {
        health = startHealth;
        VerifyHealth();

        mana = startMana;
        VerifyMana();
    }

    public void HealHealth(float amount_)
    {
        health += amount_;
        VerifyHealth();        
    }   

    public void HurtHealth(float amount_)
    {
        health -= amount_;
        VerifyHealth();
    } 

    public void VerifyHealth()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health < minHealth)
        {
            Die();    
        }
    }

    public void IncreaseMana(float amount_)
    {
        mana += amount_;
        VerifyMana();
    }

    public void DecreaseMana(float amount_)
    {
        mana -= amount_;
        VerifyMana();
    }    

    public void VerifyMana()
    {
        if (mana > maxMana)
        {
            mana = maxMana;
        }
        else if (mana < minMana)
        {
            mana = minMana; 
        }
    }

    public void Die()
    {
        //Todo
    }
}

// public class ActorResource
// {
//     float value, maxValue, minValue;

    
// }
