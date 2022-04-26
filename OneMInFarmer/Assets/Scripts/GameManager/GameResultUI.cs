using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResultUI : WindowUIBase
{
    [SerializeField] private Text _dayPlayedText;
    [SerializeField] private Text _totalScoresText;
    [SerializeField] private Button _backToTitleButton;

    private bool _canSkip = false;

    private Coroutine _showResultCoroutine;
    private Coroutine _slideNumberCoroutine;

    protected override void Update()
    {
        base.Update();

        if (_canSkip && _showResultCoroutine != null && Input.anyKeyDown)
        {
            ShowSkippedGameResult();
        }

        _canSkip = true;
    }

    public void SetupUI()
    {
        SetDayPlayedText("0");
        SetTotalScoreText("0");
        HideBackToTitleButton();
    }


    public void SetDayPlayedText(string newText)
    {
        _dayPlayedText.text = newText;
    }

    public void SetTotalScoreText(string newText)
    {
        _totalScoresText.text = newText;
    }

    public void ShowBackToTitleButton()
    {
        _backToTitleButton.gameObject.SetActive(true);
    }
    public void HideBackToTitleButton()
    {
        _backToTitleButton.gameObject.SetActive(false);
    }

    public void ShowGameResult()
    {
        ShowWindow();
        SetupUI();
        _showResultCoroutine = StartCoroutine(ShowGameResultProcess());
    }

    private IEnumerator ShowGameResultProcess()
    {
        int dayPlayed = GameManager.Instance.currentDay;
        _slideNumberCoroutine = StartCoroutine(SlideDayPlayedToTarget(dayPlayed));
        yield return new WaitUntil(() => _slideNumberCoroutine == null);
        int score = ScoreManager.Instance.GetScore;
        _slideNumberCoroutine = StartCoroutine(SlideTotalScoreToTarget(score));
        yield return new WaitUntil(() => _slideNumberCoroutine == null);
        ShowBackToTitleButton();

        _showResultCoroutine = null;
    }

    private IEnumerator SlideTotalScoreToTarget(int target)
    {
        float currentValue = 0;
        while (Mathf.RoundToInt(currentValue) != target)
        {
            currentValue = Mathf.Lerp(currentValue, target, 0.015f);
            string currentValueText = Mathf.RoundToInt(currentValue).ToString();
            SetTotalScoreText(currentValueText);
            yield return new WaitForSeconds(0.01f);
        }

        _slideNumberCoroutine = null;
    }

    private IEnumerator SlideDayPlayedToTarget(int target)
    {
        float currentValue = 0;
        while (Mathf.RoundToInt(currentValue) != target)
        {
            currentValue = Mathf.Lerp(currentValue, target, 0.2f);
            string currentValueText = Mathf.RoundToInt(currentValue).ToString();
            SetDayPlayedText(currentValueText);
            yield return new WaitForSeconds(0.1f);
        }
        _slideNumberCoroutine = null;
    }

    public void ShowSkippedGameResult()
    {
        StopAllCoroutines();

        int dayPlayed = GameManager.Instance.currentDay;
        int score = ScoreManager.Instance.GetScore;
        SetDayPlayedText(dayPlayed.ToString());
        SetTotalScoreText(score.ToString());
        ShowBackToTitleButton();
    }
}
