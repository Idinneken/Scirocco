using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public string stateType;
    public List<string> potentialStates = new();

    public string initialState, currentState;    

    void Start()
    {        
        if (potentialStates.Contains(initialState))
        {
            currentState = initialState;
        }
        else
        {
            print(initialState + " not found");
        }
    }

    public void AddState(string state_)
    {
        if (!potentialStates.Contains(state_))
        {
            potentialStates.Add(state_);
        }
        else
        {
            print(state_ + " is a duplicate state");
        }
    }

    public void ChangeCurrentState(string state_)
    {
        if (potentialStates.Contains(state_))
        {
            currentState = state_;
        }
        else
        {
            print(state_ + " not found on " + gameObject);
        }
    }

}
