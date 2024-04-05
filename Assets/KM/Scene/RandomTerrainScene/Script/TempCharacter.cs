using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCharacter : MonoBehaviour
{
    public CharacterController controller;

    public float moveSpeed = 5f;

    void Update()
    {
        float horizonInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizonInput * moveSpeed, verticalInput * moveSpeed, 0);
        move = transform.TransformDirection(move); // 로컬 좌표계를 전역 좌표계로 변환

        controller.Move(move * Time.deltaTime);
    }
}