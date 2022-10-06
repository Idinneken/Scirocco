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

    private void Update()
    {        
        if (Input.GetKeyDown(grabKeybind) && !itemsInRange.IsEmpty())
        {
            print("here");
            GrabItem(transform.GetClosestGameObject(itemsInRange).GetComponent<InventoryItem>());
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

    private void GrabItem(InventoryItem item)
    {
        item.owner = gameObject;
        item.PickUp(inventory);

        if (!item.deletedOnPickup)
        {
            inventory.AddItem(item);
        }
    }

}
