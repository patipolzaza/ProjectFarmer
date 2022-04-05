using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopForSell : Interactable
{
    public static ShopForSell Instance { get; private set; }
    public bool isFinishSellingProcess { get; private set; } = false;
    public int totalSoldPrice { get; private set; }
    [SerializeField] private TextMeshProUGUI priceTMPro;

    protected override void Awake()
    {
        base.Awake();

        Instance = this;
    }

    public bool PutItemInContainer(IValuable valuable)
    {
        if (valuable != null)
        {
            totalSoldPrice += valuable.Sell();
        }

        return true;
    }

    public void ResetTotalSoldPrice()
    {
        totalSoldPrice = 0;
    }

    public void ShowPrice(int price)
    {
        priceTMPro.text = price.ToString();
    }
    public override void HideObjectHighlight()
    {
        sr.color = defaultColor;
        priceTMPro.text = null;
        priceTMPro.gameObject.SetActive(false);
    }
    public override void ShowObjectHighlight()
    {
        if (isInteractable)
        {
            sr.color = highlightColor;
            priceTMPro.gameObject.SetActive(true);
        }
        else
        {
            sr.color = defaultColor;
            priceTMPro.gameObject.SetActive(false);
        }
    }
}
