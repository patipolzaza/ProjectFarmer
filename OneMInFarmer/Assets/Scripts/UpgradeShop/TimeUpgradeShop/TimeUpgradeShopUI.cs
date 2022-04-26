using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeUpgradeShopUI : MonoBehaviour
{
    [SerializeField] private DailyUpgradeShopButton[] upgradeButtons = new DailyUpgradeShopButton[3];
    public int upgradeButtonsCount
    {
        get { return upgradeButtons.Length; }
    }

    public void SetUpgradeButtonText(int buttonIndex, string textOnButton, string costText)
    {
        Button button = upgradeButtons[buttonIndex];
        Text buttonText = button.transform.GetChild(0).GetComponent<Text>();
        buttonText.text = textOnButton;

        Text belowButtonText = button.transform.GetChild(1).GetComponent<Text>();
        belowButtonText.text = costText;
    }

    public void AddButtonAction(int index, UnityAction action)
    {
        upgradeButtons[index].onClick.AddListener(action);
    }

    public void RemoveButtonAction(int index, UnityAction action)
    {
        upgradeButtons[index].onClick.RemoveListener(action);
    }

    public void ChangeUpgradeTarget(int oldIndex)
    {
        if (oldIndex < upgradeButtons.Length)
        {
            upgradeButtons[oldIndex].Reject();
        }
    }

    public void SetExtraTimeUpgradeButtonInteractable(int index, bool isInteractable)
    {
        if (upgradeButtons[index].isChosen)
        {
            upgradeButtons[index].interactable = false;
        }
        else
        {
            upgradeButtons[index].interactable = isInteractable;
        }
    }

    public void SetAllExtraTimeUpgradeButtonsInteractable(bool isInteractable)
    {
        int index = 0;
        foreach (var button in upgradeButtons)
        {
            SetExtraTimeUpgradeButtonInteractable(index, isInteractable);
            index++;
        }
    }
}
