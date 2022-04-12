using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Animal/Animal Data", fileName = "D_Animal")]
public class AnimalData : ScriptableObject
{
    public Sprite spriteAnimal;
    public int sellPrice = 12;
    public int purchasePrice = 5;

    public int lifespan = 5;

    public List<FoodType> edibleFoods = new List<FoodType>();
}
