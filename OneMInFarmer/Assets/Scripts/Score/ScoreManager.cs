using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    private ScoreSaveData _saveData;
    private int _score;

    public int GetScore => _score;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (_saveData == null)
        {
            _saveData = new ScoreSaveData(0);
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
        _score = score;

        UpdateScoreSaveDataOnContainer();
    }

    public void AddScore(int amount)
    {
        _score += amount;

        UpdateScoreSaveDataOnContainer();
    }
    private void UpdateScoreSaveDataOnContainer()
    {
        _saveData.UpdateData(_score);
    }

    public void ResetScore()
    {
        _score = 0;
    }
}
