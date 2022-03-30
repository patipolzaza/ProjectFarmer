using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType { Meat, Plant }

public class AnimalFood : Item
{
    [SerializeField] private FoodType foodType;

    public FoodType GetFoodType
    {
        get
        {
            return foodType;
        }
    }

    public override bool Use(Interactable targetToUse)
    {
        if (targetToUse is Animal)
        {
            Animal animal = targetToUse as Animal;
            if (animal.TakeFood(this))
            {
                return base.Use(targetToUse);
            }
        }

        return false;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }
}
