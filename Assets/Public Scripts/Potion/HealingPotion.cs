using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealingPotion : PotionEnchant
{
    public int healAmount;

    public HealingPotion(Potion potion, int amount)
    {
        this.potion = potion;
        this.Name = "Healing_Potion";
        healAmount = amount;
    }

    public override void PotionEffect()
    {
        Debug.Log(healAmount + " 의 회복 효과가 있는 포션이다.");
    }
}
