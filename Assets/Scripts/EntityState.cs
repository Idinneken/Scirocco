using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EntityState : SerializedMonoBehaviour
{    
    public string stateType, initialStateName, currentStateName, previousStateName;
    internal Dictionary<string, Dictionary<string, string>> currentState = new();
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
        const BindingFlags bindingFlags = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);
        previousStateName = currentStateName;
        currentStateName = stateName_;

        if (potentialStates.ContainsKey(stateName_)) //if potentialStates.ContainsKey("walking")
        {            
            currentState = potentialStates[stateName_]; //currentState = potentialStates["walking"]
                                                        
            foreach (KeyValuePair<string, Dictionary<string,string>> stateComponentData in currentState) //foreach component in the current state
            {       
                if(gameObject.GetComponent(stateComponentData.Key))
                {
                    Component component = gameObject.GetComponent(stateComponentData.Key);                    
                         
                    foreach (KeyValuePair<string, string> stateVariableDatom in currentState[stateComponentData.Key]) //foreach variable in the current state at the key "character movement"
                    {                                                            
                        SetComponentVariable(component, stateVariableDatom, bindingFlags);                    
                    }
                }
                else
                {
                    print("'" + stateComponentData.Key + "' not found on the state '" + stateName_ + "' on '" + gameObject.name + "' '" + stateType + "'");                    
                }
            }
        }
        else
        {
            print("'" + stateName_ + "' not found on '" + gameObject.name + "' '" + stateType + "'");
            return;
        }           
    }

    public void ToggleBetweenStates(string firstStateName_, string secondStateName_)
    {
        if (currentStateName != firstStateName_ && currentStateName != secondStateName_)
        {
            SetState(firstStateName_);
        }
        else if (currentStateName == firstStateName_)
        {
            SetState(secondStateName_);
        }
        else if (currentStateName == secondStateName_)
        {
            SetState(firstStateName_);
        }
        
    }

    public void ToggleState()
    {
        SetState(previousStateName);
    }

        

    public void SetComponentVariable(Component component_, KeyValuePair<string, string> stateVariableDatom_, BindingFlags bindingFlags_)
    {        
        FieldInfo infoOfFieldBeingSet = component_.GetType().GetField(stateVariableDatom_.Key, bindingFlags_) ?? null; //Get Info for the field that's being set
        FieldInfo infoOfFieldBeingTakenFrom = component_.GetType().GetField(stateVariableDatom_.Value, bindingFlags_) ?? null; //Get Info for the field that's being potentially being taken from

        if (infoOfFieldBeingSet == null)
        {
            print("'" + stateVariableDatom_.Key + "' not found on '" + gameObject.name + "' '" + component_.name + "'");
        } 
        
        var valueOfFieldBeingSet = infoOfFieldBeingSet.GetValue(component_);
        var valueOfFieldBeingTakenFrom = infoOfFieldBeingTakenFrom?.GetValue(component_);        

        if (infoOfFieldBeingTakenFrom != null) //If it's being set to a variable that already exists
        {                        
            infoOfFieldBeingSet.SetValue(component_, valueOfFieldBeingTakenFrom);
        }
        else //If it's not being set to a variable that already exists
        {
            infoOfFieldBeingSet.SetValue(component_, JsonConvert.DeserializeObject(stateVariableDatom_.Value, valueOfFieldBeingSet.GetType()));
        } 
    }
}
