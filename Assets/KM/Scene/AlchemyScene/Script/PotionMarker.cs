using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionMarker : MonoBehaviour
{
    // 원본 포션 데이터
    // 포션이 추가되면 원본 데이터를 추가할 것
    [SerializeField] private Sprite potionImg;
    public SpriteRenderer markerImg;

    private void Awake()
    {
        markerImg = gameObject.GetComponent<SpriteRenderer>();
        markerImg.sprite = potionImg;
    }

    /// <summary>
    /// 실제 포션 마커를 움직이는 코드
    /// </summary>
    public IEnumerator MovePotionCorutine(Vector3 startPos, Vector3 endPos)
    {
        float elapsedTime = 0.0f;
        float distance = Vector3.Distance(startPos, endPos) * 7.0f; // 시작점과 목적지 간의 거리 계산

        while (elapsedTime < distance) // 이동에 걸리는 시간을 기준으로 반복
        { 
            Vector3 newPosition = Vector3.Lerp(startPos, endPos, elapsedTime / distance);
            transform.localPosition = newPosition;
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        transform.localPosition = endPos; // 이동이 완료된 후 목적지에 정확히 위치하도록 보정
    }

    private void OnTriggerEnter2D(Collider2D collision) // 포션이 효과를 얻었을때
    {

    }
}