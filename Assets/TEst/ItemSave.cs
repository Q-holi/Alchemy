using System.Collections.Generic;

[System.Serializable]
public class ItemSave
{
    // string key is an identifier name we choose for this list
    public Dictionary<string, List<Item>> listItemDictionary;
}

