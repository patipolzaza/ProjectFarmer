using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebtManager : MonoBehaviour
{
    public static DebtManager Instance { get; private set; }

    public int dayForNextDebtPayment { get; private set; }
    public int debtPaidCount { get; private set; }
    private float deptMultiplierPerPeriod = 1.3f;
    private int startDebt = 5;

    public int GetDebt
    {
        get
        {
            int currentDay = GameManager.Instance.currentDay;

            float debt = startDebt + (debtPaidCount) * deptMultiplierPerPeriod * (currentDay * 0.15f);

            return Mathf.RoundToInt(debt);
        }
    }

    ///<summary>
    ///Pay the debt and after that will increase paid count that affect to next debt
    ///</summary>
    ///<param name="coinInput">Coin input from player</param>
    ///<returns>Scores that calculate by debt paid</returns>
    public int PayDebt(int coinInput)
    {
        int debt = GetDebt;

        if (coinInput < debt)
        {
            return 0;
        }
        int score = debt * debtPaidCount;

        debtPaidCount++;
        return score;
    }
}
