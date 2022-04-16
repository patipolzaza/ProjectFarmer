using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuyManager : MonoBehaviour
{
    public static ShopBuyManager Instance;

    [SerializeField] private List<ShopBuy> shopBuys = new List<ShopBuy>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RestockShops();
    }
    public void RestockShops()
    {
        foreach (var shop in shopBuys)
        {
            shop.Restock();
        }
    }

}
