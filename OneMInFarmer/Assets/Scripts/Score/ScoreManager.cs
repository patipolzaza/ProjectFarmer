using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int currentScore { get; private set; } = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
    }

    public void ResetScore()
    {
        currentScore = 0;
    }
}
