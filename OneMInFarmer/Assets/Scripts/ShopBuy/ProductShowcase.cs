using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductShowcase : MonoBehaviour
{
    [SerializeField] private Image _productImage;

    public void UpdateProductShowcase(IBuyable buyable)
    {
        if (buyable != null)
        {
            SetProductImage(buyable.GetIcon);
            Show();
        }
        else
        {
            Hide();
        }
    }


    private void SetProductImage(Sprite productIcon)
    {
        _productImage.sprite = productIcon;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
