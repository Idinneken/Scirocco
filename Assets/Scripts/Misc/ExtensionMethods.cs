using System;
using System.Reflection;
using UnityEngine;

namespace Extensions
{
    public static class SystemExtensions
    {
        public static bool HasMethod_<T>(this T thingToCheck, string methodName, BindingFlags bindingFlags) where T : Type
        {
            try
            {
                var type = thingToCheck.GetType();
                return type.GetMethod(methodName, bindingFlags) != null;
            }
            catch(AmbiguousMatchException)
            {
                // ambiguous means there is more than one result,
                // which means: a method with that name does exist
                return true;
            }
        } 

        public static bool HasMethod_(this Type thingToCheck, string methodName, BindingFlags bindingFlags)
        {
            try
            {                
                return thingToCheck.GetMethod(methodName, bindingFlags) != null;
            }
            catch(AmbiguousMatchException)
            {
                // ambiguous means there is more than one result,
                // which means: a method with that name does exist
                return true;
            }
        } 
    }

    public static class UnityEngineExtensions
    {
        public static bool IsOnLayer_(this GameObject gameObject_, LayerMask layer_)
        {
            if ((layer_.value & (1 << gameObject_.layer)) > 0)
            {        
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNotOnLayer_(this GameObject gameObject_, LayerMask layer_)
        {
            if ((~layer_.value & (1 << gameObject_.layer)) > 0)
            {        
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ChangePos_(this CharacterController controller_, Vector3 position_)
        {
            controller_.enabled = false;
            controller_.transform.position = position_;
            controller_.enabled = true;
        }

    }
}
    




