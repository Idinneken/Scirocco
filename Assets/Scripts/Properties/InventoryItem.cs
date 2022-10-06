using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class InventoryItem : SerializedMonoBehaviour
{    
    internal Inventory ownerInventory;
    public Collider grabBox;    
    public List<Component> associatedComponents;
    // public GameObject associatedObject;

    public List<Action> onPickupActions, onSwitchActions, onUseActions;
    [Space]
    public bool activatesOnPickup; 
    public bool activatesOnSwitch;
    public bool activatesOnUse;
    [Space]
    public bool removedOnPickup; 
    public bool removedOnSwitch; 
    public bool removedOnUse;
    [Space]
    public bool fungible;

    private Invoker invoker;

    void Start()
    {
        GameObject itemObject = new();
        
        if (!associatedComponents.Contains(this))
        {
            associatedComponents.Add(this);
        }

        foreach(Component component in associatedComponents)
        {
            itemObject.CopyPasteComponent_(component);
        }
    }

    public void GrabItem(Inventory ownerInventory_)
    {                
        ownerInventory = ownerInventory_;

        if (activatesOnPickup)
        {
            invoker.ParseActions(onPickupActions);
        }

        if(removedOnPickup)
        {
            Object.Destroy(gameObject, 0f);
        }
        else
        {
            // ownerInventory.gameObject
            // ownerInventory.isActiveAndEnabled()
        }
    }

    public void SwitchToItem()
    {        
    }

    public void UseItem()
    {
        if (activatesOnUse)
        {
            invoker.ParseActions(onUseActions);
        }

        if (removedOnUse)
        {
            Object.Destroy(this, 0f);
        }
    }


    Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields(); 
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }

    
    
}
