using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PreviewLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private PotionMarker potionMarker;

    private void Update()
    {
        if (AlchemySceneManager.instance.IngredientList.SelectItem != null)
            LinePreview((Collection)AlchemySceneManager.instance.IngredientList.SelectItem);
    }

    public void LinePreview(Collection baseItem)
    {
        gameObject.SetActive(true);

        Vector3 markerPoint = potionMarker.gameObject.transform.localPosition;
        Vector3 lineVector = new Vector3(baseItem.options.x - baseItem.options.y,
                                         baseItem.options.z - baseItem.options.w, -0.1f);

        lineRenderer.SetPosition(0, markerPoint * 10.0f);
        lineRenderer.SetPosition(1, markerPoint * 10.0f + lineVector);
    }
}
