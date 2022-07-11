using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualBasic;
using UnityEngine;

public class EntityState : SerializedMonoBehaviour
{    
    public string stateType, initialStateName;
    public Dictionary<string, Dictionary<string, string>> currentState = new();
    public Dictionary<string, Dictionary<string, Dictionary<string, string>>> potentialStates = new();
    
    void Start()
    {        
        if (potentialStates.ContainsKey(initialStateName))
        {
            currentState = potentialStates[initialStateName];
            SetState(initialStateName);
        }
        else
        {
            print(initialStateName + " not found");
        }        
    }
    
    public void AddState(string stateName_, Dictionary<string, Dictionary<string, string>> attributes_)
    {
        if (!potentialStates.ContainsKey(stateName_))
        {
            potentialStates.Add(stateName_, attributes_);
        }
    }

    public void SetState(string stateName_)
    {
        const System.Reflection.BindingFlags bindingFlags = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);
        
        if (potentialStates.ContainsKey(stateName_)) //if potentialStates.ContainsKey("walking")
        {            
            currentState = potentialStates[stateName_]; //currentState = potentialStates["walking"]
                                                        
            foreach (KeyValuePair<string, Dictionary<string,string>> stateComponentData in currentState) //foreach component in the current state
            {       
                Component component = gameObject.GetComponent(stateComponentData.Key);                    
                         
                foreach (KeyValuePair<string, string> stateVariableDatom in currentState[stateComponentData.Key]) //foreach variable in the current state at the key "character movement"
                {                                        
                    SetComponentVariable(component, stateVariableDatom, bindingFlags);                    
                }
            }

        }
        else
        {
            print(stateName_ + " not found in the statetype: " + stateType);
            return;
        }           
    }

    public void SetComponentVariable(Component component_, KeyValuePair<string, string> stateVariableDatom_, BindingFlags bindingFlags_)
    {
        // var fieldBeingSet = component_.GetType().GetField(stateVariableDatom_.Key, bindingFlags_);
        // var fieldBeingTakenFrom = component_.GetType().GetField(stateVariableDatom_.Value, bindingFlags_);
        
        object gobject = component_.gameObject;
        FieldInfo infoOfFieldBeingSet = component_.GetType().GetField(stateVariableDatom_.Key, bindingFlags_);        
        FieldInfo infoOfFieldBeingTakenFrom = component_.GetType().GetField(stateVariableDatom_.Value, bindingFlags_);
        
        object valueOfFieldBeingSet = infoOfFieldBeingSet.GetValue(component_); 
        object valueOfFieldBeingTakenFrom = null;

        if (infoOfFieldBeingTakenFrom != null)
        {
            valueOfFieldBeingTakenFrom = infoOfFieldBeingTakenFrom.GetValue(gobject);
        }
                       
        if (valueOfFieldBeingTakenFrom != null /* && valueOfFieldBeingTakenFrom.GetType() == valueOfFieldBeingSet.GetType()*/) //If it's being set to a variable that already exists.
        {
            infoOfFieldBeingSet.SetValue(component_, valueOfFieldBeingTakenFrom);
        }
        else
        {
            infoOfFieldBeingSet.SetValue(component_, JsonConvert.DeserializeObject(stateVariableDatom_.Value, valueOfFieldBeingSet.GetType()));
        }


        // if (fieldBeingTakenFrom != null && fieldBeingTakenFrom.GetType() == fieldBeingSet.GetType()) 
        // {                                                            
        //     print(fieldBeingTakenFrom);
        //     print(fieldBeingTakenFrom.GetValue(null));
        //     // fieldBeingSet.SetValue(component_, fieldBeingTakenFrom.GetType().GetField().GetValue());
        //     fieldBeingSet.SetValue(component_, fieldBeingTakenFrom.GetValue(null));
        // }
        // else
        // {
        //     fieldBeingSet.SetValue(component_, JsonConvert.DeserializeObject(stateVariableDatom_.Value, fieldBeingSet.FieldType));
        // }   
    }

}
