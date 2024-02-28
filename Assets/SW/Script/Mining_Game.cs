using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mining_Game : MonoBehaviour
{
    [SerializeField] private int width = 14;
    [SerializeField] private int height = 14;

    [SerializeField] private Board board;
    [SerializeField] private Cell[,] state;

    private void Awake()
    {
        board = GetComponentInChildren<Board>();
    }
    private void Start()
    {
        NewGame();
    }
    private void NewGame()
    {
        state = new Cell[width, height];
        GenerateCells();//--게임에 필요한 셀 생성
        board.Draw(state);//--타입에 맞게 각 셀 tile 설정
    }

    private void GenerateCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = new Cell();
                cell.position = new Vector3Int(x, y, 0);
                cell.type = Cell.Type.Empty;
                state[x, y] = cell;
            }
        }
    }
}
