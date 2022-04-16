using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuTorialManager : MonoBehaviour
{
    public static TuTorialManager Instance { get; private set; }
    [SerializeField] private TutorialUI _TutorialUI;
    public bool _isInProcess  { get; private set; }

    private void Awake()
    {
        Instance = this;
        _TutorialUI.SetActiveStartGameText(false);
    }
    private void Start()
    {
        ShowTutorial();
    }

    public void ShowTutorial()
    {
        OpenWindow();

        _isInProcess = true;
        StartCoroutine(TutorialProcess());
    }


    public void OpenWindow()
    {
        _TutorialUI.ShowWindow();
    }
    public void CloseWindow()
    {
        _TutorialUI.HideWindow();
    }

    private IEnumerator TutorialProcess()
    {
        _isInProcess = true;
        yield return new WaitForSeconds(2);
        _TutorialUI.SetActiveStartGameText(true);
        _isInProcess = false;
    }
}
