using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFood : Item
{
    public FoodType GetFoodType => ((AnimalFoodData)ItemData).foodType;

    public float GetWeightGain => ((AnimalFoodData)ItemData).weightGain;

    private void OnValidate()
    {
        if (ItemData != null && ItemData.GetType() != typeof(AnimalFoodData))
        {
            ItemData = null;
        }
    }

    public override bool Use(Interactable targetToUse)
    {
        if (targetToUse is Animal)
        {
            Animal animal = targetToUse as Animal;

            if (currentStack > 1)
            {
                AnimalFood instantiatedFood = Instantiate(this);
                instantiatedFood.currentStack = 1;

                if (animal.TakeFood(instantiatedFood))
                {
                    currentStack--;
                    instantiatedFood.SetParent(animal.transform);
                    instantiatedFood.SetLocalPosition(Vector3.zero, false, false);
                    instantiatedFood.gameObject.SetActive(false);
                }
                else
                {
                    Destroy(instantiatedFood);
                }
            }
            else
            {
                if (animal.TakeFood(this))
                {
                    currentStack--;
                    SetParent(animal.transform);
                    SetLocalPosition(Vector3.zero, false, false);
                    gameObject.SetActive(false);

                    return true;
                }
            }
        }

        return false;
    }
}
