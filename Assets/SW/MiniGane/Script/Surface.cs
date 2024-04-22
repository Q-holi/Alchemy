using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Surface : MonoBehaviour
{
    [SerializeField] public Tilemap tilemap { get; private set; }//-- set은 비공개 처리 

    [SerializeField] public Tile tileUnknow;        // 확인되지 않은 위치의 타일
    [SerializeField] public Tile cutPlant;        

    private void OnMouseDown()
    {

    }

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void SetGridPos(int width, int height)
    {
        //Vector3 gridPos = GameObject.FindWithTag("MainCamera").GetComponent<Camera>().transform.position;
        Vector3 gridPos = EventHandler.CallSetMiniGameScreen();
        gridPos.z = 0.0f;
        gridPos.x -= width / 2f;
        gridPos.y -= height / 2f - 1.0f;
        gameObject.transform.position = gridPos;
    }

    public void Draw(Cell[,] state)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
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
            for (int j = 0; j < state.GetLength(1); j++)
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
        if (cell.isRevealed)
            return GetRevealedTile(cell);
        else
            return tileUnknow;
    }

    private Tile GetRevealedTile(Cell cell)
    {
        switch (cell.type)
        {
            case Cell.Type.Empty: return null;
            case Cell.Type.Center: return cutPlant;
            default: return null;
        }
    }
}