using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

public class Status2 : SerializedMonoBehaviour
{
    Component connectedComponent;
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

    public void ChangeCurrentStateTo(string stateName_)
    {
        if (potentialStates.ContainsKey(stateName_))
        {
            currentState = potentialStates[stateName_];
            StackTrace stackTrace = new();
            connectedComponent = GetComponent(stackTrace.GetFrame(1).GetType());
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
            else
            {
                print("guh guh guh guh");
            }
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
