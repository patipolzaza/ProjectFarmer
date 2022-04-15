using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WateringPot : PickableObject, IUsable
{
    [SerializeField] private Slider sliderWaterBar;
    public float valence { get; private set; } = 20;
    public float remaining { get; private set; } = 20;
    public float RefillPerSeconds { get; private set; } = 1;
    private bool isRefill = false;


    protected override void Awake()
    {
        base.Awake();

        //interactEvent.AddListener(PickUp);
    }
    public bool Use(Interactable targetToUse)
    {
        if (targetToUse is Plot)
        {
            Plot plot = targetToUse as Plot;
            if (WateringOnPlot(plot))
            {
                //return true;
            }
        }
        if (targetToUse is Pool)
        {
            Refill();
            //return true;
        }

        return false;
    }
    public bool WateringOnPlot(Plot plot)
    {
        if (plot.seed != null)
            if (remaining > plot.seed.waterNeed)
            {
                remaining -= plot.seed.waterNeed;
                plot.Watering();
                sliderWaterBar.value = remaining;
                return true;
            }
        return false;
    }

    public void Refill()
    {
        if (remaining < 20)
        {
            remaining += RefillPerSeconds * Time.fixedDeltaTime;
            sliderWaterBar.value = remaining;
        }
    }
}
