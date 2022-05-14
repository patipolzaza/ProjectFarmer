using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaxAnimalStatusSaveData
{
    [SerializeField] private int _statusLevel;

    public int GetStatusLevel => _statusLevel;

    public MaxAnimalStatusSaveData(Status maxAnimalStatusData)
    {
        UpdateSaveData(maxAnimalStatusData);
    }

    public void UpdateSaveData(Status maxAnimalStatus)
    {
        _statusLevel = maxAnimalStatus.currentLevel;

        ObjectDataContainer.UpdateMaxAnimalStatusSaveData(this);
    }
}
