using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    IDLE,
    MOVE,
    FIGHT,
    HIDE,
    CC,
    DEATH
}

public class Tileinfo
{
    public TileType tileType;
}
