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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        extraMoveSpeedStatus = new Status(extraMoveSpeedData.name, extraMoveSpeedData);
        extraTimeStatus = new Status(extraTimeData.name, extraTimeData);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Wallet wallet = Player.Instance.wallet;
            wallet.EarnCoin(extraTimeStatus.GetUpgradeCost);
            ExecuteCommand(new UpgradeStatusCommand(extraTimeStatus));
            Debug.Log(extraTimeStatus.currentLevel);
        }
    }

    private void ExecuteCommand(ICommand command)
    {
        command?.Execute();
        commandHistory.Push(command);
    }

    public void Undo()
    {
        ICommand command = commandHistory.Pop();
        command?.Undo();
    }
    public void UndoAll()
    {
        for (int i = 0; i < commandHistory.Count; i++)
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
