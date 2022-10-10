using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : SerializedMonoBehaviour
{    
    internal Inventory ownerInventory;
    internal GameObject sourceObject, owner;

    public string itemTypeID;
    [Space]
    public Collider collectBox;    
    public List<Component> associatedComponents;    
    [Space]
    public List<Action> onPickupActions; /*onSwitchActions,*/ 
    public List<Action> onUseActions;
    [Space]
    public bool activatesOnPickup; 
    // public bool activatesOnSwitch;
    public bool activatesOnUse;
    [Space]
    public bool deletedOnPickup; 
    // public bool deletedOnSwitch; 
    public bool deletedOnUse;

    // private Invoker invoker = new();

    void Start()
    {
        sourceObject = gameObject;
    }
    
    // public void ActivateItem(List<Action> actions_)
    // {
    //     invoker.ParseActions(actions_);
    // }
}
