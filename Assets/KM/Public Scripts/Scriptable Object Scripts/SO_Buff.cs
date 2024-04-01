using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_BuffList", menuName = "Battle System/Buff Data")]
public class SO_Buff : ScriptableObject
{
    [SerializeField]
    public List<Buff> buffs = new List<Buff>();
}
