using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightingController : WindowUIBase
{
    public static LightingController Instance { get; private set; }

    [SerializeField] private Image _LightingPanel;
    [SerializeField] private Color _SunriseLight;
    [SerializeField] private Color _DayLight;
    [SerializeField] private Color _SunsetLight;
    [SerializeField] private Color _NightLight;
    private float cycleCurrentTime = 0;
    private float cycleMaxTime = 60;
    private int dayCycle = 0;
    private bool isProcess = false;
    [SerializeField] private bool isLoopCycle = false;
    // Update is called once per frame
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (isLoopCycle)
            UpdateLighting(20);
        if (isProcess)
            UpdateLighting(4);
    }

    public void StartLighting(int SetCycleMaxTime)
    {
        ResetDayLight();
        cycleMaxTime = SetCycleMaxTime;
        isProcess = true;
    }


    private void UpdateLighting(int speed)
    {
        cycleCurrentTime += Time.deltaTime * speed;
        if (cycleCurrentTime >= cycleMaxTime)
        {
            if (isLoopCycle)
            {
                cycleCurrentTime = 0; // back to 0 (restarting cycle time)
                dayCycle++; // change cycle state
            }
            isProcess = false;
        }
        float percent = cycleCurrentTime / cycleMaxTime;
        if (dayCycle % 4 == 0)
            _LightingPanel.color = Color.Lerp(_SunriseLight, _DayLight, percent);
        if (dayCycle % 4 == 1)
            _LightingPanel.color = Color.Lerp(_DayLight, _SunsetLight, percent);
        if (dayCycle % 4 == 2)
            _LightingPanel.color = Color.Lerp(_SunsetLight, _NightLight, percent);
        if (dayCycle % 4 == 3)
            _LightingPanel.color = Color.Lerp(_NightLight, _SunriseLight, percent);
    }
    private void ResetDayLight()
    {
        _LightingPanel.color = _DayLight;
        cycleCurrentTime = 0;
        dayCycle = 0;

    }
}
