using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    [SerializeField] public Tilemap tilemap {  get; private set; }//-- set은 비공개 처리 

    [SerializeField] public Tile tileUnknow;
    [SerializeField] public Tile tileEmpty;
    [SerializeField] public Tile tilePlant;
    [SerializeField] public Tile startPlant;
    [SerializeField] public Tile cutPlant;
    [SerializeField] public Tile expansionPlant1;
    [SerializeField] public Tile expansionPlant2;
    [SerializeField] public Tile expansionPlant3;
    [SerializeField] public Tile expansionPlant4;
    [SerializeField] public Tile expansionPlant5;
    [SerializeField] public Tile expansionPlant6;
    [SerializeField] public Tile expansionPlant7;
    [SerializeField] public Tile expansionPlant8;


    private void OnMouseDown()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void Draw(Cell[,] state)
    {
        int width = state.GetLength(0); 
        int height = state.GetLength(1); 
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];
                tilemap.SetTile(cell.position, GetTile(cell));
            }
        }
    }
    public void CountCellsType(in Cell[,] state, out int empty, out int plant)
    {
        empty = 0;
        plant = 0;

        for (int i = 0; i < state.GetLength(0); i++)
        {
            for (int j = 0; j < state.GetLength(1);  j++)
            {
                if (state[i, j].type == Cell.Type.Empty || state[i, j].type == Cell.Type.Invalid)
                    empty++;
                else if (state[i, j].type == Cell.Type.StartPlant || state[i, j].type == Cell.Type.Number)
                    plant++;
            }
        }
    }
    private Tile GetTile(Cell cell)
    {
        if (cell.revealed) return GetRevealedTile(cell);
        else return tileUnknow;
        
    }
    private Tile GetRevealedTile(Cell cell)
    {
        switch(cell.type)
        {
            case Cell.Type.Empty: return tileEmpty;
            case Cell.Type.Plant: return tilePlant;
            case Cell.Type.StartPlant: return startPlant;
            case Cell.Type.CUT: return cutPlant;
            case Cell.Type.Number: return GetCountPlant(cell);
            default: return null;
        }
    }

    private Tile GetCountPlant(Cell cell)
    {
        switch(cell.number) 
        {
            case 1: return expansionPlant1;
            case 2: return expansionPlant2;
            case 3: return expansionPlant3;
            case 4: return expansionPlant4;
            case 5: return expansionPlant5;
            case 6: return expansionPlant6;
            case 7: return expansionPlant7;
            case 8: return expansionPlant8;
            default: return null;
        }
    }
}
