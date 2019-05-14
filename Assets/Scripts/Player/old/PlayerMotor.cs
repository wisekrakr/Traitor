using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour{

    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 thrusterForce = Vector3.zero;

    [SerializeField]
    private float cameraRotationLimit = 85f;
    
    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    //Gets a movement vector
    public void Move(Vector3 vel)
    {
        velocity = vel;
    }

    //Gets a rotational vector
    public void Rotate(Vector3 rot)
    {
        rotation = rot;
    }

    //Gets a thruster force vector
    public void ApplyThrust(Vector3 thrust)
    {
        thrusterForce = thrust;
    }

    public void RotateCamera(float cameraRotX)
    {
        cameraRotationX = cameraRotX;
    }

    //Run every physics iteration
    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    //Perform rotation 
    void PerformRotation()
    {
        rigidBody.MoveRotation(rigidBody.rotation * Quaternion.Euler(rotation));

        if (cam != null)
        {
            //Set our rotation and clamp it
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            //Apply the rotation to the transform of the camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }

    }

    //Perform movement based on velocity variable
    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);
        }

        if (thrusterForce != Vector3.zero)
        {
            rigidBody.AddForce(thrusterForce * Time.deltaTime, ForceMode.Acceleration);
        }
    }
   
}
