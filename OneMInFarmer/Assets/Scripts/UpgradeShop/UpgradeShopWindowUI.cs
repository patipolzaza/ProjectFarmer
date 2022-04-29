using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopWindowUI : WindowUIBase
{
    [Header("Texts")]
    [SerializeField] private Text playerCoinText;

    [Header("Tabs")]
    [SerializeField] private List<Button> changeTabButtons = new List<Button>();
    [SerializeField] private List<GameObject> tabPanels = new List<GameObject>();

    public override void ShowWindow()
    {
        base.ShowWindow();
    }
    public override void HideWindow()
    {
        base.HideWindow();
    }

    public void UpdatePlayerCoinText(int coin)
    {
        playerCoinText.text = $"{coin}";
    }

    public void SetPanelButtonInteractable(int index, bool isInteractable)
    {
        changeTabButtons[index].interactable = isInteractable;
    }

    public void ChangePanel(int oldIndex, int newIndex)
    {
        SetPanelButtonInteractable(oldIndex, true);
        tabPanels[oldIndex].SetActive(false);

        tabPanels[newIndex].transform.SetAsLastSibling();
        SetPanelButtonInteractable(newIndex, false);
        tabPanels[newIndex].SetActive(true);
    }
}
