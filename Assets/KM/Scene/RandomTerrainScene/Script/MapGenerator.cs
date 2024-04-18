using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public enum TileType
{ 
    FLOOR = 0,
    WALL,
    GRASS,
    NORMAL_COLLECT,
    RARE_COLLECT,
    EPIC_COLLECT,
    LEGEND_COLLECT
}

public class MapGenerator : MonoBehaviour
{
    // 타일을 그릴 타일 맵
    [SerializeField] private Tilemap wallTileMap;
    [SerializeField] private Tilemap floorTileMap;
    [SerializeField] private Tilemap grassTileMap;
    [SerializeField] private Tilemap collectableItemTileMap;
    [SerializeField] private Transform objList;
    private List<GameObject> decoObj = new List<GameObject>();

    // 타일맵에 그려질 타일 스프라이트
    [SerializeField] private Tile[] wallTile = new Tile[4];
    [SerializeField] private RuleTile floorTile;
    [SerializeField] private Tile[] grassTile = new Tile[4];
    [SerializeField] private Tile[] collectingTile = new Tile[4];
    [SerializeField] private GameObject[] decoPrefab = new GameObject[4];

    // 맵의 크기
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private string seed;         // 맵 랜덤생성 시드 지정
    [SerializeField] private bool useRandomSeed;  // 무작위 생성 체크용 변수

    // 지형의 랜덤률(?) 결정
    [Range(0, 100)]
    [SerializeField] private int randomRoomRange;
    // 바닥의 랜덤률 결정
    [Range(0, 100)]
    [SerializeField] private int randomGrassRange;

    [SerializeField] private int wallThresholdSize; // 제거할 벽 타일의 크기
    [SerializeField] private int roomThresholdSize; // 제거할 방 타일의 크기

    // 채집물 생성 확률
    [SerializeField] private float normalObj;
    [SerializeField] private float rareObj;
    [SerializeField] private float epicObj;
    [SerializeField] private float LegendObj;

    private int[,] map;
    [SerializeField] private GameObject spawnobj;    // 스폰지점 표시하는 오브젝트
    [SerializeField] private GameObject exitObj;     // 숲에서 나가는 오브젝트

    public Vector3 spawnPoint;
    private bool isMaking;

