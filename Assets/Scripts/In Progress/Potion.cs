using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{    
    public bool isHealthPotion, isManaPotion;
    public float value;

    public void ConsumePotion()
    {
        Stats ownerStats;        

        if (GetComponent<InventoryItem>().owner.TryGetComponent(out ownerStats))
        {
            if(isHealthPotion)
            {                        
                ownerStats.HealHealth(value);                
                return;
            }   

            if(isManaPotion)
            {   
                ownerStats.IncreaseMana(value);                
                return;
            }
        }
        else
        {
            Debug.Log("The owner of this potion isn't mortal!");
        }        
    }

}
