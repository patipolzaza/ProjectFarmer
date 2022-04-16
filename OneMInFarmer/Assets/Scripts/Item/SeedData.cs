using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/SeedData")]
public class SeedData : ItemData
{
    public Sprite[] plantStages;
    [Min(1)]
    public int countHarvest;
    [Min(1)]
    public int waterNeed;
    public Product product;

}
