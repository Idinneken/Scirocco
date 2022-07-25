using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    public Component componentBeingAltered;
    public string variableOrMethodName;
    public List<LayerMask> collidableLayers = new();                  
    // public List<string> fieldsToTakeFromCollision = new();
    public bool invokeOnEnter = true, invokeOnExit = false;  

    public Dictionary<string, string> data = new();
            
    [Tooltip("Base available parameters for a collision: articulationBody, body, collider, contactCount, contacts, gameObject, impulse, relativeVelocity, rigidBody, transform")]        

    private List<object> fieldValues = new();
    private GenericInvoker invoker = new();    
    private Sourcer sourcer = new();
    private const BindingFlags bindingFlags = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.InvokeMethod);

    void OnTriggerEnter(Collider collider_)
    {        
        if (collidableLayers.Contains(collider_.gameObject.layer) && invokeOnEnter)
        {                        
            GetFieldsAndApplyAction(collider_);        
        }        
    }

    void OnTriggerExit(Collider collider_)
    {        
        if (collidableLayers.Contains(collider_.gameObject.layer) && invokeOnExit)
        {
            GetFieldsAndApplyAction(collider_);
        }
    }

    // void GetFieldsAndApplyAction(Collider collider_)
    // {
    //     foreach (string field in fieldsToTakeFromCollision)
    //     {                
    //         fieldValues.Add(collider_.GetType().GetField(field, bindingFlags).GetValue(collider_.gameObject));                
    //     }
                
    //     invoker.DetermineAndApplyAction(componentBeingAltered, variableOrMethodName, JsonConvert.SerializeObject(fieldValues));                                
    // }
    void GetFieldsAndApplyAction(Collider collider_)
    {
        foreach (KeyValuePair<string, string> datom in data)
        {                                                      
            // FieldInfo infoOfFieldBeingTakenFrom = collider_.GetType().GetField(variableName_, bindingFlags)
            invoker.DetermineAndApplyAction(componentBeingAltered, collider_, datom.Key, datom.Value);               
        }
                
        
    }
}