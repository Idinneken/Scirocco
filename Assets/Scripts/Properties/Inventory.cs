using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class Inventory : SerializedMonoBehaviour
{
    public Dictionary<string, List<GameObject>> items = new();
    public Dictionary<string, string> keybindItemTypeIDPairs = new();    

    private Invoker invoker = new();

    public void AddItem(GameObject item_)
    {        
        InventoryItem itemComponent;
        if (!item_.TryGetComponent<InventoryItem>(out itemComponent))
        {
            print("This isn't an inventory item.");
            return;
        }
        
        if (!items.ContainsKey(itemComponent.itemTypeID))
        {
            items.Add(itemComponent.itemTypeID, new List<GameObject>());
        }

        item_.SetActive(false);

        items[itemComponent.itemTypeID].Add(item_);
    }

    public void RemoveItem(InventoryItem item_)
    {
        if (items.ContainsKey(item_.itemTypeID))
        {
            items[item_.itemTypeID].Remove(item_.gameObject);
        }
    }

    public void Update()
    {
        CheckIfAnItemIsBeingActivated(keybindItemTypeIDPairs);
    }
                    
    public void CheckIfAnItemIsBeingActivated(Dictionary<string, string> bindItemTypeIDPairs_)
    {
        foreach (KeyValuePair<string, string> bindItemPair in bindItemTypeIDPairs_)
        {
            List<GameObject> itemList;

            if (Input.GetKeyDown(bindItemPair.Key) && items.TryGetValue(bindItemPair.Value, out itemList))
            {            
                GameObject item = items[bindItemPair.Value][0];        
                InventoryItem itemComponent = item.GetComponent<InventoryItem>();

                if (itemComponent.activatesOnUse)
                {                    
                    invoker.ParseActions(itemComponent?.onUseActions);  
                }
                
                if (itemComponent.deletedOnUse)
                {
                    items[bindItemPair.Value].Remove(item);         

                    if(items[bindItemPair.Value].IsEmpty())
                    {
                        items.Remove(bindItemPair.Value);
                    }

                    Destroy(item);
                }

            }
        }
    }

}

