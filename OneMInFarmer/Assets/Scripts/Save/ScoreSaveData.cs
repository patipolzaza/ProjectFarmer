using UnityEngine;
[System.Serializable]
public class ScoreSaveData
{
    [SerializeField] private int _score;

    public int GetScore;

    public ScoreSaveData(int score)
    {
        UpdateData(score);
    }

    public void UpdateData(int currentScore)
    {
        _score = currentScore;

        ObjectDataContainer.UpdateScoreSaveData(this);
    }
}
