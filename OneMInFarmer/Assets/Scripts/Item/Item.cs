using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Item : PickableObject
{
    [SerializeField] private ItemData ItemData;
    public bool isUseable;
    public bool isSellable;

    public ItemData GetItemData()
    {
        return ItemData;
    }


    public virtual bool Use(Interactable targetToUse)
    {
        return true;
    }



}
