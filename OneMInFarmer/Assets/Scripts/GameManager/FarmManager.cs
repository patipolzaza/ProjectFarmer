using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public Status plotSizeStatus { get; private set; }
    [SerializeField] private StatusData plotSizeStatusData;
    private List<Plot> plots = new List<Plot>();
    public int GetCurrentPlotCount
    {
        get
        {
            return plots.Count;
        }
    }

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
        plotSizeStatus = new Status("Plot Size", plotSizeStatusData);
        maxAnimalStatus = new Status("Max Animal", maxAnimalStatusData);
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
}
