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
                // if (previousState.outgoingDescriptions?.values != null)                
                // {                    
                //     IterateData(previousStateName, previousState.outgoingDescriptions, bindingFlags);
                // }                
            }                                                                  
                            
            currentState = states[stateName_];
            currentStateName = stateName_;

            // if(currentState.ingoingDescriptions?.values != null)
            // {                
            //     IterateData(currentStateName, currentState.ingoingDescriptions, bindingFlags);              
            // }            
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
    public void IterateData(State state_, bool iterateThroughIngoingData_, BindingFlags bindingFlags_) 
    {            
        Dictionary<string, ComponentDescription> descriptions;

        if (iterateThroughIngoingData_)
        {
            descriptions = state_.ingoingDescriptions;
        }
        else
        {
            descriptions = state_.outgoingDescriptions;
        }

        foreach (KeyValuePair<string, ComponentDescription> nameCompDescPair in descriptions)
        {
            Component component = GetComponent(nameCompDescPair.Key);
            
            if (component)
            {
                foreach (MemberDescription memberDesc in nameCompDescPair.Value.memberDescriptions) //foreach component in the current collection
                {                    
                    invoker.DetermineAndApplyAction(component, component, memberDesc.memberName, memberDesc.memberValue);
                
                    // foreach (KeyValuePair<string, string> action in compCollection_.values[componentType.Key].values)
                    // {
                    //     invoker.DetermineAndApplyAction(component, component, action.Key, action.Value);
                    // }
                }
                // else
                // {
                //     print("'" + componentType.Key + "' not found on the state '" + stateName_ + "' on '" + gameObject.name + "' '" + stateType + "'");                    
                // }            
            }

            
        }

        
    }        
    
    #endregion
}    
