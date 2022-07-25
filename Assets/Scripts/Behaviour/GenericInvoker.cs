using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

public class GenericInvoker
{
    const BindingFlags bindingFlags = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.InvokeMethod);

    public void DetermineAndApplyAction(object objectBeingAltered_, object objectBeingTakenFrom_, string variableOrMethodName_, string parameters_)
    {               
        if (objectBeingAltered_.GetType().HasMethod_(variableOrMethodName_, bindingFlags)) //"if the object that's being changed has a method called *whatever*"
        {
            InvokeComponentMethod(objectBeingAltered_, variableOrMethodName_, parameters_);
        }                
        else if (objectBeingAltered_.GetType().GetField(parameters_) != null) //"if the object that's being changed has a field with the name of the parameter being set"
        {
            EquateComponentVariable(objectBeingAltered_, objectBeingAltered_, variableOrMethodName_, parameters_);
        }
        else if (objectBeingAltered_.GetType().GetField(variableOrMethodName_) != null) //"if the object that's being changed has a field with the name of the field being changed"
        {            
            SetComponentVariable(objectBeingAltered_, variableOrMethodName_, parameters_);
        }
        else if (objectBeingTakenFrom_.GetType().GetField(parameters_) != null) //"if the object that's being taken from has a field with the name of the parameter being set"
        {
            EquateOtherComponentVariable(objectBeingAltered_, objectBeingTakenFrom_, variableOrMethodName_, parameters_);
        }
        
    }    

    public void InvokeComponentMethod(object objectBeingAltered_, string methodName_, string parameters_)
    {                              
        List<object> parameters = new();
        List<Type> parameterTypes = new();                

        if (!string.IsNullOrWhiteSpace(parameters_))
        {
            parameters = JsonConvert.DeserializeObject<List<object>>(parameters_);            
            foreach(object arg in parameters)
            {
                parameterTypes.Add(arg.GetType());
            }
        }
        else
        {
            parameters = null;
        }
           
        objectBeingAltered_.GetType().GetMethod(methodName_, bindingFlags, null, parameterTypes.ToArray(), null)
        .Invoke(objectBeingAltered_, bindingFlags, null, parameters?.ToArray(), null);        
    }

    public void SetComponentVariable(object objectBeingAltered_, string variableName_, string parameters_)
    {        
        FieldInfo infoOfFieldBeingSet = objectBeingAltered_.GetType().GetField(variableName_, bindingFlags); //Get Info for the field that's being set        

        if (infoOfFieldBeingSet == null){
            Debug.Log("'" + variableName_ + "' not found on '" + objectBeingAltered_.ToString() + "' '" + objectBeingAltered_.GetType() + "'");
        } 
                
        infoOfFieldBeingSet.SetValue(objectBeingAltered_, JsonConvert.DeserializeObject(parameters_, infoOfFieldBeingSet.GetValue(objectBeingAltered_).GetType()));        
    }

    public void EquateComponentVariable(object objectBeingAltered_, object objectBeingTakenFrom_, string variableName_, string parameters_)
    {
        FieldInfo infoOfFieldBeingSet = objectBeingAltered_.GetType().GetField(variableName_, bindingFlags); //Get Info for the field that's being set
        FieldInfo infoOfFieldBeingTakenFrom = objectBeingTakenFrom_.GetType().GetField(parameters_, bindingFlags); //Get Info for the field that's being potentially being taken from

        if (infoOfFieldBeingSet == null){
            Debug.Log("'" + variableName_ + "' not found on '" + objectBeingAltered_.ToString() + "' '" + objectBeingAltered_.GetType() + "'");
        }                            

        infoOfFieldBeingSet.SetValue(objectBeingAltered_, infoOfFieldBeingTakenFrom?.GetValue(objectBeingTakenFrom_));    
    }

    public void EquateOtherComponentVariable(object objectBeingAltered_, object objectBeingTakenFrom_, string variableName_, string parameters_)
    {
        FieldInfo infoOfFieldBeingSet = objectBeingAltered_.GetType().GetField(variableName_, bindingFlags); //Get Info for the field that's being set
        FieldInfo infoOfFieldBeingTakenFrom = objectBeingTakenFrom_.GetType().GetField(parameters_, bindingFlags); //Get Info for the field that's being potentially being taken from

        if (infoOfFieldBeingSet == null){
            Debug.Log("'" + variableName_ + "' not found on '" + objectBeingAltered_.ToString() + "' '" + objectBeingAltered_.GetType() + "'");
        }                            

        infoOfFieldBeingSet.SetValue(objectBeingAltered_, infoOfFieldBeingTakenFrom?.GetValue(objectBeingTakenFrom_));    
    }


     
}
