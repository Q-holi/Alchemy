using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BattleCharacterObj
{
    private void Awake()
    {
        CharacterInit();
    }

    public override void CharacterInit()
    {
        defaultStatus.hp = 100;
        defaultStatus.shield = 0;
        defaultStatus.name = "Player";
        defaultStatus.atkPower = 0;
        defaultStatus.defPower = 5;
        defaultStatus.speed = 10;

        HpbarInit();
        SpeedBarInit();
    }
}
