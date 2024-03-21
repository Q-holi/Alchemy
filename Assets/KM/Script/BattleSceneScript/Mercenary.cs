using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercenary : BattleCharacterObj
{
    private void Awake()
    {
        CharacterInit();
    }

    public override void CharacterInit()
    {
        defaultStatus.hp = 150;
        defaultStatus.shield = 0;
        defaultStatus.name = "Mercenary";
        defaultStatus.atkPower = 15;
        defaultStatus.defPower = 10;
        defaultStatus.speed = 10;

        HpbarInit();
        SpeedBarInit();
    }
}
