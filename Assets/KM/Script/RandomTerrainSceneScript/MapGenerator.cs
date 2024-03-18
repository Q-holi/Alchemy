using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.Tilemaps;

enum TileType
{ 
    Wall = 0,
    Dirt,
    Grass,
    Border_Dirt,
    Border_Grass
}

public class MapGenerator : MonoBehaviour
{
    // 타일맵을 그릴 타일
    [SerializeField] private Tilemap tilemap;

    // 타일맵에 그려질 타일
    [SerializeField] public Tile wallTile;     
    [SerializeField] public List<Tile> floorTile;

    // 맵의 크기
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private string seed;         // 맵 랜덤생성 시드 지정
    [SerializeField] private bool useRandomSeed;  // 무작위 생성 체크용 변수

    // 지형의 랜덤률(?) 결정
    [Range(0, 100)]
    [SerializeField] private int randomFillPercent;
    [SerializeField] private int wallThresholdSize; // 제거할 벽 타일의 크기
    [SerializeField] private int roomThresholdSize; // 제거할 방 타일의 크기

    private int[,] map;

    struct Coord
    {
        public int tileX;
        public int tileY;

        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
    }

    private void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            GenerateMap();
    }

    private void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < 8; i++)
            SmoothMap();

        ProcessMap();

        DrawTile();
    }

    // 부자연스러운 작은 공간이나 섬 제거
    private void ProcessMap()
    {
        List<List<Coord>> wallRegions = GetRegions(1);  // 공간이 벽인 타일 검사

        foreach (List<Coord> wallRegion in wallRegions)
        {
            if (wallRegion.Count < wallThresholdSize)   // 영역의 크기가 벽 제거크기보다 작다면
            {
                foreach (Coord tile in wallRegion)
                    map[tile.tileX, tile.tileY] = 0;    // 타일을 바닥으로 만듦
            }
        }

        List<List<Coord>> roomRegions = GetRegions(0);  // 공간이 방인 타일 검사

        foreach (List<Coord> roomRegion in roomRegions)
        {
            if (roomRegion.Count < roomThresholdSize)   // 영역의 크기가 방 제거크기보다 작다면
            {
                foreach (Coord tile in roomRegion)
                    map[tile.tileX, tile.tileY] = 1;    // 타일을 벽으로 만듦
            }
        }
    }

    // 생성된 방의 영역 체크
    private List<List<Coord>> GetRegions(int tileType)
    {
        List<List<Coord>> regions = new List<List<Coord>>();    // 검사한 맵의 영역(범위)
        int[,] mapFlags = new int[width, height];   // 검사할 맵 영역

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapFlags[x, y] == 0 && map[x, y] == tileType) //
                {
                    List<Coord> newRegion = GetResionTiles(x, y);
                    regions.Add(newRegion); // 영역 정보 추가

                    // 검사완료된 영역의 타일 정보 전달
                    foreach (Coord tile in newRegion)
                        mapFlags[tile.tileX, tile.tileY] = 1;
                }
            }
        }

        return regions;
    }

    // 홍수 흐름 알고리즘
    private List<Coord> GetResionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();  // 검사할 타일들이 추가될 변수
        int[,] mapFlags = new int[width, height];   // 타일 검사 유무
        int tileType = map[startX, startY];     // 시작좌표의 방과 같은 타입(방 or 벽)의 방을 검사

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY)); // 큐에 시작점 좌표 추가
        mapFlags[startX, startY] = 1;   // 영역 검사완료 처리

        while (queue.Count > 0) // 큐에 남은 타일이 없을때까지
        {
            Coord tile = queue.Dequeue();   // 큐에 추가한 첫번째 타일
            tiles.Add(tile);                // 그 타일을 영역에 추가

            // 검사 영역으로부터 주변 8칸
            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    // 맵의 범위 안이면서, 상,하,좌,우 의 타일일때 (대각선 영역타일 검사 제외)
                    if (IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
                    {
                        // 검사미완료 영역이고, 같은 방타입이라면
                        if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                        {
                            mapFlags[x, y] = 1;  // 검사완료 처리
                            queue.Enqueue(new Coord(x, y)); // 해당 타일을 큐에 추가
                        }
                    }
                }
            }
        }

        return tiles;
    }

    // 출력할 맵의 범위 안인지 검사
    private bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    // 맵을 랜덤하게 채움
    private void RandomFillMap()
    {
        // 시드를 사용할것인지 체크
        if (useRandomSeed)
            seed = Time.time.ToString();

        // 시드를 사용하면 일정한 모양을 생성할수 있다.
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1) // 맵의 최외곽은 항상 벽
                    map[x, y] = 1;
                else
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0; // 벽인지 지형인지 결정
            }
        }
    }

    // 주변 타일을 검사해서 지형을 뭉개서 다듬는 함수
    private void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                if (neighbourWallTiles > 4) // 주변 8칸중 막힌 공간이 4개보다 많으면 벽으로 전환
                    map[x, y] = 1;
                else if (neighbourWallTiles < 4) // 아니라면 그 타일은 여전히 방
                    map[x, y] = 0;
            }
        }
    }

    // 지점 주변의 벽 갯수 검사
    private int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        // 선택된 지점의 주변 8칸 검사
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                // outofRange 예외처리, 검사 대상이 0보다는 크고, 범위보다는 작은지 검사
                if (IsInMapRange(neighbourX, neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY) // 자기 자신은 제외
                        wallCount += map[neighbourX, neighbourY];
                }
                else
                    wallCount++;
            }
        }
        return wallCount;
    }

    // 맵에 들어간 데이터를 gizmo 사용해 출력
    //private void OnDrawGizmos()
    //{
    //    if (map != null)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            for (int y = 0; y < height; y++)
    //            {
    //                Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
    //                Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
    //                Gizmos.DrawCube(pos, Vector3.one);
    //            }
    //        }
    //    }
    //}


    // 맵 정보를 토대로 타일 출력
    public void DrawTile()
    {
        tilemap.ClearAllTiles();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int pos = new Vector3Int((int)(-width / 2 + x), (int)(-height / 2 + y), 0);
                if (map[x, y] == 1)
                    tilemap.SetTile(pos, wallTile);
                else
                    tilemap.SetTile(pos, floorTile[UnityEngine.Random.Range(0, floorTile.Count)]);
            }
        }
    }
}
