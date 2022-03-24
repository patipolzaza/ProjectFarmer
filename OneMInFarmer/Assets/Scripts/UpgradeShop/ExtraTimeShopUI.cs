using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraTimeShopUI : MonoBehaviour
{
    [SerializeField] private Button[] upgradeButtons = new Button[3];

    private void Start()
    {
        StartCoroutine(InitialSetUp());
    }

    private void OnEnable()
    {
        //SetAllExtraTimeUpgradeButtonsInteractable(true);
    }

    private IEnumerator InitialSetUp()
    {
        yield return new WaitUntil(() => StatusUpgradeManager.Instance);

        Status status = StatusUpgradeManager.Instance.extraTimeStatus;
        int statusLevel;

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            statusLevel = i + 2;
            string buttonText = $"+{status.GetVelueAtLevel(statusLevel)}s";
            string costText = $"Cost: {status.GetUpgradeToTargetLevelCost(statusLevel)}";
            SetUpgradeButtonText(i, buttonText, costText);
        }
    }

    private void SetUpgradeButtonText(int buttonIndex, string textOnButton, string costText)
    {
        Button button = upgradeButtons[buttonIndex];
        Text buttonText = button.transform.GetChild(0).GetComponent<Text>();
        buttonText.text = textOnButton;

        Text belowButtonText = button.transform.GetChild(1).GetComponent<Text>();
        belowButtonText.text = costText;

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
