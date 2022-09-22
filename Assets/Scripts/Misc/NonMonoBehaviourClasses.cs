using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class NonMonoBehaviourClasses{}

public class State{    
    public string stateName;    
    public List<Statement> incomingStatements, outgoingStatements;         
}

public class Statement{            
    
    [TitleGroup("Target", "What's being targeted?", TitleAlignments.Split)]
    public Component targetComponent;    
    public string targetMemberName;     
    
    [TitleGroup("Input", "What's being inputted to the target?", TitleAlignments.Split)]    
    [DisableIf("inputValueIsSourcedFromComponentVariable")]public string inputValue;
    [EnableIf("inputValueIsSourcedFromComponentVariable")]public Component componentName;  
    [EnableIf("inputValueIsSourcedFromComponentVariable")]public string inputMemberName;  

    // [EnableIf("@inputIsSourcedFromComponent && inputComponentIsKnownOf")]public Component inputComponent;
    // [EnableIf("@inputIsSourcedFromComponent && !inputComponentIsKnownOf")]public string inputComponentName;

    [TitleGroup("Conditions", "What other conditions are there?", TitleAlignments.Split)]  
    public bool inputValueIsSourcedFromComponentVariable;     
    [MinValue(0)]public float delayAmount;          
    // Maybe add support for "unknown" components. Components that aren't guaranteed to be there at runtime
    // [EnableIf("inputIsSourcedFromComponent")]public bool inputComponentIsKnownOf = true;    
    // public bool targetComponentIsKnownOf = true;     

    [TitleGroup("Notes", "Notes for this statement", TitleAlignments.Split)]
    [TextArea][HideLabel]public string statementNotes;
}
