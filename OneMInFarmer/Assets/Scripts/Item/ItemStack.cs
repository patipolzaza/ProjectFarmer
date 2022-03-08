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
        else if (CurrentItemStacks == 1)
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
    public void SetCurrentStacks(int num)
    {
        CurrentItemStacks = num;
    }
    public int GetCurrentStacks()
    {
        return CurrentItemStacks;
    }

    public void UseItemStacks(int num)
    {
        if (CurrentItemStacks >= num)
        {
            CurrentItemStacks -= num;
        }
    }
}
