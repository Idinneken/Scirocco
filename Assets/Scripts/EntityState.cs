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
    //features to add potentially: JSON deserialisation, defaulting to default variable value

    public string stateType, initialStateName, currentStateName, previousStateName;
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

    public void SetState(string stateName_)
    {
        const BindingFlags bindingFlags = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.InvokeMethod);
        previousStateName = currentStateName;
        currentStateName = stateName_;

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
                        DetermineandApplyAction(component, stateVariableDatom, bindingFlags);                        
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

    public void DetermineandApplyAction(Component component_, KeyValuePair<string, string> stateVariableDatom_, BindingFlags bindingFlags_)
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
        var parameters = JsonConvert.DeserializeObject<Dictionary<string, object>>(stateVariableDatom_.Value);

        List<Type> parameterTypes = new();

        // print(parameters["message_"]);
        
        foreach(object arg in parameters.Values)
        {
            parameterTypes.Add(arg.GetType());
            // print(arg.GetType());
            

        }
        
        print (parameterTypes[0]);
        //  = arguments.Values.ToList().Get

        // print(component_.GetType().GetMethod("Ping").GetParameters());

        // print(stateVariableDatom_.Key);
        print(parameterTypes[0]);
        print (stateVariableDatom_.Key + " " + " bindingFlags_ null " + parameterTypes.ToArray() + " null");
        // MethodInfo method = component_.GetType().GetMethod(stateVariableDatom_.Key, parameters.Count, bindingFlags_, null, parameterTypes.ToArray(), new List<ParameterModifier>().ToArray());
        MethodInfo method = component_.GetType().GetMethod(stateVariableDatom_.Key, bindingFlags_, null, parameterTypes.ToArray(), null);
        // MethodInfo method = component_.GetType().GetMethod(stateVariableDatom_.Key, 
        // print(method);
        
        // print (method.Name);
        // print (method.Attributes);

        method.Invoke(component_, bindingFlags_, null, parameters.Values.ToArray(), null);

        // var values = JsonConvert.DeserializeObject<List<object>>(stateVariableDatom_.Value);
        
        // foreach (object ob in values)
        // {
        //     print(ob);
        //     print(ob.GetType());
        // }   

        // component_.GetType().InvokeMember(stateVariableDatom_.Key, bindingFlags_, null, component_, argumentValues.Values.ToArray(), null, null, argumentValues.Keys.ToArray());      
        
        // MethodInfo infoOfMethodBeingInvoked = component_.GetType().GetMethod(stateVariableDatom_.Key, bindingFlags_);
        // // ParameterInfo[] attributeTypes = infoOfMethodBeingInvoked.GetParameters();            
        
        // infoOfMethodBeingInvoked.Invoke(component_, bindingFlags_, null, values.ToArray(), null);
        // infoOfMethodBeingInvoked.InvokeMember()
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
