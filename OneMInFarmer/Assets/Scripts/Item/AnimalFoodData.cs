using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Items/AnimalFoodData")]
public class AnimalFoodData : ItemData
{
    public FoodType foodType = FoodType.Meat;
    public float weightGain = 1;
}
public enum FoodType { Meat, Plant }
