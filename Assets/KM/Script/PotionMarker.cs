using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionMarker : MonoBehaviour
{
    [SerializeField] private Potion potion;

    public void MovePotion(Vector3 dir)
    {
        gameObject.transform.position += dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Get Option");
    }
}
