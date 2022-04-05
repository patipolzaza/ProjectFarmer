using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WateringPot : Item
{
    [SerializeField]private Slider slider;
    public float valence { get; private set; } = 20;
    public float remaining { get; private set; } = 20;
    public float RefillPerSeconds { get; private set; } = 1;
    private bool isRefill = false;


    protected override void Awake()
    {
        base.Awake();

        //interactEvent.AddListener(PickUp);
    }

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
