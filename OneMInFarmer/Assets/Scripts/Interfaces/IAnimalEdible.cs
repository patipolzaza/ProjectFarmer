using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimalEdible
{
    public FoodType GetFoodType { get; }
    public float GetWeightGain { get; }
    public Food GetFood { get; }
    /// <summary>
    /// Feed target animal with this consumable.
    /// </summary>
    /// <param name="targetToFeed"></param>
    /// <returns>true if animal ate, false if not.</returns>
    public bool Feed(Animal targetToFeed);
}
