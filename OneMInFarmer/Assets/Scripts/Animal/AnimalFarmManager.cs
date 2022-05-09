using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class AnimalFarmManager : MonoBehaviour, IContainStatus
{
    public static AnimalFarmManager Instance { get; private set; }

    private MaxAnimalStatusSaveData _saveData;
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

    public void LoadSaveData(MaxAnimalStatusSaveData saveData)
    {
        maxAnimalStatus.SetLevel(saveData.GetStatusLevel);

        _saveData = saveData;

        UpdateStatusSaveDataOnContainer();
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
        UpdateStatusSaveDataOnContainer();

        foreach (var animal in animals)
        {
            animal.IncreaseAge();
        }
    }

    private void UpdateStatusSaveDataOnContainer()
    {
        ObjectDataContainer.UpdateMaxAnimalStatusSaveData(_saveData);
    }

    public void LoadAnimalDatas(List<AnimalSaveData> animalSaveDatas)
    {
        foreach (var saveData in animalSaveDatas)
        {
            var animalPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(saveData.GetAnimalPrefabPath);
            var spawnedPosition = saveData.GetAnimalPosition;
            GameObject instantiatedAnimal = Instantiate(animalPrefab, spawnedPosition, Quaternion.identity);
            instantiatedAnimal.name = animalPrefab.name;
            Animal animalComponent = instantiatedAnimal.GetComponent<Animal>();

            if (AddAnimal(animalComponent))
            {
                animalComponent.LoadAnimalData(saveData);
            }
            else
            {
                Destroy(instantiatedAnimal);
            }
        }
    }
}
