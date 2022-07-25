using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EntityState : SerializedMonoBehaviour
{    
    //features to add potentially: JSON deserialisation, defaulting to default variable value.
    
    // IDEA: FOR DEFAULTING TO DEFAULT VARIABLE VALUE, HAVE A DICTIONARY WHICH CONTAINS THE DEFAULT VALUES FOR THINGS
    // THEN APPLY ALL IN THAT DICTIONARY EXCEPT ONES WHICH ARE OVERRIDEN BY THE NEWLY SET DICTIONARY

    public string stateType, initialStateName, currentStateName, previousStateName;    
    public Dictionary<string, State> states = new();
    internal State currentState, previousState;
    // private bool initialStateApplied = false;
    
    void Start()
    {              
        currentState = new();                     
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
                if (previousState.outgoingComponentCollection?.values != null)                
                {
                    // print("Iterating through outgoing data");
                    IterateData(previousStateName, previousState.outgoingComponentCollection, bindingFlags);
                }                
            }                                                                  
                            
            currentState = states[stateName_];
            currentStateName = stateName_;

            if(currentState.ingoingComponentCollection?.values != null)
            {
                // print("Iterating through incoming data");
                IterateData(currentStateName, currentState.ingoingComponentCollection, bindingFlags);              
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
    public void IterateData(string stateName_, ComponentCollection compCollection_, BindingFlags bindingFlags_) 
    {                
        foreach (KeyValuePair<string, ActionCollection> componentType in compCollection_.values) //foreach component in the current collection
        {      
            Component component = GetComponent(componentType.Key);

            if (component != null)
            {            
                foreach (KeyValuePair<string, string> action in compCollection_.values[componentType.Key].values)
                {
                    DetermineActionAndApplyDatom(component, action, bindingFlags_);
                }
            }
            else
            {
                print("'" + componentType.Key + "' not found on the state '" + stateName_ + "' on '" + gameObject.name + "' '" + stateType + "'");                    
            }
        }
    }
    
    public void DetermineActionAndApplyDatom(Component component_, KeyValuePair<string, string> stateVariableDatom_, BindingFlags bindingFlags_)
    {               
        if (component_.GetType().HasMethod_(stateVariableDatom_.Key, bindingFlags_))
        {
            InvokeComponentMethod(component_, stateVariableDatom_, bindingFlags_);
        }
        else if (component_.GetType().GetField(stateVariableDatom_.Value, bindingFlags_) == null)
        {            
            SetComponentVariable(component_, stateVariableDatom_, bindingFlags_);
        }        
        else if (component_.GetType().GetField(stateVariableDatom_.Value, bindingFlags_) != null)
        {
            EquateComponentVariable(component_, stateVariableDatom_, bindingFlags_);
        }
    }    

    public void InvokeComponentMethod(Component component_, KeyValuePair<string, string> stateVariableDatom_, BindingFlags bindingFlags_)
    {                              
        List<object> parameters = new();
        List<Type> parameterTypes = new();        

        if (!string.IsNullOrWhiteSpace(stateVariableDatom_.Value))
        {
            parameters = JsonConvert.DeserializeObject<List<object>>(stateVariableDatom_.Value);
            print(parameters);
            foreach(object arg in parameters)
            {
                parameterTypes.Add(arg.GetType());
            }
        }
        else
        {
            parameters = null;
        }
           
        MethodInfo methodBeingInvoked = component_.GetType().GetMethod(stateVariableDatom_.Key, bindingFlags_, null, parameterTypes.ToArray(), null);
        methodBeingInvoked.Invoke(component_, bindingFlags_, null, parameters?.ToArray(), null);
    }

    public void SetComponentVariable(Component component_, KeyValuePair<string, string> stateVariableDatom_, BindingFlags bindingFlags_)
    {        
        FieldInfo infoOfFieldBeingSet = component_.GetType().GetField(stateVariableDatom_.Key, bindingFlags_) ?? null; //Get Info for the field that's being set        

        if (infoOfFieldBeingSet == null){
            print("'" + stateVariableDatom_.Key + "' not found on '" + gameObject.name + "' '" + component_.GetType() + "'");
        } 
        
        infoOfFieldBeingSet.SetValue(component_, JsonConvert.DeserializeObject(stateVariableDatom_.Value, infoOfFieldBeingSet.GetValue(component_).GetType()));        
    }

    public void EquateComponentVariable(Component component_, KeyValuePair<string, string> stateVariableDatom_, BindingFlags bindingFlags_)
    {
        FieldInfo infoOfFieldBeingSet = component_.GetType().GetField(stateVariableDatom_.Key, bindingFlags_) ?? null; //Get Info for the field that's being set
        FieldInfo infoOfFieldBeingTakenFrom = component_.GetType().GetField(stateVariableDatom_.Value, bindingFlags_) ?? null; //Get Info for the field that's being potentially being taken from

        if (infoOfFieldBeingSet == null){
            print("'" + stateVariableDatom_.Key + "' not found on '" + gameObject.name + "' '" + component_.GetType() + "'");
        }                            

        infoOfFieldBeingSet.SetValue(component_, infoOfFieldBeingTakenFrom?.GetValue(component_));
    
    }
    
    #endregion
}    

#region Structs

public class State{
    public ComponentCollection ingoingComponentCollection, outgoingComponentCollection;         
    void Awake(){
        ingoingComponentCollection = new(); outgoingComponentCollection = new ();
    }    
}

public class ComponentCollection{    
    public Dictionary<string, ActionCollection> values;
    void Awake(){
        values = new();
    }
}

public struct ActionCollection{
    public Dictionary<string, string> values;
    void Awake(){
        values = new();
    }
}

#endregion
