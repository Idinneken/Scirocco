using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EntityState : SerializedMonoBehaviour
{    
    //features to add potentially: JSON deserialisation, defaulting to default variable value.    

    public string stateType, initialStateName, currentStateName, previousStateName;    
    public Dictionary<string, State> states = new();
    internal State currentState = new(), previousState;
    GenericInvoker invoker = new();
    // private bool initialStateApplied = false;
    
    void Start()
    {                                    
        if (states.ContainsKey(initialStateName))
        {          
            SetState(initialStateName);
        }
        else
        {
            print("initialStateName: '" + initialStateName + "' not found");
        }        
    }
    
    #region Adding/Removing/Applying/Toggling states
    public void AddState(string stateName_, State state_)
    {
        if (!states.ContainsKey(stateName_))
        {
            states.Add(stateName_, state_);
        }
    }

    public void RemoveState(string stateName_, State state_)
    {
        if (states.ContainsKey(stateName_))
        {
            states.Remove(stateName_);
        }
    }

    public void SetState(string stateName_)
    {
        if(currentState != states[stateName_])
        {
            const BindingFlags bindingFlags = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.InvokeMethod);                

            if (previousState != null)
            {
                previousState = currentState;
                previousStateName = currentStateName;                                
                if (previousState.outgoingDescription?.values != null)                
                {                    
                    IterateData(previousStateName, previousState.outgoingDescription, bindingFlags);
                }                
            }                                                                  
                            
            currentState = states[stateName_];
            currentStateName = stateName_;

            if(currentState.ingoingDescription?.values != null)
            {                
                IterateData(currentStateName, currentState.ingoingDescription, bindingFlags);              
            }            
            previousState ??= new State();        
        }
        else
        {
            print("it's already that state");
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

    #endregion
    
    #region Component Data Application  
    public void IterateData(string stateName_, ComponentVariableDescription compCollection_, BindingFlags bindingFlags_) 
    {                
        foreach (KeyValuePair<string, ActionCollection> componentType in compCollection_.values) //foreach component in the current collection
        {      
            Component component = GetComponent(componentType.Key);

            if (component != null)
            {            
                foreach (KeyValuePair<string, string> action in compCollection_.values[componentType.Key].values)
                {
                    invoker.DetermineAndApplyAction(component, component, action.Key, action.Value);
                }
            }
            else
            {
                print("'" + componentType.Key + "' not found on the state '" + stateName_ + "' on '" + gameObject.name + "' '" + stateType + "'");                    
            }
        }
    }        
    
    #endregion
}    
