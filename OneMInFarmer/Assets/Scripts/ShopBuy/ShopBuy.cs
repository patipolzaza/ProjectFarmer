using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopBuy : Interactable
{
    public IBuyable productInStock { get; private set; }
    public UnityEvent OnProductRestocked;
    public UnityEvent OnProductSold;

    [SerializeField] private List<PickableObject> possibleProducts = new List<PickableObject>();

    private void OnValidate()
    {
        if (possibleProducts.Count > 0)
        {
            int loopCount = possibleProducts.Count;
            for (int i = 0; i < loopCount;)
            {
                PickableObject product = possibleProducts[i];

                if (product is IBuyable)
                {
                    i++;
                }
                else
                {
                    possibleProducts.RemoveAt(i);
                    loopCount--;
                    i = 0;
                }
            }
        }
    }

    private void OnEnable()
    {
        interactEvent.AddListener(BuyProduct);
    }

    private void OnDisable()
    {
        interactEvent.RemoveListener(BuyProduct);
    }

    protected override void Start()
    {
        base.Start();

        Restock();
    }

    public void BuyProduct(Player player)
    {
        if (productInStock.Buy(player))
        {
            PickableObject product = Instantiate(productInStock.GetObject().GetComponent<PickableObject>());
            product.gameObject.SetActive(true);
            player.PickUpItem(product);

            OnProductSold?.Invoke();
        }
    }

    public void Restock()
    {
        if (productInStock != null)
        {
            Destroy(productInStock.GetObject());
        }

        int randomedIndex = Random.Range(0, possibleProducts.Count);
        PickableObject product = possibleProducts[randomedIndex];
        product = Instantiate(product);
        product.gameObject.SetActive(false);

        productInStock = product as IBuyable;

        OnProductRestocked?.Invoke();
    }
}
