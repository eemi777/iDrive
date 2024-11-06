using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2 : MonoBehaviour
{
    public Transform car;             // Reference to the car's transform
    public Vector3 offset = new Vector3(0, 5, -10);   // Offset for the camera
    public float followSpeed = 5f;    // Speed at which the camera follows
    public float lookSpeed = 10f;     // Speed at which the camera looks at the car
    public float driftLookOffset = 3f; // Offset to shift camera based on drift

    private Rigidbody carRb;

    void Start()
    {
        carRb = car.GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        // Calculate the car's forward and right velocities
        Vector3 carVelocity = carRb.velocity;
        Vector3 forwardVelocity = Vector3.Project(carVelocity, car.forward);
        Vector3 lateralVelocity = carVelocity - forwardVelocity;

        // Set the target position for the camera
        Vector3 targetPosition = car.position + car.TransformVector(offset);

        // Add lateral offset to the target position for the drift effect
        targetPosition += car.right * (lateralVelocity.magnitude / carRb.maxAngularVelocity) * driftLookOffset;

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Smoothly rotate the camera to look at the car
        Vector3 lookDirection = car.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
    }
}
