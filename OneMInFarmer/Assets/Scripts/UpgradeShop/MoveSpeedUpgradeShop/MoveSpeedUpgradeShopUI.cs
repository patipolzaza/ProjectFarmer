using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MoveSpeedUpgradeShopUI : MonoBehaviour
{
    [SerializeField] private DailyUpgradeShopButton[] upgradeButtons = new DailyUpgradeShopButton[2];
    public int GetButtonLength
    {
        get
        {
            return upgradeButtons.Length;
        }
    }

    public void SetUpgradeButtonText(int index, string _buttonText, string _belowButtonText)
    {
        var buttonText = upgradeButtons[index].transform.GetChild(0).GetComponent<Text>();
        var belowButtonText = upgradeButtons[index].transform.parent.GetChild(1).GetComponent<Text>();

        buttonText.text = _buttonText;
        belowButtonText.text = _belowButtonText;
    }

    public void AddButtonAction(int index, UnityAction action)
    {
        upgradeButtons[index].onClick.AddListener(action);
    }

    public void RemoveButtonAction(int index, UnityAction action)
    {
        upgradeButtons[index].onClick.RemoveListener(action);
    }

    public void SetButtonInteractable(int buttonIndex, bool isInteractable)
    {
        if (buttonIndex < upgradeButtons.Length)
        {
            upgradeButtons[buttonIndex].interactable = isInteractable;
        }
    }

    public void SetAllButtonsInteractable(bool isInteractable)
    {
        foreach (var button in upgradeButtons)
        {
            button.interactable = isInteractable;
        }
    }

    public void ChangeUpgradeTarget(int oldIndex)
    {
        if (oldIndex < upgradeButtons.Length)
        {
            upgradeButtons[oldIndex].Reject();
        }
    }
}
