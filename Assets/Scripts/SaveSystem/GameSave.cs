using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSave
{
    public Dictionary<string, GameObjectSave> gameObjectData;
    //public Dictionary<string, "필요한 다른 자료형"> "필요한 다른 자료형";

    public GameSave()
    {
        gameObjectData = new Dictionary<string, GameObjectSave>();
    }
}
