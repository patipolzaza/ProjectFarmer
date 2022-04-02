using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayResultUI : WindowUIBase
{
    [SerializeField] private Text currentDayText;
    [SerializeField] private Text totalSoldPriceText;
    [SerializeField] private Text deptText;
    [SerializeField] private Text netProfitText;

    public void SetDayText(string text)
    {
        currentDayText.text = text;
    }

    public void SetTotalSoldPriceText(string text)
    {
        totalSoldPriceText.text = text;
    }

    public void SetDeptText(string text)
    {
        deptText.text = text;
    }

    public void SetNetProfitText(string text)
    {
        netProfitText.text = text;
    }
}
