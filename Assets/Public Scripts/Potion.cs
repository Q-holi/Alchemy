using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Potion : Item
{
    public int amount;
    public int duration;
    public List<BasePotionData> enchantPotions = new List<BasePotionData>(); 

    public Potion(BasePotionData data) : base(data)
    {
        enchantPotions.Add(data);
        amount = data.amount;
        duration = data.duration;
    }

    public void EnchantPotion(BasePotionData data)
    {
        enchantPotions.Add(data);
    }
}
