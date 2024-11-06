using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float speed = 100;
    private float MaxSpeed = 100;
    private float Drag = 0.98f;
    private float turnSpeed = 10;
    private float horizontalInput;
    private float forwardInput;
    private Vector3 MoveForce;
    private float Traction = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        MoveForce += transform.forward * speed * Time.deltaTime * forwardInput;
        transform.position += MoveForce * Time.deltaTime;

        // Drag
        MoveForce *= Drag;
        MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);

        // Turning
        transform.Rotate(Vector3.up, turnSpeed * MoveForce.magnitude * horizontalInput * Time.deltaTime);

        // Traction
        Debug.DrawRay(transform.position, MoveForce.normalized * 3);
        Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
        MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, Traction * Time.deltaTime) * MoveForce.magnitude;
    }
}
