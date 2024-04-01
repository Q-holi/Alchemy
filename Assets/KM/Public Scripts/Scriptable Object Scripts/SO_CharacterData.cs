using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_CharacterDataList", menuName = "Battle System/Status Data")]
public class SO_CharacterData : ScriptableObject
{
    [SerializeField]
    public List<CharacterData> characterDatas = new List<CharacterData>();
}
