using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemUseMatcher
{
    private static Dictionary<Type, List<Type>> itemUseDictionary = new Dictionary<Type, List<Type>>();

    public static bool isMatch(IUsable usingObject, Interactable targetToUse)
    {
        Type usingObjType = usingObject.GetType();
        Type targetType = targetToUse.GetType();

        bool condition1 = itemUseDictionary.ContainsKey(usingObjType);
        bool condition2 = itemUseDictionary[usingObjType] != null && itemUseDictionary[usingObjType].Contains(targetType);

        if (condition1 && condition2)
        {
            return true;
        }

        return false;
    }

    public static void AddUseItemPair(Type usingObjectType, Type targetType)
    {
        if (usingObjectType == null || targetType == null)
        {
            return;
        }

        if (itemUseDictionary.Count > 0 && itemUseDictionary.ContainsKey(usingObjectType))
        {
            if (itemUseDictionary[usingObjectType].Count > 0)
            {
                List<Type> pairTypes = itemUseDictionary[usingObjectType];
                if (!pairTypes.Contains(targetType))
                {
                    pairTypes.Add(targetType);
                }
            }
        }
        else
        {
            List<Type> pairTypes = new List<Type>();
            pairTypes.Add(targetType);
            itemUseDictionary.Add(usingObjectType, pairTypes);
        }
    }
}
