using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalEdibleProduct : Product, IAnimalEdible
{
    public FoodType GetFoodType => FoodType.Plant;

    public float GetWeightGain => ((AnimalEdibleProductData)ItemData).weightGain;

    public Food GetFood => new Food(GetFoodType, GetWeightGain);

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
        if (targetToFeed.TakeFood(GetFood))
        {
            currentStack--;
            if (currentStack <= 0)
            {
                Destroy(gameObject);
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    public override void ShowDetail()
    {
        AnimalFoodDetailDisplayer foodDetailDisplayer = AnimalFoodDetailDisplayer.Instance;
        foodDetailDisplayer.ShowUI(this);
    }

    public override void HideDetail()
    {
        AnimalFoodDetailDisplayer foodDetailDisplayer = AnimalFoodDetailDisplayer.Instance;
        foodDetailDisplayer.HideWindow();
    }
}