    // 타일의 좌표
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

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += GenerateMap;
        EventHandler.GetSpawnPointEvent += GetSpawnPoint;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= GenerateMap;
        EventHandler.GetSpawnPointEvent -= GetSpawnPoint;
    }

    /// <summary>
    /// 실제 맵 생성하기
    /// </summary>
    public void GenerateMap()
    {
        isMaking = true;

        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < 5; i++)
            SmoothMap();

        ProcessMap();

        RandomFillGrassTile();

        for (int i = 0; i < 3; i++)
            SmoothDeco();

        DrawTile();
        RandomSpawnCollecter(normalObj, TileType.NORMAL_COLLECT);
        RandomSpawnCollecter(rareObj, TileType.RARE_COLLECT);
        RandomSpawnCollecter(epicObj, TileType.EPIC_COLLECT);
        RandomSpawnCollecter(LegendObj, TileType.LEGEND_COLLECT);
        SetSpawnPoint();

        isMaking = false;
    }

    /// <summary>
    /// 부자연스러운 작은 공간이나 섬 제거
    /// </summary>
    private void ProcessMap()
    {
        List<List<Coord>> wallRegions = GetRegions(1);  // 공간이 벽인 타일 검사

        foreach (List<Coord> wallRegion in wallRegions)
        {
            if (wallRegion.Count < wallThresholdSize)   // 영역의 크기가 벽 제거크기보다 작다면
            {
                foreach (Coord tile in wallRegion)
                    map[tile.tileX, tile.tileY] = (int)TileType.FLOOR;    // 타일을 바닥으로 만듦
            }
        }

        List<List<Coord>> roomRegions = GetRegions(0);  // 공간이 방인 타일 검사

        foreach (List<Coord> roomRegion in roomRegions)
        {
            if (roomRegion.Count < roomThresholdSize)   // 영역의 크기가 방 제거크기보다 작다면
            {
                foreach (Coord tile in roomRegion)
                    map[tile.tileX, tile.tileY] = (int)TileType.WALL;    // 타일을 벽으로 만듦
            }
        }
    }

    /// <summary>
    /// 생성된 방의 영역 체크
    /// </summary>
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

    /// <summary>
    /// 홍수 흐름 알고리즘
    /// </summary>
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

    /// <summary>
    /// 출력할 맵의 범위 안인지 검사
    /// </summary>
    private bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    /// <summary>
    /// 맵을 랜덤하게 채움
    /// </summary>
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
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomRoomRange) ? (int)TileType.WALL : (int)TileType.FLOOR; // 벽인지 지형인지 결정
            }
        }
    }

    /// <summary>
    /// 랜덤하게 장식 바닥 생성
    /// </summary>
    private void RandomFillGrassTile()
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
                if (map[x, y] == 0)
                {
                    int neighbourWallTiles = GetSurroundingTileCount(x, y, TileType.WALL);
                    if (neighbourWallTiles >= 1)    // 주변 8칸중에 벽이 한칸이라도 있으면 그대로 바닥으로 인식
                        map[x, y] = 0;
                    else if (neighbourWallTiles < 1) // 아니라면 그 타일은 확률적으로 장식 타일
                        map[x, y] = (pseudoRandom.Next(0, 100) < randomGrassRange) ? (int)TileType.GRASS : (int)TileType.FLOOR; // 바닥인지 장식인지 결정
                }
            }
        }
    }

    private void RandomSpawnCollecter(float rating, TileType rank)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] != (int)TileType.WALL)
                {
                    map[x, y] = (UnityEngine.Random.Range(0f, 1f) < rating) ? (int)rank : map[x,y]; // 바닥인지 장식인지 결정
                }
            }
        }

        collectableItemTileMap.ClearAllTiles();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int pos = new Vector3Int((int)(-width / 2 + x), (int)(-height / 2 + y), 0);
                switch (map[x, y])
                {
                    case (int)TileType.NORMAL_COLLECT:
                        collectableItemTileMap.SetTile(pos, collectingTile[0]);
                        break;
                    case (int)TileType.RARE_COLLECT:
                        collectableItemTileMap.SetTile(pos, collectingTile[1]);
                        break;
                    case (int)TileType.EPIC_COLLECT:
                        collectableItemTileMap.SetTile(pos, collectingTile[2]);
                        break;
                    case (int)TileType.LEGEND_COLLECT:
                        collectableItemTileMap.SetTile(pos, collectingTile[3]);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 주변 타일을 검사해서 지형을 뭉개 다듬는 함수
    /// </summary>
    private void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingTileCount(x, y, TileType.WALL);
                if (neighbourWallTiles > 4) // 주변 8칸중 벽이 4개보다 많으면 벽으로 전환
                    map[x, y] = (int)TileType.WALL;
                else if (neighbourWallTiles < 4) // 아니라면 그 타일은 바닥
                    map[x, y] = (int)TileType.FLOOR;
            }
        }
    }

    /// <summary>
    /// 장식바닥 타일 다듬는 함수
    /// </summary>
    private void SmoothDeco()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (GetSurroundingTileCount(x, y, TileType.WALL) < 1)
                {
                    int neighbourWallTiles = GetSurroundingTileCount(x, y, TileType.FLOOR);
                    if (neighbourWallTiles > 4) // 주변 8칸중 바닥이 4개보다 많으면 벽으로 전환
                        map[x, y] = (int)TileType.FLOOR;
                    else if (neighbourWallTiles < 4) // 아니라면 그 타일은 바닥
                        map[x, y] = (int)TileType.GRASS;
                }
            }
        }
    }

    /// <summary>
    /// 주변의 타일을 계산하는 함수
    /// </summary>
    private int GetSurroundingTileCount(int gridX, int gridY, TileType tiletype)
    {
        int tileCount = 0;
        // 선택된 지점의 주변 8칸 검사
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                // outofRange 예외처리, 검사 대상이 0보다는 크고, 범위보다는 작은지 검사
                if (IsInMapRange(neighbourX, neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY) // 자기 자신은 제외
                        if (map[neighbourX, neighbourY] == (int)tiletype) // 주변 타일이 벽이면 갯수 추가
                            tileCount++;
                }
                else
                    tileCount++;
            }
        }
        return tileCount;
    }

    /// <summary>
    /// 맵 정보를 토대로 타일 출력
    /// </summary>
    public void DrawTile()
    {
        wallTileMap.ClearAllTiles();
        floorTileMap.ClearAllTiles();
        grassTileMap.ClearAllTiles();

        foreach (GameObject deco in decoObj)
            Destroy(deco);
        decoObj.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int pos = new Vector3Int((int)(-width / 2 + x), (int)(-height / 2 + y), 0);
                switch (map[x, y])
                {
                    case (int)TileType.FLOOR:
                        floorTileMap.SetTile(pos, floorTile);
                        break;
                    case (int)TileType.WALL:
                        wallTileMap.SetTile(pos, wallTile[UnityEngine.Random.Range(0, wallTile.Length)]);

                        // 벽에는 장식 조금 추가
                        if (UnityEngine.Random.Range(0f, 1f) < 0.08f)
                        {
                            GameObject obj = Instantiate(decoPrefab[UnityEngine.Random.Range(0, decoPrefab.Length)], objList);
                            obj.transform.position = pos;
                            decoObj.Add(obj);
                        }
                        break;
                    case (int)TileType.GRASS:
                        grassTileMap.SetTile(pos, grassTile[UnityEngine.Random.Range(0, grassTile.Length)]);
                        break;
                    case (int)TileType.NORMAL_COLLECT:
                        collectableItemTileMap.SetTile(pos, collectingTile[0]);
                        break;
                    case (int)TileType.RARE_COLLECT:
                        collectableItemTileMap.SetTile(pos, collectingTile[1]);
                        break;
                    case (int)TileType.EPIC_COLLECT:
                        collectableItemTileMap.SetTile(pos, collectingTile[2]);
                        break;
                    case (int)TileType.LEGEND_COLLECT:
                        collectableItemTileMap.SetTile(pos, collectingTile[3]);
                        break;
                }

            }
        }
    }

    /// <summary>
    /// 랜덤 스폰포인트 지정
    /// </summary>
    public void SetSpawnPoint()
    {
        while (true)
        {
            int randomWidth = UnityEngine.Random.Range(0, width);
            int randomHeight = UnityEngine.Random.Range(0, height);
            if (map[randomWidth, randomHeight] == 0)
            { 
                spawnPoint = new Vector3((-width / 2 + randomWidth + .5f), (-height / 2 + randomHeight) + .5f, -0.2f);
                spawnobj.transform.position = spawnPoint;
            }

            randomWidth = UnityEngine.Random.Range(0, width);
            randomHeight = UnityEngine.Random.Range(0, height);
            if (map[randomWidth, randomHeight] == 0)    // 해당 타일이 바닥일경우 멈추기
            {
                Vector3 exitPoint = new Vector3((-width / 2 + randomWidth + .5f), (-height / 2 + randomHeight) + .5f, -0.2f);
                exitObj.transform.position = exitPoint;
                break;
            }
        }
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPoint;
    }
}
