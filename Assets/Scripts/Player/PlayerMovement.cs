using UnityEngine;

public class PlayerMovement : MonoBehaviour{

    private CharacterController characterController;

    private Vector3 moveDirection;

    [SerializeField] private float speed = 5f;
    private float moveSpeed;
    private float gravity = 15f;
    [SerializeField] private float jumpForce = 10f;    
    private float verticalVelocity;
    [SerializeField] private float sprintSpeed = 10f;
    private float crouchSpeed = 2f;

    private Transform viewRotation;
    private float standHeight = 1.6f;
    private float crouchHeight = 1f;
        
    private PlayerFootsteps playerFootsteps;
    private float sprintVolume = 1f;
    private float crouchVolume = 0.1f;
    private float walkVolumeMin = 0.2f, walkVolumeMax = 0.6f;
    private float walkStepDistance = 0.4f;
    private float sprintStepDistance = 0.25f;
    private float crouchStepDistance = 0.5f;

    [HideInInspector]
    public bool isCrouching;
    [HideInInspector]
    public bool isSprinting;  

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerFootsteps = GetComponentInChildren<PlayerFootsteps>();

        viewRotation = transform.GetChild(0); //first index of player parent        
    }

    void Start()
    {
        moveSpeed = speed;

        playerFootsteps.volumeMin = walkVolumeMin;
        playerFootsteps.volumeMax = walkVolumeMax;
        playerFootsteps.stepDistance = walkStepDistance;
    }

    void Update()
    {
        MovePlayer();        
    }

    void MovePlayer()
    {
        moveDirection = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f, Input.GetAxis(Axis.VERTICAL));

        moveDirection = transform.TransformDirection(moveDirection); //calculate movement from local to world
        moveDirection *= speed * Time.deltaTime; //movement per frame
               
        ApplyGravity();
        Sprint();
        Crouch();

        characterController.Move(moveDirection); //add movement to controller
    }

    //Calculate the amount of gravity on the y-axis
    void ApplyGravity()
    {       
        verticalVelocity -= gravity * Time.deltaTime;

        //jump
        Jump();
       
        moveDirection.y = verticalVelocity * Time.deltaTime;
    }

    //If Space key is pressed and the player is on the ground, we can move the player on the y-axis via a jumpforce floating number
    void Jump()
    {
        if (characterController.isGrounded && Input.GetKeyDown(KeyCode.Space)){
            verticalVelocity = jumpForce;
        }
    }

    //Left-shift for sprint. Handles walking speed and sound
    void Sprint() 
    {        
        if (!isCrouching && Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = sprintSpeed;

            playerFootsteps.stepDistance = sprintStepDistance;
            playerFootsteps.volumeMin = sprintVolume;
            playerFootsteps.volumeMax = sprintVolume;

            isSprinting = true;
        }
        if (!isCrouching && Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = moveSpeed;

            playerFootsteps.stepDistance = walkStepDistance;
            playerFootsteps.volumeMin = walkVolumeMin;
            playerFootsteps.volumeMax = walkVolumeMax;

            isSprinting = false;
        }
    }

    //Left-ctrl for crouch. Position of player to crouchHeight. Handles crouch sound.
    void Crouch() 
    {
        if (!isSprinting && Input.GetKeyDown(KeyCode.LeftControl))
        {            
            viewRotation.localPosition = new Vector3(0f, crouchHeight, 0f);                     //the position in respect to the parent (player), not relative to wold space
            speed = crouchSpeed;

            playerFootsteps.stepDistance = crouchStepDistance;
            playerFootsteps.volumeMin = crouchVolume;
            playerFootsteps.volumeMax = crouchVolume;

            isCrouching = true;
            
        }
        if (!isSprinting && Input.GetKeyUp(KeyCode.LeftControl))
        {
            viewRotation.localPosition = new Vector3(0f, standHeight, 0f);                     //the position in respect to the parent (player), not relative to wold space
            speed = moveSpeed;

            playerFootsteps.stepDistance = walkStepDistance;
            playerFootsteps.volumeMin = walkVolumeMin;
            playerFootsteps.volumeMax = walkVolumeMax;

            isCrouching = false;
        }
    }
}
