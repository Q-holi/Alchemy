using System;
using UnityEngine;


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
        GenerateCenter();
        GenerateStartMine();
        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10.0f); ;
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

    private void GenerateCenter()
    {
        for (int x = 5; x < 9; x++)
        {
            for (int y = 5; y < 9; y++)
            {
                state[x, y].type = Cell.Type.Plant;
                state[x, y].revealed = true;
            }
        }
    }

    private void GenerateStartMine()
    {
        //--{4,5~8} 좌
    
        int result = UnityEngine.Random.Range(0,2);
        bool boolValue = Convert.ToBoolean(result);
        if(boolValue)
        {
            int resultYPos = UnityEngine.Random.Range(5,9);
            state[4, resultYPos].type = Cell.Type.Plant;
            state[4, resultYPos].revealed = true;
        }


        //--{5~8,4} 상

        result = UnityEngine.Random.Range(0, 2);
        boolValue = Convert.ToBoolean(result);
        if (boolValue)
        {
            int resultXPos = UnityEngine.Random.Range(5, 9);
            state[resultXPos, 4].type = Cell.Type.Plant;
            state[resultXPos, 4].revealed = true;
        }


        //--{9,5~8} 우

        result = UnityEngine.Random.Range(0, 2);
        boolValue = Convert.ToBoolean(result);
        if (boolValue)
        {
            int resultYPos = UnityEngine.Random.Range(5, 9);
            state[9, resultYPos].type = Cell.Type.Plant;
            state[9, resultYPos].revealed = true;
        }


        //--{5~8,9} 하

        result = UnityEngine.Random.Range(0, 2);
        boolValue = Convert.ToBoolean(result);
        if (boolValue)
        {
            int resultxPos = UnityEngine.Random.Range(5, 9);
            state[resultxPos, 9].type = Cell.Type.Plant;
            state[resultxPos, 9].revealed = true;
        }
    }

    private void EndGame()
    {
       // Item item = new Item(1,1001,"테스트",Rating.Epic,"",1);
       // Collection collection = CollectionFactory.collectionFactory.CreateItem(item);
    }
}
