using Sirenix.OdinInspector;
using System.Collections.Generic;

public class EntityState : SerializedMonoBehaviour //features to add potentially: JSON deserialisation;  
{        
    public string stateType, initialStateName, currentStateName, previousStateName;        
    public Dictionary<string, State> states;
    public State currentState, previousState;
    private Invoker invoker = new();    

    
    void Start()
    {                           
        // foreach(KeyValuePair<string, State> pair in states)
        // {
        //     print(pair.Key);
        // }

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
                invoker.ParseStatements(previousState.outgoingStatements);
            }             
                            
            currentState = states[stateName_];                        
            if (currentState != null)
            {                
                invoker.ParseStatements(currentState.ingoingStatements);
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
