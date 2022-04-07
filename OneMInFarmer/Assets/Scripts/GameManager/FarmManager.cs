using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance { get; private set; }

    public Status plotSizeStatus { get; private set; }
    [SerializeField] private StatusData plotSizeStatusData;
    private List<Plot> plots;
    private int lastAvailablePlotIndex = 0;

    public Status maxAnimalStatus { get; private set; }
    [SerializeField] private StatusData maxAnimalStatusData;
    private List<Animal> animals = new List<Animal>();
    public int GetCurrentAnimalCount
    {
        get
        {
            return animals.Count;
        }
    }

    private void Awake()
    {
        Instance = this;
        plotSizeStatus = new Status("Plot Size", plotSizeStatusData);
        maxAnimalStatus = new Status("Max Animal", maxAnimalStatusData);

        plots = new List<Plot>(FindObjectsOfType<Plot>());

        UnlockPlots();
    }

    public bool AddAnimal(Animal animalToAdd)
    {
        if (animals.Count >= maxAnimalStatus.GetValue || !animalToAdd)
        {
            return false;
        }

        animals.Add(animalToAdd);
        return true;
    }

    public bool RemoveAnimal(Animal animalToRemove)
    {
        if (!animalToRemove || !animals.Contains(animalToRemove))
        {
            return false;
        }

        animals.Remove(animalToRemove);
        return true;
    }

    public bool UnlockPlots()
    {
        if (lastAvailablePlotIndex == plotSizeStatus.GetValue - 1)
        {
            return false;
        }

        int loopCount = plotSizeStatus.GetValue;
        for (int i = lastAvailablePlotIndex; i < loopCount; i++)
        {
            //plots[i].Unlock();
        }

        return true;
    }
}
