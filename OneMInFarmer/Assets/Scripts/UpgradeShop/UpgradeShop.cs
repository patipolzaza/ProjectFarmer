using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeShop : MonoBehaviour
{
    public static UpgradeShop instance;
    private UpgradeShopWindowUI ui;
    [SerializeField] private int baseTimeCostPerSecond = 1;

    public int dayParam;

    private void Awake()
    {
        instance = this;
        ui = FindObjectOfType<UpgradeShopWindowUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Debug.Log(GetPrice(5));
        }
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
        var playerWallet = GameManager.instance.player.wallet;

        if (playerWallet.coin > GetPrice(extraTime))
        {
            GameManager.instance.IncreaseTimeForNextDay(extraTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetPrice(int extraTime)
    {
        int currentPrice = Mathf.CeilToInt(baseTimeCostPerSecond * (dayParam * 0.35f)) * extraTime;
        return currentPrice;
    }
}
