using System;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class Mining_Game : MonoBehaviour
{
    [SerializeField] private int width = 14;
    [SerializeField] private int height = 14;

    [SerializeField] private Board surfaceBoard;
    [SerializeField] private Board underBoard;

    [SerializeField] private Cell[,] surfaceState;
    [SerializeField] private Cell[,] underState;

    /*
    //-- 확장하려는 뿌리 길이 값 (최소, 최대)
    //-----------------------------------------------------
    [SerializeField] private int _minExpansionLengh = 6;
    [SerializeField] private int _maxExpansionLengh = 8;
    //-----------------------------------------------------
    */
    [SerializeField] private int expansionLengh = 7;
    [SerializeField] private int shallowDigCount = 25;
    [SerializeField] private int emptyCount;
    [SerializeField] private int plantCount;

    private bool IsValid(int x, int y) { return x >= 0 && x < width && y >= 0 && y < height; }

    private void Awake()
    {
        Board[] childBoard = gameObject.GetComponentsInChildren<Board>();

        foreach (Board child in childBoard)
        {
            switch (child.gameObject.name)
            {
                case "UnkownTileMap":
                    surfaceBoard = child;
                    break;
                case "RootTileMap":
                    underBoard = child;
                    break;
            }
        }
    }
    private void Start()
    {
        NewGame();
    }
    private void NewGame()
    {
        surfaceState = new Cell[width, height];
        underState = new Cell[width, height];
        GenerateCells();//--게임에 필요한 셀 생성
        GenerateCenter();
        GenerateStartRoot();
        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10.0f);

        underBoard.CountCellsType(underState, out emptyCount, out plantCount);

        surfaceBoard.Draw(surfaceState);//--타입에 맞게 각 셀 tile 설정
        underBoard.Draw(underState);
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(1))    // 좌클릭 깊게 파기
            ShallowDig();
        else if (Input.GetMouseButtonUp(0)) // 우클릭 얕게 파기
            DeepDig();


        if (Input.GetKeyDown(KeyCode.R))
        {
            GenerateCells();//--게임에 필요한 셀 생성
            GenerateCenter();
            GenerateStartRoot();
            surfaceBoard.CountCellsType(surfaceState, out emptyCount, out plantCount);
            surfaceBoard.Draw(surfaceState);//--타입에 맞게 각 셀 tile 설정
        }
    }

    private Cell GetCell(int x, int y)
    {
        if (IsValid(x, y)) return surfaceState[x, y];
        else return new Cell();
    }


    private void ShallowDig()
    {

        if (shallowDigCount == 0)
            return;

        //-- 마우스 우 클릭 얕게 파기
        /* 얕게 파기란 한칸을 파는걸 의미 한다.
         * 만약 얕게 파기를 실행했으나 그대로이면 그 부분은 아무것도 없는 흙인 부분이다. 만약 얕게 파기 실행 시 한칸이 겉어지고 뿌리가 있으면 그 부분 1칸만 흙을 치우고 뿌리를 노출시킨다.
         */
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = surfaceBoard.tilemap.WorldToCell(mouseWorldPosition);
        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        if (cell.revealed)
            return;

        if (cell.type == Cell.Type.Invalid || cell.type == Cell.Type.Empty)
        {
            shallowDigCount--;
            return;
        }


        if (cell.type == Cell.Type.Plant || cell.type == Cell.Type.StartPlant || cell.type == Cell.Type.Number)
        {
            shallowDigCount--;
            cell.revealed = true;
            surfaceState[cellPosition.x, cellPosition.y] = cell;
            surfaceBoard.Draw(surfaceState);
        }
    }
    private void DeepDig()
    {
        //-- 마우스 왼클릭이 깊게 파기
        /* 깊게 파기란 9칸을 파는걸 의미 한다. 
         만약 9칸중 흑으로 덮인 부분그대로 있으면 그 부분은 뿌리가 있는것이다.
         */
        // 주변 타일의 상대적 위치를 상수로 정의
        int[,] directions = new int[,] 
        {
            {-1, -1}, {-1, 0}, {-1, 1},
            {0, -1},           {0, 1},
            {1, -1},  {1, 0},  {1, 1}
        };

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = surfaceBoard.tilemap.WorldToCell(mouseWorldPosition);
        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        if (cell.type == Cell.Type.Plant || cell.type == Cell.Type.StartPlant || cell.type == Cell.Type.Number)
        {
            cell.revealed = true;
            cell.type = Cell.Type.CUT;
            surfaceState[cellPosition.x, cellPosition.y] = cell;
            Debug.LogError("뿌리 짤림");
        }
        else
        {
            cell.revealed = true;
            surfaceState[cellPosition.x, cellPosition.y] = cell;
        }

        // 주변 타일 반복 처리
        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int xOffset = directions[i, 0];
            int yOffset = directions[i, 1];

            cell = GetCell(cellPosition.x + xOffset, cellPosition.y + yOffset);
            if (cell.type == Cell.Type.Empty || cell.revealed)
            {
                cell.revealed = true;
                surfaceState[cellPosition.x + xOffset, cellPosition.y + yOffset] = cell;
            }
        }
        surfaceBoard.Draw(surfaceState);
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
                surfaceState[x, y] = cell;
                underState[x, y] = cell;
            }
        }
    }

    private void GenerateCenter()
    {
        for (int x = 5; x < 9; x++)
        {
            for (int y = 5; y < 9; y++)
            {
                surfaceState[x, y].type = Cell.Type.Plant;
                underState[x, y].type = Cell.Type.Plant;
                surfaceState[x, y].revealed = true;
                underState[x, y].revealed = true;
            }
        }
    }

    private void GenerateStartRoot()
    {
        List<Vector3Int> startPlantPos = new List<Vector3Int>(); //x-> x축,y-> y축,z-> 뿌리의 길이 

        int resultYPos = UnityEngine.Random.Range(5, 9);
        //int expansionLengh = UnityEngine.Random.Range(_minExpansionLengh, _maxExpansionLengh);//--뿌리의 길이 랜덤으로 생성
        {
            //--{4,5~8} 좌 시작 지점 생성
            underState[4, resultYPos].type = Cell.Type.StartPlant;
            underState[3, resultYPos].type = Cell.Type.StartPlant;
            //state[4, resultYPos].revealed = true;
            startPlantPos.Add(new Vector3Int(3, resultYPos, expansionLengh));

        }
        resultYPos = UnityEngine.Random.Range(5, 9);
        //expansionLengh = UnityEngine.Random.Range(_minExpansionLengh, _maxExpansionLengh);
        {
            //--{9,5~8} 우 시작 지점 생성
            underState[9, resultYPos].type = Cell.Type.StartPlant;
            underState[10, resultYPos].type = Cell.Type.StartPlant;
            //state[9, resultYPos].revealed = true;
            startPlantPos.Add(new Vector3Int(10, resultYPos, expansionLengh));
        }

        int resultXPos = UnityEngine.Random.Range(5, 9);
        //expansionLengh = UnityEngine.Random.Range(_minExpansionLengh, _maxExpansionLengh);//--뿌리의 길이 랜덤으로 생성

        {
            //--{5~8,4} 상 시작 지점 생성
            underState[resultXPos, 4].type = Cell.Type.StartPlant;
            underState[resultXPos, 3].type = Cell.Type.StartPlant;
            //state[resultXPos, 4].revealed = true;
            startPlantPos.Add(new Vector3Int(resultXPos, 3, expansionLengh));
        }

        resultXPos = UnityEngine.Random.Range(5, 9);
        //expansionLengh = UnityEngine.Random.Range(_minExpansionLengh, _maxExpansionLengh);//--뿌리의 길이 랜덤으로 생성
        {
            //--{5~8,9} 하 시작 지점 생성
            underState[resultXPos, 9].type = Cell.Type.StartPlant;
            underState[resultXPos, 10].type = Cell.Type.StartPlant;
            //state[resultXPos, 9].revealed = true;
            startPlantPos.Add(new Vector3Int(resultXPos, 10, expansionLengh));
        }
        //-- 처음 시작 지점을 먼저 생성한다 
        //-- 이후 각 시작점을 기준으로 뿌리를 생성

        foreach (var item in startPlantPos)
            ExpansionRoot(item.x, item.y, item.z);

        //--뿌리 확장 반복 
    }


    private void ExpansionRoot(int posX, int posY, int expansionLengh)
    {
        List<Vector2Int> expansionPos = new List<Vector2Int>();

        if (expansionLengh == 0)
            return;

        //--검사 시작
        if (IsValid(posX, posY + 1) && underState[posX, posY + 1].type == Cell.Type.Empty) //--상 
            expansionPos.Add(new Vector2Int(posX, posY + 1));

        if (IsValid(posX, posY - 1) && underState[posX, posY - 1].type == Cell.Type.Empty) //--하
            expansionPos.Add(new Vector2Int(posX, posY - 1));

        if (IsValid(posX - 1, posY) && underState[posX - 1, posY].type == Cell.Type.Empty) //--좌
            expansionPos.Add(new Vector2Int(posX - 1, posY));

        if (IsValid(posX + 1, posY) && underState[posX + 1, posY].type == Cell.Type.Empty) //--우
            expansionPos.Add(new Vector2Int(posX + 1, posY));

        if (expansionPos.Count == 0) { NewGame(); return; }//-- 확장 가능성이 없으면 새롭게 다시 시작


        //--expansionPos에 추가된 확장 가능한 위치중 랜덤함 위치를 선택해서 확장시킨다.
        int randomPos = UnityEngine.Random.Range(0, expansionPos.Count);
        underState[expansionPos[randomPos].x, expansionPos[randomPos].y].number = expansionLengh;
        underState[expansionPos[randomPos].x, expansionPos[randomPos].y].type = Cell.Type.Number;
        //state[expansionPos[randomPos].x, expansionPos[randomPos].y].revealed = true;
        //---------------------------------------------------------------------

        //--확장하려고 하는 뿌리의 길이 만큼 재귀함수 호출 (확장이 된 좌표를 기준으로 다시 위치 설정)
        ExpansionRoot(expansionPos[randomPos].x, expansionPos[randomPos].y, expansionLengh - 1);

    }
    
    private void EndGame()
    {

    }
}
