using System;
using System.Collections.Generic;

public class NonMonoBehaviourClasses{}

public class State{
    public Dictionary<string, ComponentDescription> ingoingDescriptions, outgoingDescriptions;         
    void Awake(){
        ingoingDescriptions = new(); outgoingDescriptions = new ();
    }    
}

public class ComponentDescription{    
    public List<MemberDescription> memberDescriptions;
    void Awake(){
        memberDescriptions = new();
    }
}

public struct MemberDescription{
    // public Tuple<string, string> members;
    public string memberName, memberValue;

}
