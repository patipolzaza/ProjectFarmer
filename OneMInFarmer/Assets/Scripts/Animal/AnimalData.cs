using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Animal/Animal Data", fileName = "D_Animal")]
public class AnimalData : ScriptableObject
{
    public string animalName = "Animal";

    public Sprite inShopIcon;
    public int sellPricePerKilo = 1;

    public int bonusSellPriceForAdult = 1;
    public int bonusSellPriceForElder = 2;

    public int purchasePrice = 5;

    public int lifespan = 5;

    public int stomachSize = 1;
    public float startWeight = 1;

    public List<FoodType> edibleFoods = new List<FoodType>();
}
