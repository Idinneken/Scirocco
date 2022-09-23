using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class NonMonoBehaviourClasses{}

public class State{    
    // public string stateName;    
    public List<Statement> ingoingStatements, outgoingStatements;         
}

public class Statement{                
    [TitleGroup("Target", "What's being targeted?", TitleAlignments.Split)]
    [LabelWidth(192)]public Component targetComponent;    
    [LabelWidth(192)]public string targetMemberName;     
    
    [TitleGroup("Input", "What's being inputted to the target?", TitleAlignments.Split)]    
    [LabelWidth(192)][DisableIf("inputValueIsSourcedFromComponentField")]public string inputValue;
    [LabelWidth(192)][EnableIf("inputValueIsSourcedFromComponentField")]public Component inputComponent;  
    [LabelWidth(192)][EnableIf("inputValueIsSourcedFromComponentField")]public string inputMemberName;  

    // [EnableIf("@inputIsSourcedFromComponent && inputComponentIsKnownOf")]public Component inputComponent;
    // [EnableIf("@inputIsSourcedFromComponent && !inputComponentIsKnownOf")]public string inputComponentName;

    [TitleGroup("Conditions", "What other conditions are there?", TitleAlignments.Split)]  
    [LabelWidth(320)]public bool inputValueIsSourcedFromComponentField;     
    [LabelWidth(192)][MinValue(0)]public float delayAmount;          
    // Maybe add support for "unknown" components. Components that aren't guaranteed to be there at runtime
    // [EnableIf("inputIsSourcedFromComponent")]public bool inputComponentIsKnownOf = true;    
    // public bool targetComponentIsKnownOf = true;     

    [TitleGroup("Notes", "Notes for this statement", TitleAlignments.Split)]
    [SerializeField][TextArea][HideLabel]private string statementNotes;
}
