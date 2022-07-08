using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Reflection;
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
                                                        
            foreach (KeyValuePair<string, Dictionary<string,string>> componentData in currentState) //foreach component in the current state
            {       
                Component component = gameObject.GetComponent(componentData.Key);                    
                         
                foreach (KeyValuePair<string, string> variableData in currentState[componentData.Key]) //foreach variable in the current state at the key "character movement"
                {                    
                    var componentField = component.GetType().GetField(variableData.Key, bindingFlags);                                       
                    componentField.SetValue(component, JsonConvert.DeserializeObject(variableData.Value, componentField.FieldType));

                }
            }
        }
        else
        {
            print(stateName_ + " not found in the statetype: " + stateType);
            return;
        }           
    }

}
