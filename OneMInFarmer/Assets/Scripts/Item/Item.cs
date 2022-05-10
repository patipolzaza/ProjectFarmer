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
    [SerializeField] private TextMeshProUGUI currentStackDisplayer;

    [SerializeField] protected ItemData ItemData;
    //public int currentStack { get; protected set; }
    public int currentStack = 1 ;
    public string GetItemId => ItemData.ID;

    public ItemData GetItemData
    {
        get
        {
            return ItemData;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (currentStackDisplayer)
        {
            if (currentStack > 1)
            {
                currentStackDisplayer.gameObject.SetActive(true);
                currentStackDisplayer.text = currentStack.ToString();
            }
            else
            {
                currentStackDisplayer.gameObject.SetActive(false);
            }
        }
    }

    public void addItemStack(Item otherItem)
    {
        currentStack += otherItem.currentStack;
    }
    public int GetCurrentNumber => currentStack;
}
