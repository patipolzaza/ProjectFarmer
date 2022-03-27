using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUpgradeManager : MonoBehaviour
{
    public static StatusUpgradeManager Instance { get; private set; }
    //MoveSpeed
    public MoveSpeedStatus moveSpeedStatus { get; private set; }
    [SerializeField] private MoveSpeedStatusData moveSpeedData;

    //ExtraTime
    public Status extraTimeStatus { get; private set; }
    [SerializeField] private StatusData extraTimeData;

    private Stack<ICommand> commandHistory = new Stack<ICommand>();

    public bool isReadied { get; private set; } = false;

    private void Awake()
    {
        StartCoroutine(InitialSetUp());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Debug.Log(moveSpeedStatus.GetValue);
            Debug.Log(moveSpeedStatus.GetValueAtLevel(2));
        }
    }

    private IEnumerator InitialSetUp()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        moveSpeedStatus = new MoveSpeedStatus(moveSpeedData.name, moveSpeedData);
        extraTimeStatus = new Status(extraTimeData.name, extraTimeData);

        isReadied = true;
        yield return null;
    }

    public bool ExecuteCommand(ICommand command)
    {
        if (command.Execute())
        {
            commandHistory.Push(command);
            return true;
        }
        else return false;
    }

    public void Undo()
    {
        ICommand command = commandHistory.Pop();
        command.Undo();
    }
    public void UndoAll()
    {
        int loopCount = commandHistory.Count;
        for (int i = 0; i < loopCount; i++)
        {
            Undo();
        }
    }

    public void ClearUpgradeHistory()
    {
        commandHistory.Clear();
    }

    public void ResetDiaryUpgradeStatus()
    {
        extraTimeStatus.ResetLevel();
        moveSpeedStatus.ResetLevel();
    }
}
