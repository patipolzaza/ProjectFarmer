using UnityEngine;
using UnityEngine.UI;

public class DailyUpgradeShopButton : Button
{
    private Color defaultDisableColor;
    private Color whenChosenColor;
    public bool isChosen { get; private set; }
    private ColorBlock colorsWorkspace;

    protected override void Awake()
    {
        defaultDisableColor = colors.disabledColor;
        whenChosenColor = new Color32(62, 236, 140, 255);

        colorsWorkspace = colors;
    }

    protected override void Start()
    {
        base.Start();

        onClick.AddListener(Choose);
    }

    private void Update()
    {
        if (isChosen)
        {
            colorsWorkspace.disabledColor = whenChosenColor;
        }
        else
        {
            colorsWorkspace.disabledColor = defaultDisableColor;
        }
        colors = colorsWorkspace;
    }

    private void Choose()
    {
        interactable = false;
        isChosen = true;
    }

    public void Reject()
    {
        interactable = true;
        isChosen = false;
    }
}
