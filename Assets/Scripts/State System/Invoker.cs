using System;
using Extensions;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;

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
        DetermineActionAndApplyStatement(statement_);                        
    }   

    public void DetermineActionAndApplyStatement(Statement statement_)
    {
        if(statement_.targetComponent.GetType().HasMember_(statement_.targetMemberName, bindingFlags))
        {                        
            if(!statement_.statementInvokesAMethod) //If the statement doesn't invoke a method
            {
                if (!statement_.inputValueIsSourcedFromAnExistingComponentField) //If the input value is typed in via the inspector
                {
                    SetComponentField(statement_.targetComponent, statement_.targetMemberName, statement_.inputValue);
                }
                else //If the input value is a field from another component
                {
                    EquateComponentField(statement_.targetComponent, statement_.inputComponent, statement_.targetMemberName, statement_.inputMemberName);
                }
            } 
            else //If the statement invokes a method
            {
                if (!statement_.inputValueIsSourcedFromAnExistingComponentField) //If the input value is typed in via the inspector
                {                    
                    InvokeComponentMethod(statement_.targetComponent, statement_.targetMemberName, statement_.inputValue);    
                    
                }
                else //If the input value is a field from another component
                {                    
                    InvokeComponentMethod(statement_.targetComponent, statement_.inputComponent, statement_.targetMemberName, statement_.inputMemberName);
                }
            }

        }
        else
        {
            Debug.Log(statement_.targetComponent.name + "doesn't have the member: '" + statement_.targetMemberName + "'");
        }        
    } 

    public void InvokeComponentMethod(Component componentBeingAltered_, Component componentBeingTakenFrom_, string invokedMethodName_, string fieldTakenFrom_)
    {
        List<object> values = new();                        

        foreach(string parameter in JsonConvert.DeserializeObject<List<string>>(fieldTakenFrom_)) 
        {
            values.Add(componentBeingTakenFrom_.GetType().GetField(parameter, bindingFlags).GetValue(componentBeingTakenFrom_));
        }

        componentBeingAltered_.GetType().GetMethod(invokedMethodName_, bindingFlags, null, values.GetTypes_().ToArray(), null)
        .Invoke(componentBeingAltered_, bindingFlags, null, values.ToArray(), null);
    }
    
    public void InvokeComponentMethod(Component componentBeingAltered_, string methodName_, string parameter_)
    {                              
        List<object> parameters = new();                          

        if (!string.IsNullOrWhiteSpace(parameter_))
        {
            parameters = JsonConvert.DeserializeObject<List<object>>(parameter_);     

            for (int i = 0; i < parameters.Count; i++)
            {
                if (parameters[i].GetType() == typeof(System.Int64))
                {                    
                    parameters[i] = Convert.ToInt32(parameters[i]);                                         
                }
            }                                  
        }
        else
        {
            parameters = null;                   
        }                      
        
        componentBeingAltered_.GetType().GetMethod(methodName_, bindingFlags, null, parameters.GetTypes_().ToArray(), null)
        .Invoke(componentBeingAltered_, bindingFlags, null, parameters.ToArray(), null);        
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
