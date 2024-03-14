using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PreviewLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private PotionMarker potionMarker;

    public void LinePreview(Collection baseItem)
    {
        gameObject.SetActive(true);

        Vector3 markerPoint = potionMarker.gameObject.transform.localPosition;
        Vector3 lineVector = new Vector3(baseItem.Green_Option - baseItem.Alpha_Option,
                                         baseItem.Red_Option - baseItem.Blue_Option, 0.0f);

        lineRenderer.SetPosition(0, markerPoint * 10.0f);
        lineRenderer.SetPosition(1, markerPoint * 10.0f + lineVector);
    }
}
