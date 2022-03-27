using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraTimeShopUI : MonoBehaviour
{
    [SerializeField] private ExtraTimeUpgradeButton[] upgradeButtons = new ExtraTimeUpgradeButton[3];
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

    public void ChangeUpgradeChosen(int oldIndex)
    {
        upgradeButtons[oldIndex].Reject();
    }

    public void SetExtraTimeUpgradeButtonInteractable(int index, bool isInteractable)
    {
        upgradeButtons[index].interactable = isInteractable;
    }

    public void SetAllExtraTimeUpgradeButtonsInteractable(bool isInteractable)
    {
        foreach (var button in upgradeButtons)
        {
            button.interactable = isInteractable;
        }
    }
}
