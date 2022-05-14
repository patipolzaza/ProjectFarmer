using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayScore")]
public class PlayScoreSO : ScriptableObject
{
    public int score { get; private set; } = 0;

    public void SetScore(int score)
    {
        this.score = score;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void ResetScore()
    {
        score = 0;
    }
}
