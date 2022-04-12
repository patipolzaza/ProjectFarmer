using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    [SerializeField] private PlayScoreSO scoreSO;

    public int GetScore => scoreSO.score;
    private void Awake()
    {
        Instance = this;
    }

    public void AddScore(int amount)
    {
        scoreSO.AddScore(amount);
    }

    public void ResetScore()
    {
        scoreSO.ResetScore();
    }
}
