using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public Status plotSizeStatus { get; private set; }
    [SerializeField] private StatusData plotSizeStatusData;
    public List<Plot> plots = new List<Plot>();
    public int GetCurrentPlotCount
    {
        get
        {
            return plots.Count;
        }
    }

    public Status maxAnimalStatus { get; private set; }
    [SerializeField] private StatusData maxAnimalStatusData;
    public List<Animal> animals = new List<Animal>();
    public int GetCurrentAnimalCount
    {
        get
        {
            return animals.Count;
        }
    }

    private void Awake()
    {
        plotSizeStatus = new Status("Plot Size", plotSizeStatusData);
        maxAnimalStatus = new Status("Max Animal", maxAnimalStatusData);
    }
}
