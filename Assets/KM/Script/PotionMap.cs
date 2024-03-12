using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionMap : MonoBehaviour
{
    // 확대, 축소

    // 포션 옵션의 위치
    [SerializeField] private List<Vector3> optionPoint = new List<Vector3>();
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private Transform potionPosition;
    public Transform PotionPosition { get { return potionPosition; } }

    private void Start()
    {
        MapInit();
    }

    private void MapInit()
    {
        Vector3 tempPos = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), -0.1f);
        optionPoint.Add(tempPos);

        // 옵션 위치 생성
        foreach (Vector3 options in optionPoint)
        {
            GameObject tempobj = Instantiate(markerPrefab, gameObject.transform);
            tempobj.transform.position += options * 10.0f;
        }
    }
}