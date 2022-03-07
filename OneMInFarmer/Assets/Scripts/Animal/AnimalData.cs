using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Animal/Animal Data", fileName = "D_Animal")]
public class AnimalData : ScriptableObject
{
    public int lifespan = 6;

    public List<FoodType> edibleFoods = new List<FoodType>();
}
