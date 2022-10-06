using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public GameObject owner;
    public bool isHealthPotion, isManaPotion;
    public float value;

    public void ConsumePotion()
    {
        Stats ownerStats;

        if (owner.TryGetComponent<Stats>(out ownerStats))
        {
            if(isHealthPotion)
            {                        
                ownerStats.HealHealth(value);
                Destroy(gameObject);
                return;
            }   

            if(isManaPotion)
            {   
                ownerStats.IncreaseMana(value);
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            Debug.Log("The owner of this potion isn't mortal!");
        }        
    }

}
