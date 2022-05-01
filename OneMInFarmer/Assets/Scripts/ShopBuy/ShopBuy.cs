using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopBuy : Interactable
{
    public IBuyable productInStock { get; private set; }
    public UnityEvent<IBuyable> OnProductRestocked;

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

        OnHighlightShowed.AddListener(ShowProductDetail);
        OnHighlightHided.AddListener(HideProductDetail);
    }

    private void OnDisable()
    {
        interactEvent?.RemoveListener(BuyProduct);

        OnHighlightShowed?.RemoveListener(ShowProductDetail);
        OnHighlightHided?.RemoveListener(HideProductDetail);
    }

    protected override void Start()
    {
        base.Start();

        Restock();
    }

    public void BuyProduct(Player player)
    {
        PickableObject product = Instantiate(productInStock.GetObject().GetComponent<PickableObject>());
        if (((IBuyable)product).Buy(player))
        {
            product.gameObject.SetActive(true);
            player.playerHand.PickUpObject(product);
            SoundEffectsController.Instance.PlaySoundEffect("ClinkingCoin");
        }
        else
        {
            Destroy(product.gameObject);
        }
    }

    public void Restock()
    {
        if (productInStock != null)
        {
            Destroy(productInStock.GetObject());
            productInStock = null;
        }

        if (possibleProducts.Count > 0)
        {
            int randomedIndex = Random.Range(0, possibleProducts.Count);
            PickableObject product = possibleProducts[randomedIndex];
            product = Instantiate(product);
            product.gameObject.SetActive(false);

            productInStock = product as IBuyable;

            OnProductRestocked?.Invoke(productInStock);
        }
    }

    private void ShowProductDetail()
    {
        if (productInStock != null)
        {
            ShopProductDetailDisplayer productDetailDisplayer = ShopProductDetailDisplayer.Instance;
            productDetailDisplayer.SetUpUI(productInStock);
            productDetailDisplayer.ShowWindow();
        }
    }

    private void HideProductDetail()
    {
        ShopProductDetailDisplayer.Instance.HideWindow();
    }
}
