using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SerializedMonoBehaviour
{
    public Dictionary<string, string> keybindItemTypeIDPairs = new();
    public Dictionary<string, List<InventoryItem>> inventory = new();

    public void AddItem(InventoryItem item_)
    {
        if (!inventory.ContainsKey(item_.itemTypeID))
        {
            inventory.Add(item_.itemTypeID, new List<InventoryItem>());
        }

        inventory[item_.itemTypeID].Add(item_);
    }

    public void RemoveItem(InventoryItem item_)
    {
        if (inventory.ContainsKey(item_.itemTypeID))
        {
            inventory[item_.itemTypeID].Remove(item_);
        }
    }

    public void Update()
    {
        CheckIfAnItemIsBeingActivated(keybindItemTypeIDPairs);
    }

    public void CheckIfAnItemIsBeingActivated(Dictionary<string, string> bindItemTypeIDPairs_)
    {
        foreach (KeyValuePair<string, string> pair in bindItemTypeIDPairs_)
        {
            if (Input.GetKeyDown(pair.Key))
            {
                if (inventory[pair.Value].Count > 0)
                {
                    UseItem(inventory[pair.Value][0]);
                }                                
            }
        }
    }

    public void UseItem(InventoryItem item_)
    {        
        item_.UseFromInventory();
    }
}

