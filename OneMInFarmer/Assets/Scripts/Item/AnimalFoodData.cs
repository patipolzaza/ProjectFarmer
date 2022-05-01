using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Items/ItemDatas/AnimalFoodData")]
public class AnimalFoodData : ItemData
{
    public FoodType foodType = FoodType.Meat;
    public float weightGain = 1;
    public int purchasePrice = 5;
}
public enum FoodType { Meat, Plant }
