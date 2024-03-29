using System.Collections.Generic;

[System.Serializable]

public class GameObjectSave
{
    // string key = scene name
    public Dictionary<string, ItemSave> itemData;
    public GameObjectSave()
    {
        itemData = new Dictionary<string, ItemSave>();
    }
    public GameObjectSave(Dictionary<string, ItemSave> itemData)
    {
        this.itemData = itemData;
    }
}
