using System;
using Extensions;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

public class Invoker
{
    const BindingFlags bindingFlags = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.InvokeMethod);    

    public void ParseStatements(List<Statement> statements_)
    {
        if (statements_ != null || statements_ != default(List<Statement>))
            foreach(Statement statement in statements_)
            {                                                                    
                ParseStatement(statement);
            }       
        else        
        return;        
    }
    
    public void ParseStatement(Statement statement_)
    {      
        DetermineAndApplyStatement(statement_);

        // foreach (MemberDescription memberDesc in statement_.memberDescriptions) 
        // {                    
        //     DetermineAndApplyAction(componentBeingAltered_, componentBeingTakenFrom_, memberDesc.memberName, memberDesc.memberValue);                
        // }                          
    }   

    public void DetermineAndApplyStatement(Statement statement_)
    {
        if(statement_.targetComponent.GetType().HasField_(statement_.targetMemberName, bindingFlags)) //If the statement's target component has the specified field (isn't a method)
        {
            if(!statement_.inputValueIsSourcedFromComponentField) //If the input value is inputted via the inspector (not sourced from a component)
            {
                SetComponentField(statement_.targetComponent, statement_.targetMemberName, statement_.inputValue);
            }
            else //If the input value is sourced from a component (not inputted via the inspector)
            {
                EquateComponentField(statement_.targetComponent, statement_.inputComponent, statement_.targetMemberName, statement_.inputMemberName);
            }

        }
        else if(statement_.targetComponent.GetType().HasMethod_(statement_.targetMemberName, bindingFlags)) //If the statement's target component has the specified method (isn't a field)
        {
            if(!statement_.inputValueIsSourcedFromComponentField) //If the input value is inputted via the inspector (not sourced from a component)
            {
                InvokeComponentMethod(statement_.targetComponent, statement_.targetMemberName, statement_.inputValue);
            }
            else //If the input value is sourced from a component (not inputted via the inspector)
            {
                InvokeComponentMethod(statement_.targetComponent, statement_.inputComponent, statement_.targetMemberName, statement_.inputValue);
            }
        }
        
    } 

    public void InvokeComponentMethod(Component componentBeingAltered_, Component componentBeingTakenFrom_, string methodName_, string parameters_)
    {
        List<object> values = new();
        List<string> parameters = JsonConvert.DeserializeObject<List<string>>(parameters_);

        foreach(string str in parameters) 
        {
            values.Add(componentBeingAltered_.GetType().GetField(parameters_, bindingFlags).GetValue(componentBeingTakenFrom_));
        }

        componentBeingAltered_.GetType().GetMethod(methodName_, bindingFlags, null, values.GetTypes_().ToArray(), null)
        .Invoke(componentBeingAltered_, bindingFlags, null, values.ToArray(), null);
    }
    
    public void InvokeComponentMethod(Component componentBeingAltered_, string methodName_, string parameters_)
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
           
        componentBeingAltered_.GetType().GetMethod(methodName_, bindingFlags, null, parameterTypes.ToArray(), null)
        .Invoke(componentBeingAltered_, bindingFlags, null, parameters?.ToArray(), null);        
    }


    public void SetComponentField(Component componentBeingAltered_, string fieldName_, string parameters_)
    {        
        FieldInfo infoOfFieldBeingSet = componentBeingAltered_.GetType().GetField(fieldName_, bindingFlags); //Get Info for the field that's being set        

        if (infoOfFieldBeingSet == null){
            Debug.Log("'" + fieldName_ + "' not found on '" + componentBeingAltered_.ToString() + "' '" + componentBeingAltered_.GetType() + "'");
        } 
                
        infoOfFieldBeingSet.SetValue(componentBeingAltered_, JsonConvert.DeserializeObject(parameters_, infoOfFieldBeingSet.GetValue(componentBeingAltered_).GetType()));        
    }

    public void EquateComponentField(Component componentBeingAltered_, Component componentBeingTakenFrom_, string alteredFieldName_, string takenFromFieldName_)
    {
        FieldInfo infoOfFieldBeingSet = componentBeingAltered_.GetType().GetField(alteredFieldName_, bindingFlags); //Get Info for the field that's being set
        FieldInfo infoOfFieldBeingTakenFrom = componentBeingTakenFrom_.GetType().GetField(takenFromFieldName_, bindingFlags); //Get Info for the field that's being potentially being taken from

        if (infoOfFieldBeingSet == null){
            Debug.Log("'" + alteredFieldName_ + "' not found on '" + componentBeingAltered_.ToString() + "' '" + componentBeingAltered_.GetType() + "'");
        }                            

        infoOfFieldBeingSet.SetValue(componentBeingAltered_, infoOfFieldBeingTakenFrom?.GetValue(componentBeingTakenFrom_));    
    }

    #region Old Maybe unnecessary code

    // public void EquateOtherComponentVariable(object objectBeingAltered_, object objectBeingTakenFrom_, string variableName_, string parameters_)
    // {
    //     FieldInfo infoOfFieldBeingSet = objectBeingAltered_.GetType().GetField(variableName_, bindingFlags); //Get Info for the field that's being set
    //     FieldInfo infoOfFieldBeingTakenFrom = objectBeingTakenFrom_.GetType().GetField(parameters_, bindingFlags); //Get Info for the field that's being potentially being taken from

    //     if (infoOfFieldBeingSet == null){
    //         Debug.Log("'" + variableName_ + "' not found on '" + objectBeingAltered_.ToString() + "' '" + objectBeingAltered_.GetType() + "'");
    //     }                            

    //     infoOfFieldBeingSet.SetValue(objectBeingAltered_, infoOfFieldBeingTakenFrom?.GetValue(objectBeingTakenFrom_));    
    // }     

    // public void ApplyStatement(Statement statement_)
    // {                        
    //     foreach (MemberDescription memberDesc in statement_.memberDescriptions) 
    //     {                    
    //         DetermineAndApplyAction(componentBeingAltered_, componentBeingTakenFrom_, memberDesc.memberName, memberDesc.memberValue);                
    //     }                          
    // }    
    
    // public void DetermineAndApplyAction(Component componentBeingAltered_, Component componentBeingTakenFrom_, string variableOrMethodName_, string parameters_)
    // {                 
    //     if (componentBeingAltered_.GetType().HasMethod_(variableOrMethodName_, bindingFlags)) //"if the object that's being changed has a method called *whatever*"        
    //     {
    //         InvokeComponentMethod(componentBeingAltered_, variableOrMethodName_, parameters_);     
    //     }      
    //     else if (componentBeingAltered_.GetType().GetField(parameters_) != null) //"if the object that's being changed has a field with the name of the parameter being set"
    //     {
    //         EquateComponentVariable(componentBeingAltered_, componentBeingAltered_, variableOrMethodName_, parameters_);
    //     }
    //     else if (componentBeingAltered_.GetType().GetField(variableOrMethodName_) != null) //"if the object that's being changed has a field with the name of the field being changed"
    //     {
    //         SetComponentVariable(componentBeingAltered_, variableOrMethodName_, parameters_);
    //     }
    //     else if (componentBeingTakenFrom_.GetType().GetField(parameters_) != null) //"if the object that's being taken from has a field with the name of the parameter being set"
    //     {
    //         EquateOtherComponentVariable(componentBeingAltered_, componentBeingTakenFrom_, variableOrMethodName_, parameters_);
    //     }
    // }

    #endregion    

}
