using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelIngredient : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int key = collision.gameObject.GetComponent<SelectItem>().GetIteminfo;
        Destroy(collision.gameObject);
    }
}
