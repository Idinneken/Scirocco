using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using UnityEngine;

public class Status1 : MonoBehaviour
{
    public string stateType, initialStateName;
    Component connectedComponent;
    public Dictionary<string, Dictionary<string, string>> potentialStates = new();
    public Dictionary<string, string> currentState = new();

    List<List<string>> fub = new();

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

    public void AddState(string stateName_, Dictionary<string, string> attributes_)
    {
        if (!potentialStates.ContainsKey(stateName_))
        {
            potentialStates.Add(stateName_, attributes_);
        }
    }

    public void ChangeCurrentState(string stateName_)
    {
        if (potentialStates.ContainsKey(stateName_))
        {
            currentState = potentialStates[stateName_];
        }
        else
        {
            print(stateName_ + " not found in the statetype: " + stateType);
            return;
        }

        foreach(var item in currentState)
        {
            if (connectedComponent.GetType().GetProperty(item.Key) != null)
            {                
                Convert.ChangeType(item.Value, item.Value.GetType());                
                connectedComponent.GetType().GetProperty(item.Key).SetValue(item.Value, 0);
            }
            //else
            //{
            //    print(item.Key + " was not found on ");
            //}
        }                
    }

}
