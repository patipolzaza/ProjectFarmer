using UnityEngine;
using TMPro;

public class ProductSellPriceDisplayer : WindowUIBase
{
    public static ProductSellPriceDisplayer Instance { get; private set; }
    [SerializeField] private TMP_Text _sellPriceText;

    protected override void Awake()
    {
        Instance = this;
    }

    private void SetSellPriceText(string newText)
    {
        _sellPriceText.text = newText;
    }

    public void SetUpText(ISellable sellable)
    {
        if (sellable != null)
        {
            SetSellPriceText($"{sellable.GetSellPrice}");
        }
    }

    public override void ShowWindow()
    {
        ISellable sellable = Player.Instance.playerHand.holdingObject as ISellable;
        if (sellable != null)
        {
            SetUpText(sellable);
            base.ShowWindow();
        }
    }
}
