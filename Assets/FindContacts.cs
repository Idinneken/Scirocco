using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindContacts : MonoBehaviour
{
    public LayerMask layerMask;

    private void OnTriggerStay(Collider other)
    {
        if ((layerMask.value & (1 << other.transform.gameObject.layer)) > 0) {
            
        }
    }

}
