using System.Collections.Generic;

public class NonMonoBehaviourClasses{}

public class State{
    public ComponentVariableDescription ingoingDescription, outgoingDescription;         
    void Awake(){
        ingoingDescription = new(); outgoingDescription = new ();
    }    
}

public class ComponentVariableDescription{    
    public Dictionary<string, ActionCollection> values;
    void Awake(){
        values = new();
    }
}

public struct ActionCollection{
    public Dictionary<string, string> values;
    void Awake(){
        values = new();
    }
}
