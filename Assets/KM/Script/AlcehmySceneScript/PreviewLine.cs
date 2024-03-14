using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PreviewLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    public void DrawLine(Vector3 startPos, Vector3 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos); 
    }
}
