using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatusUpgradeManager : MonoBehaviour
{
    public static StatusUpgradeManager Instance { get; private set; }

    public UnityEvent OnClearUpgradeHistory;
    public UnityEvent OnResetDailyStatus;

    private Stack<ICommand> commandHistory = new Stack<ICommand>();

    public bool isReadied { get; private set; } = false;

    private void Awake()
    {
        InitialSetUp();
    }

    private void InitialSetUp()
    {
        Instance = this;
        isReadied = true;
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

        OnClearUpgradeHistory?.Invoke();
    }

    public void ResetDailyUpgradeStatus()
    {
        Timer.Instance.timeStatus.ResetLevel();
        Player.Instance.moveSpeedStatus.ResetLevel();

        OnResetDailyStatus?.Invoke();
    }
}
