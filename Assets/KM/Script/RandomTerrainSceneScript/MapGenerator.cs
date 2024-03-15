using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewBehaviourScript : MonoBehaviour
{
    // 맵의 크기
    public int width;           
    public int height;

    public string seed;         // 맵 랜덤생성 시드 지정
    public bool useRandomSeed;  // 무작위 생성 체크용 변수

    // 지형의 랜덤률(?) 결정
    [Range(0,100)]
    public int randomFillPercent;

    int[,] map;

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

        for (int i = 0; i < 5; i++)
            SmoothMap();
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
                if (neighbourWallTiles > 4) // 주변 8칸중 막힌 공간이 4개 이상이면 벽으로 전환
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
                if (neighbourX >= 0 && neighbourX < width &&
                    neighbourY >= 0 && neighbourY < height)
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
    private void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
