using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Item : Interactable
{
    public ItemData ItemData;
    public TextMeshProUGUI DisplayStacks;
    public int CurrentItemStacks = 1 ;

    private void Update()
    {

        if (CurrentItemStacks > 1)
        {
            DisplayStacks.gameObject.SetActive(true);
            DisplayStacks.text = CurrentItemStacks.ToString();
        }
        else if(CurrentItemStacks == 1)
        {
            DisplayStacks.gameObject.SetActive(false);
        }
        else if (CurrentItemStacks < 1)
        {
            DisplayStacks.gameObject.SetActive(false);
           Debug.Log(" Destroy(this)" + this);
            Destroy(this.gameObject);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        interactEvent.AddListener(PickUp);
    }

    public void UseItemStacks(int num)
    {
        if (CurrentItemStacks >= num)
        {
            CurrentItemStacks -= num;
        }
    }

    public void PickUp(Player player)
    {
        player.SetHoldingItem(this);
    }

    public void Drop(Player player)
    {
        player.SetHoldingItem(null);
    }

}
