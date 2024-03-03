using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Collection : Item
{
    [SerializeField] private int red_Option;
    public int Red_Option { get => red_Option; set => red_Option = value; }


    [SerializeField] private int black_Option;
    public int Black_Option { get => black_Option; set => black_Option = value; }


    [SerializeField] private int blue_Option;

    public int Blue_Option { get => blue_Option; set => blue_Option = value; }

}
