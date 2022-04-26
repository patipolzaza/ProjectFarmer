using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AnimalFarmManager : MonoBehaviour, IContainStatus
{
    public static AnimalFarmManager Instance { get; private set; }

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
        maxAnimalStatus = new Status("Max Animal", maxAnimalStatusData);
        Instance = this;
    }

    public Status GetStatus
    {
        get
        {
            return maxAnimalStatus;
        }
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

    public void GrowUpAnimals()
    {
        foreach (var animal in animals)
        {
            animal.IncreaseAge();
        }
    }
}
