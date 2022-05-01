using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalEdibleProduct : Product, IAnimalEdible
{
    public FoodType GetFoodType => FoodType.Plant;

    public float GetWeightGain => ((AnimalEdibleProductData)ItemData).weightGain;

    private void OnEnable()
    {
        OnHighlightShowed.AddListener(ShowDetail);
        OnHighlightHided.AddListener(HideDetail);
    }

    private void OnDisable()
    {
        OnHighlightShowed.RemoveListener(ShowDetail);
        OnHighlightHided.RemoveListener(HideDetail);
    }

    public bool Feed(Animal targetToFeed)
    {
        if (targetToFeed.TakeFood(this))
        {
            currentStack--;
            SetParent(targetToFeed.transform);
            SetLocalPosition(Vector3.zero, false, false, false, false);
            gameObject.SetActive(false);

            return true;
        }

        return false;
    }

    private void ShowDetail()
    {
        AnimalFoodDetailDisplayer foodDetailDisplayer = AnimalFoodDetailDisplayer.Instance;
        foodDetailDisplayer.ShowUI(this);
    }

    private void HideDetail()
    {
        AnimalFoodDetailDisplayer foodDetailDisplayer = AnimalFoodDetailDisplayer.Instance;
        foodDetailDisplayer.HideWindow();
    }
}
