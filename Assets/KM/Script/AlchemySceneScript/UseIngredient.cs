using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseIngredient : MonoBehaviour
{
    [SerializeField] private PotionMarker potionMarker;     // 맵에 있는 포션 마커
    [SerializeField] private CaulDron caulDron;             // 가마솥

    // 사용 영역에 아이템이 들어왔을때 호출
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 아이템의 정보 불러오기
        Collection item =
            (Collection)collision.gameObject.GetComponent<SelectItem>().GetIteminfo;
        if (caulDron.UpdateContent(item)) // 가마솥 내용물 업데이트
        {
            AlchemySceneManager.instance.IngredientList.ItemUse(true ,item);
            Debug.Log("Use Item : " + item.itemData.itemName);

            // 아이템 옵션의 효과 (포션의 이동거리)
            Vector3 lineVector = new Vector3((int)item.options.x - (int)item.options.x,
                                     (int)item.options.x - (int)item.options.x, 0.0f) / 10.0f;

            // 포션 마커의 시작점
            Vector3 markerPoint = potionMarker.gameObject.transform.localPosition;
            // 아이템 옵션만큼 옮긴 목적지
            Vector3 endPoint = markerPoint + lineVector;

            StartCoroutine(potionMarker.MovePotionCorutine(markerPoint, endPoint));
        }
        else
        {
            Debug.Log("Use Item Fail");
        }

        Destroy(collision.gameObject);
    }
}
