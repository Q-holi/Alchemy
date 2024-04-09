
using UnityEngine;

public struct Cell 
{
    public enum Type
    {
        Invalid,
        Empty,//비어있는 공간->흙
        Center,// 중앙의 식물이 차지하는 공간
        StartPlant,//--식물의 뿌리 시작점
        Plant,//--식물의 뿌리
        Number,//--지뢰찾기의 뿌리의 위치를 알려줄 번호 추구 변경예정
        CUT,//--깊게 파기로 인한 뿌리 짦림
    }

    [SerializeField] public Vector3Int position;
    [SerializeField] public Type type;
    [SerializeField] public int number;
    [SerializeField] public bool isRevealed; //--사용자가 상호작용하여 확인을 했는지 않했는지 판별
    [SerializeField] public int cutCount; //-- 식물의 뿌리가 몇번 정도 잘렸는지 카운트
    [SerializeField] public Vector2Int prevCell;
    [SerializeField] public Vector2Int nextCell;

    public static Vector2Int CalcDirection(Vector2Int target, Vector2Int compare)
    {
        if (target.y - compare.y < 0)
        {
            return Vector2Int.up;
        }
        else if (target.y - compare.y > 0)
        {
            return Vector2Int.down;
        }
        else if (target.y - compare.y == 0)
        {
            if (target.x - compare.x < 0)
            {
                return Vector2Int.left;
            }
            else if (target.x - compare.x > 0)
            {
                return Vector2Int.right;
            }
        }

        return Vector2Int.zero;
    }
}

