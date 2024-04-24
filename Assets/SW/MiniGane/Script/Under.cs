using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Under : MonoBehaviour
{
    [SerializeField] public Tilemap tilemap { get; private set; }//-- set은 비공개 처리 

    [SerializeField] public Tile[] expansionPlant;    // 뿌리 타일
    [SerializeField] public Tile dirtTile;
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
                tilemap.SetTile(cell.position, GetRevealedTile(cell));
            }
        }
    }

    public void CountCellsType(in Cell[,] state, out int plant)
    {
        plant = 0;

        for (int i = 0; i < state.GetLength(0); i++)
        {
            for (int j = 0; j < state.GetLength(1); j++)
            {
                if (state[i, j].type == Cell.Type.StartPlant || state[i, j].type == Cell.Type.Number)
                    if (!state[i,j].isRevealed)
                        plant++;
            }
        }
    }

    private Tile GetRevealedTile(Cell cell)
    {
        switch (cell.type)
        {
            case Cell.Type.Empty: return dirtTile;
            case Cell.Type.Center: return cutPlant;
            case Cell.Type.StartPlant: return expansionPlant[6];
            case Cell.Type.CUT: return cutPlant;
            case Cell.Type.Number: return Numbertile(cell);
            default: return null;
        }
    }

    private Tile Numbertile(Cell cell)
    {
        if (cell.nextCell == Vector2Int.zero || cell.prevCell == Vector2Int.zero)
            return expansionPlant[6];

        // 왼쪽/아래
        if (cell.prevCell + cell.nextCell == new Vector2Int(1, -1))
            return expansionPlant[3];
        // 오른쪽/아래
        if (cell.prevCell + cell.nextCell == new Vector2Int(-1, -1))
            return expansionPlant[2];
        // 왼쪽/위 
        if (cell.prevCell + cell.nextCell == new Vector2Int(1, 1))
            return expansionPlant[5];
        // 오른쪽/위
        if (cell.prevCell + cell.nextCell == new Vector2Int(-1, 1))
            return expansionPlant[4];

        if (cell.prevCell + cell.nextCell == new Vector2Int(0, 0))
        {
            // 왼쪽/오른쪽
            if (cell.prevCell.y == cell.nextCell.y)
                return expansionPlant[0];
            // 위/아래
            if (cell.prevCell.x == cell.nextCell.x)
                return expansionPlant[1];
        }

        return null;
    }
}