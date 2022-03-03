using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WateringPot : Item
{
    public Slider slider;
    float valence = 20;
    float remaining = 20;
    float RefillPerSeconds = 4;
    public bool isRefill = false;

    public void WateringOnPlot(Plot plot)
    {
        if (plot.seed != null)
            if (remaining > plot.seed.waterNeed)
            {
                remaining -= plot.seed.waterNeed;
                plot.Watering();
                slider.value = remaining;
            }
    }

    public void Refill()
    {
        if (remaining < 20)
        {
            remaining += RefillPerSeconds * Time.fixedDeltaTime;
        }
    }
    public void Refilling(bool isRe)
    {
        isRefill = isRe;
    }

    private void FixedUpdate()
    {
        slider.value = remaining;
        if (isRefill)
        {
            Refill();

        }
    }
}
