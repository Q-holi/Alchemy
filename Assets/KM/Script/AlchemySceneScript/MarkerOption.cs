using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerOption : MonoBehaviour
{
    public BasePotionData potionData;

    public Potion SetPotion(Potion targetPotion)
    {
        targetPotion.EnchantPotion(potionData);
        return targetPotion;
    }
}
