using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightingController : WindowUIBase
{
    public static LightingController Instance { get; private set; }

    [SerializeField] private Image _LightingPanel;
    [SerializeField] private Color _DayLight;
    [SerializeField] private Color _NightLight;
    public float cycleCurrentTime = 0;
    public float cycleMaxTime = 60;
    private int dayCycle = 0;
    public bool isLoopCycle = false;
    // Update is called once per frame
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {

        UpdateLighting();
    }

    private void UpdateLighting()
    {
        cycleCurrentTime += Time.deltaTime * 10;
        if (cycleCurrentTime >= cycleMaxTime)
        {
            if (isLoopCycle)
            {
                cycleCurrentTime = 0; // back to 0 (restarting cycle time)
                dayCycle++; // change cycle state
            }
        }
        float percent = cycleCurrentTime / cycleMaxTime;
        if (dayCycle % 2 == 0)
            _LightingPanel.color = Color.Lerp(_DayLight, _NightLight, percent);
        if (dayCycle % 2 != 0)
            _LightingPanel.color = Color.Lerp(_NightLight, _DayLight, percent);
    }
}
