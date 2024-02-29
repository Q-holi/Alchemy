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
            default: return null;
        }
    }
}
