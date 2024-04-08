using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class Board : MonoBehaviour
{
    [SerializeField] public Tilemap tilemap {  get; private set; }//-- set은 비공개 처리 

    [SerializeField] public RuleTile tileUnknow;        // 확인되지 않은 위치의 타일
    [SerializeField] public RuleTile cutPlant;          // 뿌리가 잘린 타일 이미지
    [SerializeField] public RuleTile expansionPlant;    // 뿌리 타일


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
    private RuleTile GetTile(Cell cell)
    {
        if (cell.revealed) 
            return GetRevealedTile(cell);
        else 
            return tileUnknow;
        
    }
    private RuleTile GetRevealedTile(Cell cell)
    {
        switch(cell.type)
        {
            case Cell.Type.Empty: return null;
            case Cell.Type.Plant: return expansionPlant;
            case Cell.Type.StartPlant: return expansionPlant;
            case Cell.Type.CUT: return cutPlant;
            case Cell.Type.Number: return expansionPlant;
            default: return null;
        }
    }
}
