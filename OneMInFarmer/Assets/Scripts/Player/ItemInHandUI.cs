using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInHandUI : WindowUIBase
{
    [SerializeField] private Image _itemIconImage;

    public static ItemInHandUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ShowUI(PickableObject pickableObject)
    {
        Sprite itemSprite = null;
        if (pickableObject is Item)
        {
            itemSprite = ((Item)pickableObject).GetItemData.Icon;
        }
        else if (pickableObject is Animal)
        {
            itemSprite = ((Animal)pickableObject).GetIcon;
        }
        else if (pickableObject is WateringPot)
        {
            itemSprite = ((WateringPot)pickableObject).GetIcon;
        }
        else
        {
            return;
        }

        SetItemIconImage(itemSprite);
        ShowItemIcon();
    }

    public override void HideWindow()
    {
        HideItemIcon();
    }

    private void SetItemIconImage(Sprite newImageSprite)
    {
        _itemIconImage.sprite = newImageSprite;
    }

    private void ShowItemIcon()
    {
        _itemIconImage.gameObject.SetActive(true);
    }

    private void HideItemIcon()
    {
        _itemIconImage.gameObject.SetActive(false);
    }
}
