using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    [SerializeField] Plot[] plots;

    public void nextDay()
    {
        foreach(Plot plot in plots)
        {
            if(plot.isPlanted)
            plot.Grow();
        }
    }
}
