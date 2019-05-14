using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    private WeaponAnimations weaponAnimations;
    [SerializeField] private float amount;
    [SerializeField] private float maxAmount;
    [SerializeField]
    private float smoothAmount = 4f;  
    private Vector3 initialPosition;  

    void Start()
    {
        initialPosition = transform.localPosition; //position in respect to the parent (player)
        weaponAnimations = GetComponent<WeaponAnimations>();
    }

    // Update is called once per frame
    void Update()
    {
        float movementX = -Input.GetAxis(MouseAxis.MOUSE_X) * amount;
        float movementY = -Input.GetAxis(MouseAxis.MOUSE_Y) * amount;

        movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);

        
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, smoothAmount * Time.deltaTime);
        
        
    }
}
