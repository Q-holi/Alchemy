using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_Item : MonoBehaviour
{ 
    [SerializeField] private int _itemCode;

    public int ItemCode { get { return _itemCode; } set { _itemCode = value; } }
}
