using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Sourcer
{
    const BindingFlags bindingFlags = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);
 
    // public GetMember<T>(Component component_, string methodName_, List<Type> parameterTypes_) 
    // {                        
    //     component_.GetType().GetMember()

    //     return component_.GetType().GetMethod(methodName_, bindingFlags, null, parameterTypes_.ToArray(), null);
    // }

    // public T GetMember<T>(Component component_, string methodName_, List<Type> parameterTypes_)
    // {
    //     component_.GetType().GetMember(methodName_, bindingFlags)
    //     component_.GetType().GetMembers
    //     return null;
    // }

    
    
}
