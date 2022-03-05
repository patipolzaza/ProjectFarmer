using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/Seed")]
public class Seed : ItemData
{
    public Sprite[] plantStages;
    [Min(1)]
    public int countHarvest;
    [Min(1)]
    public int waterNeed;
    public Item product;
}
