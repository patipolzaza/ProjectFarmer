using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductDetailDisplayer : WindowUIBase
{
    public static ProductDetailDisplayer Instance { get; private set; }

    [SerializeField] private TMP_Text _sellPriceValueText;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowUI(Product product)
    {
        SetSellPriceValueText(product.GetSellPrice);

        ShowWindow();
    }

    private void SetSellPriceValueText(int sellPrice)
    {
        _sellPriceValueText.text = sellPrice.ToString();
    }

    /// <summary>
    /// This method is will showed without setup please use ShowUI(Product) to show this ui with seted detail.
    /// </summary>
    public override void ShowWindow()
    {
        base.ShowWindow();
    }
}
