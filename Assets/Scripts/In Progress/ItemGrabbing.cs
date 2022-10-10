using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System.Runtime.CompilerServices;
using Sirenix.Utilities;

public class ItemGrabbing : MonoBehaviour
{    
    public Collider grabBox;
    public Inventory inventory;
    public string grabKeybind;

    private List<GameObject> itemsInRange = new();
    private Invoker invoker = new();

    private void Update()
    {        
        if (Input.GetKeyDown(grabKeybind) && !itemsInRange.IsEmpty())
        {            
            // itemsInRange.TrimExcess();
            for (int i = 0; i < itemsInRange.Count; i++)
            {
                print(itemsInRange[i]);
            }

            PickUpItem(transform.GetClosestGameObject(itemsInRange));
        }
        else if (Input.GetKeyDown(grabKeybind))
        {
            print("nothing in range");
        }
    }

    private void OnTriggerEnter(Collider other_)
    {
        if (other_.gameObject.GetComponent<InventoryItem>())
        {            
            itemsInRange.Add(other_.gameObject);         
        }
    }    

    private void OnTriggerExit(Collider other_)
    {
        if (other_.gameObject.GetComponent<InventoryItem>())
        {
            itemsInRange.Remove(other_.gameObject);
        }
    }

    private void PickUpItem(GameObject item_)
    {
        itemsInRange.Remove(item_);

        InventoryItem itemComponent = item_.GetComponent<InventoryItem>();
        itemComponent.owner = gameObject;
        itemComponent.ownerInventory = inventory;

        if (itemComponent.activatesOnPickup)
        {
            // itemComponent.ActivateItem(itemComponent?.onPickupActions);
            invoker.ParseActions(itemComponent.onPickupActions);
        }

        if (!itemComponent.deletedOnPickup)
        {                        
            inventory.AddItem(item_);            
        }
        else
        {
            Destroy(item_);
        }
    }

}
