using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GridManager : MonoBehaviour
{
    public static GridManager instance; // 싱글톤

    [SerializeField]
    private SpriteAtlas tileImage;  // 타일에 쓰이는 이미지
    [SerializeField]
    private int row, column;        // 타일 크기
    private List<List<GameObject>> tiles = new List<List<GameObject>>();    // 타일 리스트

    public GameObject selectTile;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        TilesInitialize();  
    }

    private void TilesInitialize()     // 타일 초기화
    {
        for (int i = 0; i < row; i++)
        {
            tiles.Add(new List<GameObject>());
            for (int j = 0; j < column; j++)
            {
                GameObject temp = Resources.Load<GameObject>("Tile");
                temp.GetComponent<Tile>().TileInit(j, i, tileImage.GetSprite(Random.Range(0, 7).ToString()));
                temp.transform.position = new Vector3(j , i , 0);
                tiles[i].Add(Instantiate(temp, this.transform));
            }
        }
    }

    public void TileSelect(GameObject tile) // 타일 선택 시
    {
        if (selectTile == null)
        {
            selectTile = tile;
            return;
        }

        if (selectTile)
        {
            if (selectTile == tile)
                return;
            if (CheckDistance(selectTile.GetComponent<Tile>(), tile.GetComponent<Tile>()))
            {
                tile.GetComponent<Tile>().TileUnSelected();
                selectTile.GetComponent<Tile>().TileUnSelected();
                SwapTile(selectTile.GetComponent<Tile>(), tile.GetComponent<Tile>());
                selectTile = null;
            }
            else
            {
                selectTile.GetComponent<Tile>().TileUnSelected();
                selectTile = tile;
                tile.GetComponent<Tile>().TileSelected();
                return;
            }
        }
    }

    public bool CheckDistance(Tile alreadySelect, Tile nowSelect)
    {
        if (Mathf.Abs(alreadySelect.x - nowSelect.x) == 0)
        {
            if (Mathf.Abs(alreadySelect.y - nowSelect.y) == 1)
                return true;
        }
        else if (Mathf.Abs(alreadySelect.y - nowSelect.y) == 0)
        {
            if (Mathf.Abs(alreadySelect.x - nowSelect.x) == 1)
                return true;
        }

        return false;
    }

    public void SwapTile(Tile alreadySelect, Tile nowSelect)
    {
        GameObject Tile1 = tiles[alreadySelect.y][alreadySelect.x];
        SpriteRenderer sprite1 = Tile1.GetComponent<SpriteRenderer>();

        GameObject Tile2 = tiles[nowSelect.y][nowSelect.x];
        SpriteRenderer sprite2 = Tile2.GetComponent<SpriteRenderer>();

        Sprite temp = sprite1.sprite;
        sprite1.sprite = sprite2.sprite;
        sprite2.sprite = temp;
    }
}
