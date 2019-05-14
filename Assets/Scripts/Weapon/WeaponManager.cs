using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponHandler[] weapons;

    private int currentWeaponSelected;
   
    void Start()
    {
        currentWeaponSelected = 0;
        weapons[currentWeaponSelected].gameObject.SetActive(true); //we need the gameobject because we are searching for the script (WeaponHandler) of the weapon
    }

    void Update()
    {
        WeaponSelecting();
    }

    //Turns the current selected off and turns a new weapon on
    private void TurnOnSelectedWeapon(int weaponSelected)
    {
        if (currentWeaponSelected == weaponSelected) return;

        //turn off current weapon
        weapons[currentWeaponSelected].gameObject.SetActive(false); //we need the gameobject because we are searching for the script (WeaponHandler) of the weapon
        //turn on new weapon
        weapons[weaponSelected].gameObject.SetActive(true); //we need the gameobject because we are searching for the script (WeaponHandler) of the weapon
        //store current selected weapon index
        currentWeaponSelected = weaponSelected;
    }

    //Numeric key that corresponds to the WeaponHandler index to select a weapon
    private void WeaponSelecting()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TurnOnSelectedWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TurnOnSelectedWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TurnOnSelectedWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TurnOnSelectedWeapon(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TurnOnSelectedWeapon(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            TurnOnSelectedWeapon(5);
        }
    }

    //Returns the current selected weapon
    public WeaponHandler GetCurrentSelectedWeapon()
    {
        return weapons[currentWeaponSelected];
    }
}
