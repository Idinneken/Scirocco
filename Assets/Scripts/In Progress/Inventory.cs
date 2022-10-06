using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Dictionary<string, InventoryItemBundle> items = new();

    // public void AddItem(string itemCategory_, GameObject item_, string keybind_)
    // {        
    //     if (!items.ContainsKey(itemCategory_))
    //     {            
    //         items.Add(itemCategory_, new List<GameObject>());
    //         items[itemCategory_].Add(item_);
    //     }
    //     else
    //     {
    //         items[itemCategory_].Add(item_);
    //     }
    // }

    // Dictionary<string, InventoryItemBundle> items = new();

    // Dictionary<InventoryItem, List<InventoryItem>> items = new();
    // Dictionary<List<Component>, GameObject> items;    

    // public void AddItem(GameObject item, List<Component> identifyingComponents)
    // {
    //     items.TryAdd(identifyingComponents, item);        
    // }

    // Dictionary<List<Component>, InventoryItemBundle
}

public class InventoryItemBundle
{
    public List<InventoryItem> items;
    
    public Component identifyingComponent;    
    public string identifyingLabel;
    public bool itemsAreDuplicates;
    public string keybind;
    
    public InventoryItemBundle(Component identifyingComponent_, string identifyingLabel_, bool itemsAreDuplicates_, string keybind_)
    {        
        identifyingComponent = identifyingComponent_;
        identifyingLabel = identifyingLabel_;
        itemsAreDuplicates = itemsAreDuplicates_;
        keybind = keybind_;
        

    }           
}

// public class InventoryItem
// {
//     GameObject item;
    
// }
