using System.Collections.Generic;
using Sirenix.OdinInspector;

public class Consumable : SerializedMonoBehaviour
{    
    public List<Action> actions;    
    private Invoker invoker = new();
    
    public void ConsumeItem()
    {
        invoker.ParseActions(actions);
    }
}
