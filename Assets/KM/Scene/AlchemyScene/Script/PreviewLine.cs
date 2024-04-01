using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PreviewLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private PotionMarker potionMarker;

    private void Start()
    {
        InventoryEventHandler.OnMouse += LinePreview;
    }

    private void OnDestroy()
    {
        InventoryEventHandler.OnMouse -= LinePreview;
    }

    public void LinePreview(ItemDetails item)
    {
        gameObject.SetActive(true);

        Vector3 markerPoint = potionMarker.gameObject.transform.localPosition;          // 포션 마커의 위치
        Vector3 lineVector = new Vector3(item.itemOption[0] - item.itemOption[2],
                                         item.itemOption[1] - item.itemOption[3], -0.1f);       // 목적지까지의 거리

        // 포션 맵의 스케일에 맞춰야 하기 때문에 10을 곱해줌
        lineRenderer.SetPosition(0, markerPoint * 10.0f);
        lineRenderer.SetPosition(1, markerPoint * 10.0f + lineVector);
    }
}
