
using UnityEngine;

public struct Cell 
{
    public enum Type
    {
        Empty,//비어있는 공간->흙
        Plant,//--식물의 뿌리가 생성
        Number,//--지뢰찾기의 뿌리의 위치를 알려줄 번호 추구 변경예정
    }

    [SerializeField] public Vector3Int position;
    [SerializeField] public Type type;
    [SerializeField] public int number;
    [SerializeField] public bool revealed; //--사용자가 상호작용하여 확인을 했는지 않했는지 판별
    [SerializeField] public int cutCount; //-- 식물의 뿌리가 몇번 정도 잘렸는지 카운트
}
