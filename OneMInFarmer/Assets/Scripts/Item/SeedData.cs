using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Items/ItemDatas/SeedData")]
public class SeedData : ItemData
{
    public Sprite[] plantStages;
    public int purchasePrice = 5;
    [Min(1)]
    public int countHarvest = 1;
    [Min(1)]
    public int waterNeed = 2;
    public Product product;

}
