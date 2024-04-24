using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollower : MonoBehaviour
{
    private Vector3 followerPos;

    private void OnEnable()
    {
        followerPos = Player.Instance.gameObject.transform.position;
    }

    private void Update()
    {
        this.gameObject.transform.position = followerPos;
    }
}
