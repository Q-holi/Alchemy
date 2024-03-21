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
        EventHandler.OnMouse += LinePreview;
    }

    private void OnDestroy()
    {
        EventHandler.OnMouse -= LinePreview;
    }

    public void LinePreview(Item baseItem)
    {
        gameObject.SetActive(true);
        Collection temp = (Collection)baseItem;

        Vector3 markerPoint = potionMarker.gameObject.transform.localPosition;
        Vector3 lineVector = new Vector3(temp.options.x - temp.options.z,
                                         temp.options.y - temp.options.w, -0.1f);

        lineRenderer.SetPosition(0, markerPoint * 10.0f);
        lineRenderer.SetPosition(1, markerPoint * 10.0f + lineVector);
    }
}
