using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    //private string _gameSaveKey = "gameSave";
    private string _gameSaveKey = "testSave";
    [SerializeField] private Button _continueButton;

    [SerializeField] private GameObject _startNewGameConfirmWindow;
    private void Update()
    {
        if (PlayerPrefs.HasKey(_gameSaveKey))
        {
            _continueButton?.gameObject.SetActive(true);
        }
        else
        {
            _continueButton?.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        if (!PlayerPrefs.HasKey(_gameSaveKey))
        {
            GoToGameScene();
        }
        else
        {
            _startNewGameConfirmWindow?.gameObject.SetActive(true);
        }
    }

    public void StartNewGame()
    {
        PlayerPrefs.DeleteKey(_gameSaveKey);
        PlayerPrefs.Save();

        GoToGameScene();
    }

    public void ContinueGame()
    {
        GoToGameScene();
    }

    public void GoToGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
