using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalEdibleProduct : Product, IAnimalEdible
{
    public FoodType GetFoodType => FoodType.Plant;

    public float GetWeightGain => ((AnimalEdibleProductData)ItemData).weightGain;

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
}
