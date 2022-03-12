using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeShop : MonoBehaviour
{
    public static UpgradeShop instance;
    private UpgradeShopWindowUI ui;

    private void Awake()
    {
        instance = this;
        ui = FindObjectOfType<UpgradeShopWindowUI>();
    }

    public void OpenWindow()
    {
        ui.ShowWindow();
        GameManager.instance.SetTimeScale(0);
    }

    public void CloseWindow()
    {
        ui.HideWindow();
        GameManager.instance.SetTimeScale(1);
    }

    public bool UpgradeTime(int extraTime)
    {
        //TODO: Check if wallet is enough
        GameManager.instance.IncreaseTimeForNextDay(extraTime);
        return true;
        //Else if not enough return false
    }
}
