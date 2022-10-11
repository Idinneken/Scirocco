using System;
using Extensions;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;

public class Invoker
{
    const BindingFlags bindingFlags = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.InvokeMethod);        

    public void ParseActions(List<Action> statements_)
    {
        if (statements_ != null || statements_ != default(List<Action>))
            foreach(Action statement in statements_)
            {                                                                    
                ParseAction(statement);
            }       
        else        
        return;        
    }
    
    public void ParseAction(Action statement_)
    {      
        DetermineActionAndApplyAction(statement_);                        
    }   

    private void DetermineActionAndApplyAction(Action statement_)
    {        
        if (!statement_.statementInvokesAMethod) //If the statement doesn't invoke a method
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


        #region wip code

        //if (!statement_.useTryGetComponent)
        //{
        //    if(statement_.targetComponent.GetType().HasMember_(statement_.targetMemberName, bindingFlags))
        //    {                        
        //        if(!statement_.statementInvokesAMethod) //If the statement doesn't invoke a method
        //        {
        //            if (!statement_.inputValueIsSourcedFromAnExistingComponentField) //If the input value is typed in via the inspector
        //            {
        //                SetComponentField(statement_.targetComponent, statement_.targetMemberName, statement_.inputValue);
        //            }
        //            else //If the input value is a field from another component
        //            {
        //                EquateComponentField(statement_.targetComponent, statement_.inputComponent, statement_.targetMemberName, statement_.inputMemberName);
        //            }
        //        } 
        //        else //If the statement invokes a method
        //        {
        //            if (!statement_.inputValueIsSourcedFromAnExistingComponentField) //If the input value is typed in via the inspector
        //            {                    
        //                InvokeComponentMethod(statement_.targetComponent, statement_.targetMemberName, statement_.inputValue);    
                        
        //            }
        //            else //If the input value is a field from another component
        //            {                    
        //                InvokeComponentMethod(statement_.targetComponent, statement_.inputComponent, statement_.targetMemberName, statement_.inputMemberName);
        //            }
        //        }

        //    }
        //    else
        //    {
        //        Debug.Log(statement_.targetComponent.name + "doesn't have the member: '" + statement_.targetMemberName + "'");
        //        return;
        //    }        
        //}

        #endregion
    }

    private void InvokeComponentMethod(Component componentBeingAltered_, Component componentBeingTakenFrom_, string methodName_, string fieldOrFieldsBeingTakenFrom_)
    {
        List<object> values = new();        
        foreach(string parameter in JsonConvert.DeserializeObject<List<string>>(fieldOrFieldsBeingTakenFrom_)) 
        {
            FieldInfo valueInfo = componentBeingTakenFrom_.GetType().GetField(parameter, bindingFlags);
            if (valueInfo == null){
                Debug.Log("When trying to source the variable '" + parameter + "' while trying to add it to the list of variables for the Method' " + methodName_ + "', it wasn't found on '" + componentBeingAltered_.ToString() + "'");
                return;
            }
            values.Add(valueInfo.GetValue(componentBeingTakenFrom_));
        }

        MethodInfo infoOfMethodBeingInvoked = componentBeingAltered_.GetType().GetMethod(methodName_, bindingFlags, null, values.GetTypes_().ToArray(), null);

        if (infoOfMethodBeingInvoked == null){
            Debug.Log("When trying to find the Method '" + methodName_ + "', it wasn't found on '" + componentBeingAltered_.ToString() + "'");
            return;
        }

        infoOfMethodBeingInvoked.Invoke(componentBeingAltered_, bindingFlags, null, values.ToArray(), null);
    }
    
    private void InvokeComponentMethod(Component componentBeingAltered_, string methodName_, string parameter_)
    {                              
        List<object> values = new();                          
        if (!string.IsNullOrWhiteSpace(parameter_))
        {
            //Debug.Log("parameter_: " + parameter_);
            values = JsonConvert.DeserializeObject<List<object>>(parameter_);     

            for (int i = 0; i < values.Count; i++)
            {
                //Debug.Log(values[i]);

                if (values[i].GetType() == typeof(long))
                {                    
                    values[i] = Convert.ToInt32(values[i]);                                         
                }
            }                                  
        }

        MethodInfo infoOfMethodBeingInvoked = componentBeingAltered_.GetType().GetMethod(methodName_, bindingFlags, null, values.GetTypes_().ToArray(), null);

        if (infoOfMethodBeingInvoked == null)
        {
            Debug.Log("When trying to find the Method '" + methodName_ + "', it wasn't found on '" + componentBeingAltered_.ToString() + "'");
            return;
        }

        infoOfMethodBeingInvoked.Invoke(componentBeingAltered_, bindingFlags, null, values.ToArray(), null);        
    }

    private void SetComponentField(Component componentBeingAltered_, string fieldName_, string parameters_)
    {        
        FieldInfo infoOfFieldBeingSet = componentBeingAltered_.GetType().GetField(fieldName_, bindingFlags); //Get Info for the field that's being set        

        if (infoOfFieldBeingSet == null){
            Debug.Log("When trying to find the variable '" + fieldName_ + "' while trying to set it to the value of '" + parameters_ + "', it wasn't found on '" + componentBeingAltered_.ToString() + "'");
            return;
        } 
                
        infoOfFieldBeingSet.SetValue(componentBeingAltered_, JsonConvert.DeserializeObject(parameters_, infoOfFieldBeingSet.GetValue(componentBeingAltered_).GetType()));        
    }

    private void EquateComponentField(Component componentBeingAltered_, Component componentBeingTakenFrom_, string alteredFieldName_, string takenFromFieldName_)
    {
        FieldInfo infoOfFieldBeingSet = componentBeingAltered_.GetType().GetField(alteredFieldName_, bindingFlags); //Get Info for the field that's being set
        FieldInfo infoOfFieldBeingTakenFrom = componentBeingTakenFrom_.GetType().GetField(takenFromFieldName_, bindingFlags); //Get Info for the field that's being potentially being taken from

        if (infoOfFieldBeingSet == null){
            Debug.Log("When trying to find the variable '" + alteredFieldName_ + "' while trying to set it to the value of '" + takenFromFieldName_ + "', it wasn't found on '" + componentBeingAltered_.ToString() + "'");
            return;
        }                    
        if (infoOfFieldBeingTakenFrom == null){
            Debug.Log("When trying to source the variable '" + takenFromFieldName_ + "' while trying to equate it to the variable '" + alteredFieldName_ + "', it wasn't found on '" + componentBeingTakenFrom_.ToString() + "'"); 
            return;
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
