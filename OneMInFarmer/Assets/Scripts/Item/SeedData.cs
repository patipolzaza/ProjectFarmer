using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/Items/ItemDatas/SeedData")]
public class SeedData : ItemData
{
    public Sprite[] plantStages;
    public int purchasePrice = 5;
    [Min(1)]
    public int countHarvest = 1;
    [Min(1)]
    public Product product;

    public string GetPath => AssetDatabase.GetAssetPath(this);
}
