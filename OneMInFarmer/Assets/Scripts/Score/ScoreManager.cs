using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{
    public int currentScore { get; private set; } = 0;

    public void AddScore(int amount)
    {
        currentScore += amount;
    }
}
