using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 600f;
    
    private float movement = 0;

    private void Update()
    {
        movement = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        transform.RotateAround(Vector3.zero, Vector3.forward, movement * Time.fixedDeltaTime * -moveSpeed);
    }
}