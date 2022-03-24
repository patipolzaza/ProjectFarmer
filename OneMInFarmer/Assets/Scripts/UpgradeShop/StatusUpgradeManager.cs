using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUpgradeManager : MonoBehaviour
{
    public static StatusUpgradeManager Instance { get; private set; }
    //MoveSpeed
    public Status extraMoveSpeedStatus { get; private set; }
    [SerializeField] private StatusData extraMoveSpeedData;

    //ExtraTime
    public Status extraTimeStatus { get; private set; }
    [SerializeField] private StatusData extraTimeData;

    private Stack<ICommand> commandHistory = new Stack<ICommand>();

    public bool isReadied { get; private set; } = false;

    private void Awake()
    {
        StartCoroutine(InitialSetUp());
    }

    private IEnumerator InitialSetUp()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        extraMoveSpeedStatus = new Status(extraMoveSpeedData.name, extraMoveSpeedData);
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
        extraMoveSpeedStatus.ResetLevel();
    }
}
