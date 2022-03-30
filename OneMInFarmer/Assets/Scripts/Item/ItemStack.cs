using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemStack : Item
{
    [SerializeField] private TextMeshProUGUI DisplayStacks;
    private int CurrentItemStacks = 1;

    private void Update()
    {

        if (CurrentItemStacks > 1)
        {
            DisplayStacks.gameObject.SetActive(true);
            DisplayStacks.text = CurrentItemStacks.ToString();
        }
        else
        {
            DisplayStacks.gameObject.SetActive(false);
        }
    }
    public void SetCurrentStacks(int num)
    {
        CurrentItemStacks = num;
    }
    public int GetCurrentStacks
    {
        get
        {
            return CurrentItemStacks;
        }
    }

    public void UseItemStacks(int num)
    {
        if (CurrentItemStacks >= num)
        {
            CurrentItemStacks -= num;
        }
    }
}
