using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Collection_Tool : Item
{
    [SerializeField] private int x_Range;
    public int X_Range { get => x_Range; set => x_Range = value; }


    [SerializeField] private int y_Range;
    public int Y_Range { get => y_Range; set => y_Range = value; }
}
