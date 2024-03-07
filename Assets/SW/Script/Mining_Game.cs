using System;
using System.Collections.Generic;
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) DeepDigging();

    }

    private void DeepDigging()
    {

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
                //state[x, y].revealed = true;
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
            //====================================================
            int expansionLengh = UnityEngine.Random.Range(2, 5);
            ExpansionMine(4, resultYPos, expansionLengh);
        }


        //--{5~8,4} 상

        result = UnityEngine.Random.Range(0, 2);
        boolValue = Convert.ToBoolean(result);
        if (boolValue)
        {
            int resultXPos = UnityEngine.Random.Range(5, 9);
            state[resultXPos, 4].type = Cell.Type.Plant;
            state[resultXPos, 4].revealed = true;
            //====================================================
            int expansionLengh = UnityEngine.Random.Range(2, 5);
            ExpansionMine(resultXPos, 4, expansionLengh);

        }


        //--{9,5~8} 우

        result = UnityEngine.Random.Range(0, 2);
        boolValue = Convert.ToBoolean(result);
        if (boolValue)
        {
            int resultYPos = UnityEngine.Random.Range(5, 9);
            state[9, resultYPos].type = Cell.Type.Plant;
            state[9, resultYPos].revealed = true;
            //====================================================
            int expansionLengh = UnityEngine.Random.Range(2, 5);
            ExpansionMine(9, resultYPos, expansionLengh);
        }


        //--{5~8,9} 하

        result = UnityEngine.Random.Range(0, 2);
        boolValue = Convert.ToBoolean(result);
        if (boolValue)
        {
            int resultxPos = UnityEngine.Random.Range(5, 9);
            state[resultxPos, 9].type = Cell.Type.Plant;
            state[resultxPos, 9].revealed = true;
            //====================================================
            int expansionLengh = UnityEngine.Random.Range(2, 5);
            ExpansionMine(resultxPos, 9, expansionLengh);
        }
    }


    private void ExpansionMine(int posX, int posY, int expansionLengh)
    {
        List<Vector2Int> expansionPos = new List<Vector2Int>();
       if (expansionLengh == 0)
            return;

        //--검사 시작
        if (state[posX, posY + 1].type != Cell.Type.Plant) //--상 
            expansionPos.Add(new Vector2Int(posX, posY + 1));

        if (state[posX, posY - 1].type != Cell.Type.Plant) //--하
            expansionPos.Add(new Vector2Int(posX, posY - 1));

        if (state[posX-1, posY].type != Cell.Type.Plant) //--좌
            expansionPos.Add(new Vector2Int(posX - 1, posY));

        if (state[posX+1, posY].type != Cell.Type.Plant) //--우
            expansionPos.Add(new Vector2Int(posX + 1, posY));

        int randomPos = UnityEngine.Random.Range(0, expansionPos.Count);
        state[expansionPos[randomPos].x, expansionPos[randomPos].y].type = Cell.Type.Plant;
        state[expansionPos[randomPos].x, expansionPos[randomPos].y].revealed = true;


        ExpansionMine(expansionPos[randomPos].x, expansionPos[randomPos].y, expansionLengh - 1);

    }
    private void EndGame()
    {
        
    }
}
