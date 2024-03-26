using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Potion : Item
{
    public int amount;
    public int duration;
    public List<BasePotionData> enchantPotions = new List<BasePotionData>(); 

    public Potion(int keyCode) : base(keyCode)
    {
        if (InventoryManager.itemDB[keyCode] is BasePotionData potionData)
        {
            amount = potionData.amount;
            duration = potionData.duration;
        }
    }

    public void EnchantPotion(BasePotionData data)
    {
        enchantPotions.Add(data);
    }
}
