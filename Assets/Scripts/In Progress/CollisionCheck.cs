using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

public class CollisionCheck : SerializedMonoBehaviour
{
    public List<LayerMask> collidableLayers = new();        
    public string componentBeingAltered;                              
    // public Dictionary<string, ComponentDescription> data = new();            
    [Tooltip("Base available parameters for a collision: articulationBody, body, collider, contactCount, contacts, gameObject, impulse, relativeVelocity, rigidBody, transform")]        

    public bool invokeOnEnter = true, invokeOnExit = false;  
    private List<object> fieldValues = new();
    private Invoker invoker = new();    
    private const BindingFlags bindingFlags = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.InvokeMethod);

    // void OnTriggerEnter(Collider collider_)
    // {        
    //     if (collidableLayers.Contains(collider_.gameObject.layer) && invokeOnEnter)
    //     {                                    
    //         invoker.ApplyComponentDescriptions(data, collider_);        
    //     }                
    // }

    // void OnTriggerExit(Collider collider_)
    // {        
    //     if (collidableLayers.Contains(collider_.gameObject.layer) && invokeOnExit)
    //     {
    //         GetFieldsAndApplyAction(collider_);
    //     }
    // }
}