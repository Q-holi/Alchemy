using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePotion : PotionEnchant
{
    public int damageAmount;

    public DamagePotion(Potion potion, int amount)
    {
        this.potion = potion;
        this.Name = "Damage_Potion";
        damageAmount = amount;
    }

    public override void PotionEffect()
    {
        Debug.Log(damageAmount + " 의 피해를 줄 수 있는 포션이다.");
    }
}
