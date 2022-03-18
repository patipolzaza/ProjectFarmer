using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Animal/Animal Data", fileName = "D_Animal")]
public class AnimalData : ScriptableObject
{
    public int sellPrice = 12;
    public int buyPrice = 5;

    public int lifespan = 5;

    public List<FoodType> edibleFoods = new List<FoodType>();
}
