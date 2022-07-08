using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System;
using UnityEngine;

public class Status2 : SerializedMonoBehaviour
{    
    public string stateType, initialStateName;
    public Dictionary<string, Dictionary<string, string>> currentState = new();
    public Dictionary<string, Dictionary<string, Dictionary<string, string>>> potentialStates = new();
    
    void Start()
    {        
        if (potentialStates.ContainsKey(initialStateName))
        {
            currentState = potentialStates[initialStateName];            
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

    //public void ChangeCurrentStateTo(string stateName_) ////CHANGING STATE OF OBJECT VS CHANGING STATE OF SPECIFIC COMPONENT ON OBJECT THINKING
    //{
    //    if (potentialStates.ContainsKey(stateName_))
    //    {
    //        currentState = potentialStates[stateName_];
    //        StackTrace stackTrace = new();
    //        connectedComponent = GetComponent(stackTrace.GetFrame(1).GetType());
    //    }
    //    else
    //    {
    //        print(stateName_ + " not found in the statetype: " + stateType);
    //        return;
    //    }

    //    foreach (var item in currentState)
    //    {
    //        if (connectedComponent.GetType().GetProperty(item.Key) != null)
    //        {
    //            Convert.ChangeType(item.Value, item.Value.GetType());
    //            connectedComponent.GetType().GetProperty(item.Key).SetValue(item.Value, 0);
    //        }
    //        else
    //        {
    //            print("guh guh guh guh");
    //        }
    //    }
    //}

}
