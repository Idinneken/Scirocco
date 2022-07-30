using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoCheck : MonoBehaviour
{
    public LayerMask layerMask;

    private void OnTriggerEnter(Collider other) {
        if ((layerMask.value & (1 << other.transform.gameObject.layer)) > 0) {
            Debug.Log("Hit with Layermask");
        }
        else {
            Debug.Log("Not in Layermask");
        }
    }

}
