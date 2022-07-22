using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
// using ExtensionMethods;
using UnityEngine;

public class EntityState : SerializedMonoBehaviour
{    
    //features to add potentially: JSON deserialisation, defaulting to default variable value.
    
    // IDEA: FOR DEFAULTING TO DEFAULT VARIABLE VALUE, HAVE A DICTIONARY WHICH CONTAINS THE DEFAULT VALUES FOR THINGS
    // THEN APPLY ALL IN THAT DICTIONARY EXCEPT ONES WHICH ARE OVERRIDEN BY THE NEWLY SET DICTIONARY

    public string stateType, initialStateName, stateName, previousStateName;
    internal Dictionary<string, Dictionary<string, string>> currentState = new();
    public Dictionary<string, Dictionary<string, Dictionary<string, string>>> potentialStates = new();
    
    void Start()
    {                
        if (potentialStates.ContainsKey(initialStateName))
        {
            currentState = potentialStates[initialStateName];
            SetState(initialStateName);
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

    public void ToggleBetweenStates(string firstStateName_, string secondStateName_)
    {
        if (stateName != firstStateName_ && stateName != secondStateName_)
        {
            SetState(firstStateName_);
        }
        else if (stateName == firstStateName_)
        {
            SetState(secondStateName_);
        }
        else if (stateName == secondStateName_)
        {
            SetState(firstStateName_);
        }
        
    }

    public void ToggleState()
    {
        SetState(previousStateName);
    }

    public void SetState(string stateName_)
    {
        previousStateName = stateName;
        stateName = stateName_;  

        const BindingFlags bindingFlags = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.InvokeMethod);                
        if (potentialStates.ContainsKey(stateName_)) //if potentialStates.ContainsKey("walking")
        {            
            currentState = potentialStates[stateName_]; //currentState = potentialStates["walking"]
                                                        
            foreach (KeyValuePair<string, Dictionary<string,string>> stateComponentData in currentState) //foreach component in the current state
            {       
                if(gameObject.GetComponent(stateComponentData.Key))
                {
                    Component component = gameObject.GetComponent(stateComponentData.Key);                    
                         
                    foreach (KeyValuePair<string, string> stateVariableDatom in currentState[stateComponentData.Key]) //foreach variable in the current state at the key "character movement"
                    {                                                            
                        DetermineAndApplyAction(component, stateVariableDatom, bindingFlags);                        
                    }
                }
                else
                {
                    print("'" + stateComponentData.Key + "' not found on the state '" + stateName_ + "' on '" + gameObject.name + "' '" + stateType + "'");                    
                }
            }
        }
        else
        {
            print("'" + stateName_ + "' not found on '" + gameObject.name + "' '" + stateType + "'");
            return;
        }         
        
        
    }

    public void DetermineAndApplyAction(Component component_, KeyValuePair<string, string> stateVariableDatom_, BindingFlags bindingFlags_)
    {       
        // print(component_.GetType().HasMethod_("ToggleCrouch", bindingFlags_));
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
        
        // print("here");

        if (stateVariableDatom_.Value != null)
        {
            parameters = JsonConvert.DeserializeObject<List<object>>(stateVariableDatom_.Value);
            
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
}
