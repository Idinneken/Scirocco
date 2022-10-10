using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

        public static bool IsEmpty<T>(this IList<T> list_)
        {
            return !list_.Any();
        }


        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list_)
        {
            if (list_ == null)
            {
                return true;
            }
            /* If this is a list, use the Count property for efficiency. 
            * The Count property is O(1) while IEnumerable.Count() is O(N). */
            var collection = list_ as ICollection<T>;
            if (collection != null)
            {
                return collection.Count < 1;
            }
            return !list_.Any(); 
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

        public static GameObject GetClosestGameObject(this Transform transform_, List<GameObject> gameObjects_)
        {
            GameObject bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform_.position;
            foreach (GameObject potentialObject in gameObjects_)
            {
                Vector3 directionToTarget = potentialObject.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialObject;
                }
            }

            return bestTarget;
        }

        public static void CopyPasteComponent_(this GameObject destination_, Component component_)
        {
            Type type = component_.GetType();
            Component copy = destination_.AddComponent(type);
            // Copied fields can be restricted with BindingFlags
            FieldInfo[] fields = type.GetFields(); 
            foreach (FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(component_));
            }
            // return copy;
        }

        

    //public static Component CopyComponent_(this GameObject original_, Component destination)
    //{
    //    Type type = original_.GetType();
    //    Component copy = destination.AddComponent(type);
    //    System.Reflection.FieldInfo[] fields = type.GetFields();
    //    foreach (System.Reflection.FieldInfo field in fields)
    //    {
    //        field.SetValue(copy, field.GetValue(original));
    //    }
    //    return copy;
    //}


}
}
    




