using UnityEngine;

public class MouseView : MonoBehaviour
{
    [SerializeField] private Transform playerRotation, viewRotation;
    [SerializeField] private bool invert;
    [SerializeField] private bool unlockCursor = true;
    [SerializeField] private float sensitivity = 5f;
    [SerializeField] private int smoothSteps = 10;
    [SerializeField] private float smoothWeight = 0.4f;
    [SerializeField] private float rollAngle = 0f;
    [SerializeField] private float rollSpeed = 3f;
    [SerializeField] private Vector2 defaultViewLimits = new Vector2(-70f, 80);

    private Vector2 viewAngles;
    private Vector2 currentMouseView;
    private Vector2 smoothMove;
    private float currentRollAngle;
    private int lastViewFrame;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        LockAndUnlockCursor();
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            LookAround();
        }
    }

    //If we hit the escape button we unlock or lock the cursor
    void LockAndUnlockCursor(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    //Handles looking around with the mouse. 
    void LookAround()
    {
        currentMouseView = new Vector2(Input.GetAxis(MouseAxis.MOUSE_Y), Input.GetAxis(MouseAxis.MOUSE_X));

        viewAngles.x += currentMouseView.x * sensitivity * (invert ? 1f : -1f);
        viewAngles.y += currentMouseView.y * sensitivity;
        
        viewAngles.x = Mathf.Clamp(viewAngles.x, defaultViewLimits.x, defaultViewLimits.y); //don't rotate around own axis

        currentRollAngle = Mathf.Lerp( //"drunk" movement
            currentRollAngle, 
            Input.GetAxisRaw(MouseAxis.MOUSE_X) * rollAngle, 
            rollSpeed * Time.deltaTime
            );

        viewRotation.localRotation = Quaternion.Euler(viewAngles.x, 0f, currentRollAngle);
        playerRotation.localRotation = Quaternion.Euler(0f, viewAngles.y, 0f);
    }
}
