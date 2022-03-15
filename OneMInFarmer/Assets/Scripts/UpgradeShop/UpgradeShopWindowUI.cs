using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopWindowUI : WindowUIBase
{
    [Header("Texts")]
    [SerializeField] private Text playerCoinText;
    [SerializeField] private Text currentExtraTimeText;
    [SerializeField] private Text currentCostText;
    [SerializeField] private Text timeForNextDayText;

    [Header("Buttons")]
    [SerializeField] private Button increaseTimeButton;
    [SerializeField] private Button decreaseTimeButton;
    [SerializeField] private Button confirmBuyTimeButton;

    private UpgradeShop upgradeShop;
    protected override void Awake()
    {
        upgradeShop = UpgradeShop.instance;
    }

    protected override void Update()
    {
        base.Update();
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

    public void UpdatePlayerCoinText(int coin)
    {
        playerCoinText.text = $"{coin}";
    }

    public void UpdateCurrentExtraTimeText(int extraTime)
    {
        currentExtraTimeText.text = $"{extraTime}";
    }

    public void UpdateCurrentCostText(int cost)
    {
        currentCostText.text = $"Cost: {cost}";
    }

    public void UpdateTimeForNextDayText(string timeForNextDayString)
    {
        timeForNextDayText.text = timeForNextDayString;
    }

    public void SetTimeUpgradeButtonsInteractable(bool isInteractable)
    {
        confirmBuyTimeButton.interactable = isInteractable;
        SetIncreaseButtonInteractable(isInteractable);
        SetDecreaseButtonInteractable(isInteractable);
    }

    public void SetIncreaseButtonInteractable(bool isInteractable)
    {
        increaseTimeButton.interactable = isInteractable;
    }
    public void SetDecreaseButtonInteractable(bool isInteractable)
    {
        decreaseTimeButton.interactable = isInteractable;
    }
}
