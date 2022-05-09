using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    private ScoreSaveData _saveData;
    [SerializeField] private PlayScoreSO scoreSO;

    public int GetScore => scoreSO.score;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (_saveData == null)
        {
            _saveData = new ScoreSaveData(GetScore);
        }

        UpdateScoreSaveDataOnContainer();
    }

    public void LoadSaveData(ScoreSaveData saveData)
    {
        SetScore(saveData.GetScore);

        _saveData = saveData;

        UpdateScoreSaveDataOnContainer();
    }

    private void SetScore(int score)
    {
        scoreSO.SetScore(score);

        UpdateScoreSaveDataOnContainer();
    }

    public void AddScore(int amount)
    {
        scoreSO.AddScore(amount);

        UpdateScoreSaveDataOnContainer();
    }
    private void UpdateScoreSaveDataOnContainer()
    {
        ObjectDataContainer.UpdateScoreSaveData(_saveData);
    }

    public void ResetScore()
    {
        scoreSO.ResetScore();
    }
}
