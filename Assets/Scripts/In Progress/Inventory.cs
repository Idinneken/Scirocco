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

        // for (int i = 0; i < itemComponent.associatedComponents.Count; i++)
        // {                        
        //     itemComponent.associatedComponents[i].
        // }

        // foreach(Component component in itemComponent.associatedComponents)
        // {
        //     Type associatedComponentType = component.GetType();
        //     if (component == item_.TryGetComponent<typeof(associatedComponentType)>)
            
        // }

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

            print(bindItemPair.Key);
            print(bindItemPair.Value);
            print(items.TryGetValue(bindItemPair.Value, out itemList));

            if (Input.GetKeyDown(bindItemPair.Key) && items.TryGetValue(bindItemPair.Value, out itemList))
            {            
                GameObject item = items[bindItemPair.Value][/*items[bindItemPair.Value].Count - 1*/0];        
                InventoryItem itemComponent = item.GetComponent<InventoryItem>();

                if (itemComponent.activatesOnUse)
                {
                    // itemComponent.ActivateItem(itemComponent?.onUseActions);    
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
                    
            
            
            
            // // if (Input.GetKeyDown(pair.Key) && !items[pair.Value].IsNullOrEmpty())
            // // {
            //     // List<GameObject> itemList;

            //     // if (items.TryGetValue(pair.Value))
            //     // {
            //         GameObject item = items[bindItemPair.Value][items[bindItemPair.Value].Count - 1];
            //         // print(items[pair.Value][0]);
            //         // print(items[pair.Value][1]);

            //         // items.TrimExcess();
            //         InventoryItem itemComponent = item.GetComponent<InventoryItem>();

            //         // if (itemComponent.activatesOnUse)
            //         // {
            //             itemComponent.ActivateItem(itemComponent?.onUseActions);
            //         // }
                
            //         if (itemComponent.deletedOnUse)
            //         {

            //             Destroy(item);
            //         }
                    
            //     // }
            //     // else
            //     // {
            //     //     Debug.Log("");
            //     // }
            }
        }
    }

}

