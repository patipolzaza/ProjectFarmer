using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class AnimalFarmManager : MonoBehaviour, IContainStatus
{
    public static AnimalFarmManager Instance { get; private set; }

    private MaxAnimalStatusSaveData _saveData;
    public Status maxAnimalStatus { get; private set; }
    [SerializeField] private StatusData maxAnimalStatusData;
    private List<Animal> animals = new List<Animal>();

    private int _currentLevelInMem;

    public delegate void OnValueChangedDelegate(int currentAnimalCount, int maxAnimal);
    public OnValueChangedDelegate OnValueChanged;

    public int GetCurrentAnimalCount => animals.Count;

    private void Awake()
    {
        maxAnimalStatus = new Status("Max Animal", maxAnimalStatusData);
        _currentLevelInMem = maxAnimalStatus.currentLevel;
        Instance = this;
    }

    private void Update()
    {
        if (maxAnimalStatus.currentLevel != _currentLevelInMem)
        {
            InvokeOnValueChangedDelegate(animals.Count, maxAnimalStatus.GetValue);
        }
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
        _currentLevelInMem = maxAnimalStatus.currentLevel;
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
        InvokeOnValueChangedDelegate(animals.Count, maxAnimalStatus.GetValue);
        return true;
    }

    public bool RemoveAnimal(Animal animalToRemove)
    {
        if (!animalToRemove || !animals.Contains(animalToRemove))
        {
            return false;
        }

        animals.Remove(animalToRemove);
        InvokeOnValueChangedDelegate(animals.Count, maxAnimalStatus.GetValue);
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
        int loopCount = animalSaveDatas.Count;
        for (int i = 0; i < loopCount; i++)
        {
            var saveData = animalSaveDatas[i];
            var animalPrefab = Resources.Load<GameObject>(saveData.GetAnimalPrefabPath);

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

    private void InvokeOnValueChangedDelegate(int currentAnimalCount, int maxAnimal)
    {
        OnValueChanged?.Invoke(currentAnimalCount, maxAnimal);
    }
}
