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

    [SerializeField] private ItemData ItemData;
    public int currentStack { get; private set; }

    public bool isUsable
    {
        get
        {
            return ItemData.isUsable;
        }
    }
    public bool isConsumable
    {
        get
        {
            return ItemData.IsConsumable;
        }
    }

    public ItemData GetItemData
    {
        get
        {
            return ItemData;
        }
    }

    private void Update()
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

    public void SetCurrentStackNumber(int newNum)
    {
        currentStack = newNum;
    }

    public int GetCurrentNumber
    {
        get
        {
            return currentStack;
        }
    }

    public virtual bool Use(Interactable targetToUse)
    {
        currentStack--;

        if (currentStack > 0)
        {
            return false;
        }
        else
        {
            Destroy(gameObject);
            return true;
        }
    }
}
