using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public class Inventory : MonoBehaviour
{
    [SerializeField] private List<IItem> items = new List<IItem>();
    private int maxInventoryCount = 32;

    private void Awake()
    {
       
    }
    private void Start()
    {

    }
}
