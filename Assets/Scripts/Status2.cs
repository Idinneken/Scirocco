using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System;
using UnityEngine;

public class Status2 : SerializedMonoBehaviour
{    
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

    public void SetState(string stateName_)
    {
        if (potentialStates.ContainsKey(stateName_)) //if potentialStates.ContainsKey("walking")
        {            
            currentState = potentialStates[stateName_]; //currentState = potentialStates["walking"]
                                                        
            foreach (KeyValuePair<string, Dictionary<string,string>> componentData in currentState) //foreach component in the current state
            {                
                foreach (KeyValuePair<string, string> variableData in currentState[componentData.Key]) //foreach variable in the current state at the key "character movement"
                {
                    Component component = gameObject.GetComponent(componentData.Key);                    
                    var componentField = component.GetType().GetField(variableData.Key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);
                    FieldInfo componentFieldInfo = component.GetType().GetField(variableData.Key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
                    Type componentFieldType = componentField.FieldType;

                    var changedValue = JsonConvert.DeserializeObject(variableData.Value, componentFieldType);
                    
                    print(changedValue.GetType() + ": " + changedValue);

                    //componentFieldInfo.SetValue(componentField, changedValue);

                    //component.GetType()
                    //    .GetField(variableData.Key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty)
                    //    .SetValue(componentVariable, changedVariable);
                    



                }

                #region OLD

                //foreach (KeyValuePair<string, string> variable in currentState[component.Key]) //foreach variable in the current state at the key "character movement"
                //{
                //    string statusVariableName = variable.Key;
                //    //string statusVariableValue = variable.Value;



                //    //FieldInfo statusVariableInfo = statusVariableValue.GetType().GetField(statusVariableName, BindingFlags.NonPublic | BindingFlags.Instance);                                        
                //    FieldInfo componentVariableInfo = componentOfRealObject.GetType().GetField(statusVariableName, BindingFlags.NonPublic | BindingFlags.Instance);


                //    //var thing = componentVariableInfo.GetValue(componentOfRealObject.);
                //    var thing = componentVariableInfo.GetValue(componentOfRealObject);
                //    //Type componentVariableType = ;

                //    print(thing);
                //    //print(componentVariableInfo.FieldType);
                //    //print(componentVariableInfo.FieldHandle);
                //    //print(componentVariableInfo.);

                //    //object componentVariableValue = componentVariableInfo.GetValue(componentVariableType);

                //    //print(componentVariableValue);

                //    //print("statusVariableName: " + statusVariableName);
                //    //print("statusVariableValue: "+ statusVariableValue);                    

                //    //print(statusVariableInfo.Name + ": " + statusVariableInfo.FieldType);
                //    //print(componentVariableInfo.Name + ": " + componentVariableInfo.FieldType);






                //    //componentVariableInfo.SetValue(statusVariableName, statusVariableValue);





                //    //TypeCode valueTypeCode = Type.GetTypeCode(componentOfRealObject.GetType().GetField(variableName).FieldType);
                //    //print("variableName: " + statusVariableName);
                //    //print("variableValue: " + statusVariableValue);
                //    //print("valueTypeCode: " + valueTypeCode);
                //    //var variableInfo = JsonConvert.DeserializeAnonymousType(variableValue, componentOfRealObject.GetType().GetField(variableName));
                //}

                #endregion
            }
        }
        else
        {
            print(stateName_ + " not found in the statetype: " + stateType);
            return;
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
