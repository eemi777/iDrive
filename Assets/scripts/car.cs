using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car : MonoBehaviour
{
    public float speed = 10f;          // Speed at which the car moves forward
    public float turnSpeed = 50f;      // Speed at which the car turns
    private float Drag = 1.98f;
    private float MaxSpeed = 50;
    private Vector3 MoveForce;

    void Update()
    {
        // Move the car forward at a constant speed
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Get player input for turning (left or right)
        float turnInput = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys

        // Apply rotation based on the input
        transform.Rotate(Vector3.up, turnInput * turnSpeed * Time.deltaTime);

        MoveForce *= Drag;
        MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);
    }
}