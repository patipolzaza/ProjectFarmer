using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WateringPot : PickableObject, IUsable
{
    [SerializeField] private Slider sliderWaterBar;
    [SerializeField] private ParticleSystem WaterParticle;
    [SerializeField] private Sprite _icon;
    public float valence { get; private set; } = 20;
    public float remaining { get; private set; } = 20;
    public float RefillPerSeconds { get; private set; } = 1;
    public float waterPerUse { get; private set; } = 5;

    public Sprite GetIcon => _icon;

    protected override void Awake()
    {
        base.Awake();

        AddTargetType(typeof(Plot));
        AddTargetType(typeof(Pool));

        //interactEvent.AddListener(PickUp);
    }
    public bool Use(Interactable targetToUse)
    {
        if (targetToUse is Plot)
        {
            if (remaining >= waterPerUse)
            {

                Plot plot = targetToUse as Plot;
               
                WateringOnPlot(plot);
                //return true;
            }
        }
        if (targetToUse is Pool)
        {
            Refill();
        }

        return false;
    }
    public bool WateringOnPlot(Plot plot)
    {
        if (remaining >= waterPerUse)
        {
            SoundEffectsController.Instance.PlaySoundEffect("Watering");
            playWaterParticle(plot);
            remaining -= waterPerUse;
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

    public void AddTargetType(Type targetType)
    {
        ItemUseMatcher.AddUseItemPair(GetType(), targetType);
    }

    public void playWaterParticle(Plot PlotTarget)
    {
        Vector3 pos = PlotTarget.gameObject.transform.position;
        pos.y += 0.6f;
        WaterParticle.gameObject.transform.position = pos;
        WaterParticle.Play();
    }
}
