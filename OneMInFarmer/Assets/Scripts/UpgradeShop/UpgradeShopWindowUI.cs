using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopWindowUI : WindowUIBase
{
    [SerializeField] private Text playerCoinText;
    [SerializeField] private Text currentExtraTimeText;
    [SerializeField] private Text currentCostText;

    private int currentExtraTime;
    [SerializeField] private int minExtraTime = 0;
    [SerializeField] private int maxExtraTime = 30;

    [SerializeField] private Button[] upgradeTimeButtons = new Button[3];

    private UpgradeShop upgradeShop;
    protected override void Awake()
    {
        upgradeShop = UpgradeShop.instance;
    }

    protected override void Update()
    {
        base.Update();
        UpdateUITexts();
    }

    public override void ShowWindow()
    {
        base.ShowWindow();
        SetTimeUpgradeButtonsInteractable(true);
    }
    public override void HideWindow()
    {
        base.HideWindow();
    }

    public void BuyExtraTime()
    {
        if (upgradeShop.UpgradeTime(currentExtraTime))
        {
            SetTimeUpgradeButtonsInteractable(false);
        }
        else
        {

        }
    }

    public void IncreaseExtraTime()
    {
        int extraTime = ++currentExtraTime;
        SetExtraTime(extraTime);
    }

    public void DecreaseExtraTime()
    {
        int extraTime = --currentExtraTime;
        SetExtraTime(extraTime);
    }

    public void SetExtraTime(int extraTime)
    {
        currentExtraTime = Mathf.Clamp(extraTime, minExtraTime, maxExtraTime);
    }

    private void UpdateUITexts()
    {
        UpdateCurrentCostText();
        UpdateCurrentExtraTimeText();
        UpdatePlayerCoinText();
    }

    private void UpdatePlayerCoinText()
    {
        playerCoinText.text = GameManager.instance.player.wallet.coin.ToString();
    }

    private void UpdateCurrentExtraTimeText()
    {
        currentExtraTimeText.text = currentExtraTime.ToString();
    }

    private void UpdateCurrentCostText()
    {
        currentCostText.text = upgradeShop.GetPrice(currentExtraTime).ToString();
    }

    private void SetTimeUpgradeButtonsInteractable(bool isInteractable)
    {
        foreach (var button in upgradeTimeButtons)
        {
            button.interactable = isInteractable;
        }
    }
}
