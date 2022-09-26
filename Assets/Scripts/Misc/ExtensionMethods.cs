using System;
using System.Reflection;
using System.Collections.Generic;
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

        public static bool HasMember_(this Type thingToCheck, string memberName, BindingFlags bindingFlags)
        {
            try
            {                
                return thingToCheck.GetMember(memberName, bindingFlags) != null;
            }
            catch(AmbiguousMatchException)
            {
                // ambiguous means there is more than one result,
                // which means: a method with that name does exist
                return true;
            }
        }

        public static bool HasField_(this Type thingToCheck, string variableName, BindingFlags bindingFlags)
        {
            try
            {                
                return thingToCheck.GetField(variableName, bindingFlags) != null;
            }
            catch(AmbiguousMatchException)
            {
                // ambiguous means there is more than one result,
                // which means: a method with that name does exist
                return true;
            }
        } 

        public static List<Type> GetTypes_(this List<object> objects_)
        {
            List<Type> types = new();
            
            foreach (object ob in objects_)
            {
                types.Add(ob.GetType());
            }

            return types;
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

        public static void ChangePos_(this CharacterController controller_, Vector3 position_)
        {
            controller_.enabled = false;
            controller_.transform.position = position_;
            controller_.enabled = true;
        }

        public static Vector3 Absolute_(this Vector3 vector_)
        {
            return new Vector3(Mathf.Abs(vector_.x), Mathf.Abs(vector_.y), Mathf.Abs(vector_.z) );
        }

        public static float AngleBetweenPoints_(this Vector3 vertexPoint, Vector3 point1, Vector3 point2)
        {
            return Vector3.Angle(point1 - vertexPoint, point2 - vertexPoint);
        }


    }
}
    




