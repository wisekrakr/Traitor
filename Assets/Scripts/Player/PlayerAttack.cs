using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weaponManager;
    private WeaponHandler weaponController;


    private void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();
        weaponController = GetComponent<WeaponHandler>();
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
