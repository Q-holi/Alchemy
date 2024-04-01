using System.Reflection;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class N_ItemDetail
{
    public int itemCode;
    public ItemRating itemRating;
    public string itemDescription;
}
[System.Serializable]
public class N_ItemCollection : N_ItemDetail
{
    public Vector3Int itemOption;
}
[System.Serializable]
public class N_ItemTool : N_ItemDetail
{
    public float durability;
}



[CreateAssetMenu(fileName = "so_ItemList", menuName = "Scriptable Objects/Item/Item List")]
public class SO_ItemList : ScriptableObject
{
    [SerializeField]
    public List<N_ItemCollection> collectionDetails;

    [SerializeField]
    public List<N_ItemTool> toolDetails;

}
