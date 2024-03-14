using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelIngredient : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Use Item Cancel");
        Destroy(collision.gameObject);
    }
}
