using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int x, y;
    [SerializeField]
    private SpriteRenderer renderer;

    // 타일 좌표 초기화
    public void TileInit(int x, int y, Sprite image)
    {
        this.x = x;
        this.y = y;

        renderer = this.GetComponent<SpriteRenderer>();
        renderer.sprite = image;
    }

    public void TileSelected()
    {
        renderer.color = Color.grey;
    }

    public void TileUnSelected()
    {
        renderer.color = Color.white;
    }

    public void TileSelect()
    {
        GridManager.instance.TileSelect(this.gameObject);
    }

    private void OnMouseDown()
    {
        TileSelected();
        this.TileSelect();
    }
}
