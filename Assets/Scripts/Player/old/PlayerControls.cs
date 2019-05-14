using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerControls : MonoBehaviour{

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float thrusterForce = 2500f;
    [SerializeField]
    private float lookSensitivity = 2f;

    [Header("Spring settings:")]    
    [SerializeField]
    private float jointSpring = 40f;
    [SerializeField]
    private float jointMaxForce = 40f;


    private PlayerMotor motor;
    private ConfigurableJoint joint;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();

        SetJointSettings(jointSpring);
    }

    private void Update()
    {
        //Calculate movement velocity as a 3D Vector
        float velX = Input.GetAxisRaw("Horizontal");
        float velZ = Input.GetAxisRaw("Vertical");

        Vector3 horizontalVel = transform.right * velX;
        Vector3 verticalVel = transform.forward * velZ;

        //Final movement vector
        Vector3 velocity = (horizontalVel + verticalVel).normalized * speed;

        //Apply movement
        motor.Move(velocity);

        //Calculate rotation as a 3D vector
        float rotationY = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0f, rotationY, 0f) * lookSensitivity;

        //Apply rotation
        motor.Rotate(rotation);

        //Calculate camera rotation as a 3D vector
        float rotationX = Input.GetAxisRaw("Mouse Y");

        float cameraRotationX = rotationX * lookSensitivity;

        //Apply rotation
        motor.RotateCamera(cameraRotationX);

        //Calculate thrusterForce based on player input
        Vector3 newThrusterForce = Vector3.zero;

        if (Input.GetButton("Jump"))
        {
            newThrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        }
        else
        {
            SetJointSettings(jointSpring);
        }

        //Apply thruster force
        motor.ApplyThrust(newThrusterForce);
    }

    private void SetJointSettings(float spring)
    {
        joint.yDrive = new JointDrive
        {
            positionSpring = spring,
            maximumForce = jointMaxForce
        };
    }

}
