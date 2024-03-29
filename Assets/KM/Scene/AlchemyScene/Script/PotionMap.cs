using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionMap : MonoBehaviour
{
    // 확대, 축소 기능 추후 제작

    [SerializeField] private List<Vector3> optionPoint = new List<Vector3>();    // 포션 옵션의 위치 설정
    [SerializeField] private GameObject markerPrefab;   // 옵션 마커 프리팹

    private void Start()
    {
        MapInit();
    }

    private void MapInit()
    {
        MakeOption(15);

        // 옵션 위치 생성
        foreach (Vector3 options in optionPoint)
        {
            GameObject tempobj = Instantiate(markerPrefab, gameObject.transform);
            tempobj.transform.position += options * 10.0f;
            tempobj.GetComponent<SpriteRenderer>().color =
                new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f));
            //tempobj.GetComponent<MarkerOption>().potionData = potionDB[Random.Range(0, potionDB.Count)];
        }
    }

    private void MakeOption(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 tempPos = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), -0.1f);
            optionPoint.Add(tempPos);
        }
    }
}