using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EntityState : SerializedMonoBehaviour //features to add potentially: JSON deserialisation;  
{        
    public string stateType, initialStateName, currentStateName, previousStateName;    
    public Dictionary<string, State> states = new();
    internal State currentState, previousState;
    Invoker invoker = new();    
    
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
        if(states[stateName_] != currentState) //If the incoming state isn't the current one
        {                       
            if (previousState != null) //If there has been a state before
            {
                previousState = currentState; 
                invoker.ApplyComponentDescriptions(previousState.outgoingDescriptions, null, gameObject);
            }             
                            
            currentState = states[stateName_];                        
            if (currentState != null)
            {
                invoker.ApplyComponentDescriptions(currentState.ingoingDescriptions, null, gameObject);
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
            currentStateName = firstStateName_;
        }
        else if (currentStateName == firstStateName_)
        {
            SetState(secondStateName_);
            currentStateName = secondStateName_;
        }
        else if (currentStateName == secondStateName_)
        {
            SetState(firstStateName_);
            currentStateName = firstStateName_;
        }
        
    }

    public void ToggleState()
    {
        SetState(previousStateName);
    }    
    
}    
