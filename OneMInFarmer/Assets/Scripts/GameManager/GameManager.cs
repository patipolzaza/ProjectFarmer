using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private string _gameSaveKey = "gameSave";
    public int currentDay { get; private set; } = 1;
    public int defaultTimePerDay { get; private set; } = 5;

    public Player player { get; private set; }

    [SerializeField] private GameObject _quitGameConfirmWindow;

    public UnityEvent OnSaveLoaded;
    public bool isSaveLoaded { get; private set; }
    public bool isHaveMoreProgress { get; private set; }

    public UnityEvent OnDayStarted;
    public UnityEvent OnDayEnded;

    public UnityEvent OnGameEnded;
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        StartCoroutine(StartGameFirstDay());
    }

    private IEnumerator StartGameFirstDay()
    {
        yield return new WaitUntil(() => isCompletedAllSetup());
        FindObjectOfType<SoundEffectsController>().PlaySoundEffect("BGM");
        if (PlayerPrefs.HasKey(_gameSaveKey))
        {
            LoadGameProgress();
            isSaveLoaded = true;
            isHaveMoreProgress = false;
            OnSaveLoaded?.Invoke();
        }
        else
        {
            TuTorialManager.Instance.ShowTutorial();
            yield return new WaitUntil(() => Input.GetButtonDown("ActionA") && !TuTorialManager.Instance._isInProcess);
            TuTorialManager.Instance.CloseWindow();
            StartDay();
        }
    }

    public void StartDay()
    {
        Debug.Log("Start day [" + currentDay + "]");
        StatusUpgradeManager.Instance.ClearUpgradeHistory();
        OnDayStarted?.Invoke();
    }

    public void EndDay()
    {
        OnDayEnded?.Invoke();
        StartCoroutine(EndDayProcess());
    }

    private IEnumerator EndDayProcess()
    {
        EndDayUI endDayUI = EndDayUI.Instance;
        endDayUI.Show();

        yield return new WaitUntil(() => EndDayUI.Instance.isFinishedAnimation);
        yield return new WaitForSeconds(2);

        endDayUI.Hide();

        var dayResultManager = DayResultManager.Instance;
        dayResultManager.ShowDayResult();
    }

    public void ToNextDay()
    {
        currentDay++;

        ShopForSell.Instance.ResetTotalSoldPrice();
        StatusUpgradeManager.Instance.ResetDailyUpgradeStatus();
        DebtManager.Instance.ResetParameters();

        AnimalFarmManager.Instance.GrowUpAnimals();
        PlotManager.Instance.ResetPlotsStatus();
        ShopBuyManager.Instance.RestockShops();
        player.wallet.UpdateSaveDataOnContainer();

        isHaveMoreProgress = true;
        SaveGameProgress();

        UpgradeShop.Instance.OpenWindow();
    }

    public void GameOver()
    {
        Debug.Log("Game is O V E R.");
        OnGameEnded?.Invoke();
        ObjectDataContainer.ClearAllSaveData(_gameSaveKey);
    }

    public void SaveGameProgress()
    {
        if (isHaveMoreProgress)
        {
            Debug.Log("Game saved.");
            ObjectDataContainer.SaveDatas(_gameSaveKey, currentDay);
        }
    }

    public void LoadGameProgress()
    {
        Debug.Log("Game loaded.");
        int dayPlayed = ObjectDataContainer.LoadDatas(_gameSaveKey);
        Debug.Log("Load data " + dayPlayed + " days played.");
        currentDay = dayPlayed;
    }

    private void SetTimeScale(float value)
    {
        Time.timeScale = value;
    }

    private bool isCompletedAllSetup()
    {
        if (!DayResultManager.Instance || !DayResultManager.Instance.isReadied)
        {
            Debug.Log("DayResultManager");
            return false;
        }
        if (!UpgradeShop.Instance || !UpgradeShop.Instance.isReadied)
        {
            Debug.Log("UpgradeShop");
            return false;

        }
        if (!LightingController.Instance)
        {
            Debug.Log("LightingController");
            return false;

        }
        if (!Timer.Instance)
        {
            Debug.Log("Timer");
            return false;

        }
        if (!TuTorialManager.Instance)
        {
            Debug.Log("TuTorialManager");
            return false;

        }
        if (!StatusUpgradeManager.Instance || !StatusUpgradeManager.Instance.isReadied)
        {
            Debug.Log("StatusUpgradeManager");
            return false;

        }
        if (!Player.Instance)
        {
            Debug.Log("Player");
            return false;

        }
        return true;
    }

    public void QuitGame()
    {
        if (isSaveLoaded && !isHaveMoreProgress)
        {
            ObjectDataContainer.ClearAllSaveData(_gameSaveKey);
        }

        GoToMainMenuScene();
    }

    private void GoToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        Debug.Log("Pause Game.");
        SetTimeScale(0);
    }

    public void UnpauseGame()
    {
        Debug.Log("Unpause Game.");
        SetTimeScale(1);
    }
}
