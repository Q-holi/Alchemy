using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Rating
{
    Normal,
    Rare,
    Epic,
    Legend
}
public class Item : MonoBehaviour
{

    public int keyvalue;
    public string name;
    public Rating rating;
    public string texture2DImagePath;
    public int count;

    private void Awake()
    {
    }
}
