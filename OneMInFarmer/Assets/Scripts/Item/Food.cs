using UnityEngine;
[System.Serializable]
public class Food
{
    [SerializeField] private FoodType _foodType;
    [SerializeField] private float _weightGain;

    public Food(FoodType foodType, float weightGain)
    {
        _foodType = foodType;
        _weightGain = weightGain;
    }

    public FoodType GetFoodType => _foodType;
    public float GetWeightGain => _weightGain;
}
