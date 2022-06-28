using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public string stateCategoryName;
    public List<string> inputtedStates = new();
    public Dictionary<int, string> availableStates = new();

    public string initialState;
    public string currentState;

    // Start is called before the first frame update
    void Start()
    {
        foreach (string state_ in inputtedStates)
        {
            AddState(state_);
        }
        currentState = initialState;
    }

    public void AddState(string state_)
    {
        if (!availableStates.ContainsValue(state_))
        {
            availableStates.Add(availableStates.Keys.Count - 1, state_);
        }
        else
        {
            print(state_ + " is a duplicate state");
        }
    }

    public void ChangeCurrentState(string state_)
    {
        if (availableStates.ContainsValue(state_))
        {
            currentState = state_;
        }
        else
        {
            print(state_ + " not found on " + gameObject);
        }
    }

    public void ChangeCurrentState(int stateIndex_)
    {
        if (availableStates.ContainsKey(stateIndex_))
        {
            currentState = availableStates[stateIndex_];
        }
        else
        {
            print("no state was found at index " + stateIndex_ + " on " + gameObject);
        }
    }
}
