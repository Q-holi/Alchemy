using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BattleCharacterObj
{
    private void Awake()
    {
        CharacterInit();
    }

    public override void CharacterInit()
    {
        defaultStatus.hp = 50;
        defaultStatus.shield = 0;
        defaultStatus.name = "Enemy";
        defaultStatus.atkPower = 8;
        defaultStatus.defPower = 3;
        defaultStatus.speed = 5;

        currentStatus = defaultStatus;
        HpbarInit();
        SpeedBarInit();
    }
}
