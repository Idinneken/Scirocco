using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class InventoryItem : SerializedMonoBehaviour
{    
    internal Inventory ownerInventory;
    internal GameObject sourceObject, owner;

    public string itemTypeID;
    [Space]
    public Collider collectBox;    
    public List<Component> associatedComponents;    
    public List<Action> onPickupActions, onSwitchActions, onUseActions;
    [Space]
    public bool activatesOnPickup; 
    public bool activatesOnSwitch;
    public bool activatesOnUse;
    [Space]
    public bool deletedOnPickup; 
    public bool deletedOnSwitch; 
    public bool deletedOnUse;

    private Invoker invoker = new();

    void Start()
    {
        sourceObject = gameObject;
    }

    public void PickUp(Inventory ownerInventory_)
    {                
        ownerInventory = ownerInventory_;

        if (activatesOnPickup)
        {
            ActivateItem(onPickupActions);            
        }

        //CollectItem();

        if(deletedOnPickup)
        {
            PurgeItem();
        }       
    }

    public void UseFromInventory()
    {
        if (activatesOnUse)
        {
            ActivateItem(onUseActions);            
        }

        if (deletedOnUse)
        {
            PurgeItem();
        }
    }    

    //public void CollectItem()
    //{
    //    //ownerInventory.AddItem(new InventoryItem().);
    //    Destroy(gameObject, 0f);
    //}

    public void PurgeItem()
    {
        ownerInventory.RemoveItem(this);        
    }

    public void ActivateItem(List<Action> actions_)
    {
        invoker.ParseActions(actions_);
    }

    //public void DropItem()
    //{
    //    ownerInventory.RemoveItem(itemTypeID, this);
    //    Instantiate(sourceObject);
    //    Destroy(this); 
    //}

    public void SwitchedTo()
    {
    }

}
