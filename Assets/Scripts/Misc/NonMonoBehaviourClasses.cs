using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class NonMonoBehaviourClasses{}

public class State{    
    public string stateName;    
    public List<Statement> statements;         

    public State()
    {
        
    }  

    public void Awake()     
    {

    }
}

public class Statement{    
    [Header("Data")]
    public string targetComponent;    
    public string targetMember;
    [DisableIf("valueIsTransposed")] public string value;    
    [EnableIf("valueIsTransposed")] public string sourcedValue;
    [EnableIf("sourcedFromElsewhere")] public GameObject sourcedFromObject;
    [EnableIf("sourcedFromElsewhere")] public bool sourcedFromComponent;  
    // [Space(4)]

    [Header("Conditions")]    
    [ToggleLeft] public bool valueIsTransposed;
    [ToggleLeft] public bool sourcedFromElsewhere;     

          
}
