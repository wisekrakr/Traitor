using UnityEngine;



public class WeaponHandler : MonoBehaviour   
{    
    private WeaponAnimations weaponAnimations;
    private WeaponAudio weaponAudio;
    private WeaponManager weaponManager;
    private WeaponAimDownSights weaponADS;

    [Header("Weapon Ammo Stats")]
    [SerializeField] private int bulletsPerMag = 30; //bullets per magazine
    [SerializeField] private int bulletsLeft = 200; //total number of bullets left
    [SerializeField] private int currentBullets; //bullets currently in magazine    

    [Header("Weapon Ammo Specs")]
    [SerializeField] private WeaponSpecs.WeaponType weaponType; //a single fire mode or a auto fire mode
    [SerializeField] private WeaponSpecs.ShootMode shootMode; //a single fire mode or a auto fire mode
    [SerializeField] private float fireRate = 0.2f; //delay between each bullet that is fired
    private float fireInterval; //time counter for the delay between bullets fired
    [SerializeField] private float range = 100f; //maximum range of a bullet         
    [SerializeField] private float damage = 20f; //damage given per projectile    
    [SerializeField] private WeaponSpecs.WeaponAmmoType ammoType; // what ammo this weapon uses

    [Header("Weapon Positioning and Aiming")]
    [SerializeField] private Transform shootPoint; //the point from which the bullet will travel    
    [SerializeField] private GameObject attackPointMelee; //determines if we hit something with a melee weapon
    private Vector3 initialPosition; //initial position of the weapon
    private Vector3 initialRotation; //initial rotation of the weapon    
    [SerializeField] private WeaponSpecs.AimMode aimMode; // what way are we aiming
    [SerializeField] private Vector3 aimPosition; //calculated position for the weapon when we are aiming
    [SerializeField] private Vector3 aimRotation; //calculated rotation for the weapon when we are aiming
    [SerializeField] private float adsSpeed = 15f; //the speed of the animation in which we aim    

    private bool shootInput;
    private GameObject crosshair;

    private void Awake()
    {
        currentBullets = bulletsPerMag;

        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);
    }

    void Start()
    {        
        initialPosition = transform.localPosition; //position based on the parent not the world 
        initialRotation = transform.localRotation.eulerAngles;        

        weaponAnimations = GetComponent<WeaponAnimations>();
        weaponAudio = GetComponent<WeaponAudio>();
        weaponManager = GetComponent<WeaponManager>();
        weaponADS = GetComponent<WeaponAimDownSights>();
    }

    void Update()
    {        
        //Fire weapon
        Shooting();

        //Aim down sight of weapon
        AimDownSights();

        // Reload weapon
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(currentBullets < bulletsPerMag && bulletsLeft > 0)
                weaponAnimations.PlayReloadAnimation();
        }
        
        if(fireInterval < fireRate)
        {
            fireInterval += Time.deltaTime;
        }
    }    

    private bool CanFire()
    {
        if (fireInterval < fireRate || currentBullets <= 0 || weaponAnimations.isReloading)
        {
            return true;
        }
        return false;
    }

    private void Fire()
    {
        if (CanFire()) return; // not the time to shoot yet or no more bullets left

        RaycastHit hit;

        //Position of the point of shooting, shooting on the z axis, whatever we hit will be stored in "hit"
        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range)) 
        {
            Debug.Log("Shooting: " + hit.transform.name);
            //Returns particle effect 90 degrees from where it hits
            weaponAnimations.AddHitParticle(hit);
            //Returns bullethole effect perpendicular to the surface it hits
            weaponAnimations.AddBulletHoleParticle(hit);
            //Applies damage to the health of the Game Object that gets hit
            if (hit.transform.GetComponent<HealthController>())
            {
                hit.transform.GetComponent<HealthController>().ApplyDamage(damage);
            }
        }        
        weaponAnimations.PlayShootAnimation();
        weaponAnimations.PlayMuzzleFlash();
        weaponAudio.PlayShootSound();

        currentBullets--;
        fireInterval = 0f; //reset interval

    }

    //Function that handles the Fire method, automatic reloading and the way of firing a weapon
    private void Shooting()
    {

        if (ammoType == WeaponSpecs.WeaponAmmoType.BULLET)
        {
            switch (shootMode)
            {
                case WeaponSpecs.ShootMode.AUTO:
                    shootInput = Input.GetButton(MouseButtons.MB1);
                    break;
                case WeaponSpecs.ShootMode.SINGLE:
                    shootInput = Input.GetButtonDown(MouseButtons.MB1);
                    break;
                default:
                    Debug.Log("No Shoot Mode Selected");
                    break;
            }

            if (shootInput)
            {
                if (currentBullets > 0)
                    Fire(); //execute Fire method if we press/hold the left mouse button      
                else if (bulletsLeft > 0)
                    weaponAnimations.PlayReloadAnimation();
            }
        }else if (weaponType == WeaponSpecs.WeaponType.SPEAR)
        {
            if (weaponAnimations.isAiming && Input.GetButton(MouseButtons.MB1)) //only possible for self aiming weapons
            {
                weaponAnimations.PlayShootAnimation();                
            }
        }
        else
        {
            if (Input.GetButtonDown(MouseButtons.MB1))
            {
                weaponAnimations.PlayShootAnimation();
            }
        }           
    }

    //AimDownSights function to slightly zoom and position the weapon perpendicular to the player (Right mouse button)
    private void AimDownSights()
    {
        if (aimMode == WeaponSpecs.AimMode.AIM)
        {
            if (Input.GetButton(MouseButtons.MB2) && !weaponAnimations.isReloading)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, adsSpeed * Time.deltaTime);
                transform.localRotation.SetFromToRotation(transform.localRotation.eulerAngles, aimRotation);

                //crosshair.SetActive(false);

                if (weaponType == WeaponSpecs.WeaponType.LONG_RANGE_RIFLE)
                {
                    weaponADS.ZoomIn(30, adsSpeed);
                }
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, adsSpeed * Time.deltaTime);
                transform.localRotation.SetFromToRotation(transform.localRotation.eulerAngles, initialRotation);
                
                weaponADS.ZoomOut(adsSpeed);
                crosshair.SetActive(true);
            }
        }else if(aimMode == WeaponSpecs.AimMode.SELF_AIM) //for weapons that aim themselves
        {
            if (Input.GetButton(MouseButtons.MB2) && !weaponAnimations.isReloading)
            {
                weaponAnimations.Aim(true);
            }
            else
            {
                weaponAnimations.Aim(false);
            }
        }
        
    }    

    //Reload function if there are bullets left and deduct the reloaded bullets from bullets that are left (implemented in ReloadState)
    public void Reload()
    {
        if (bulletsLeft <= 0) return; //if we have no bullets left, do nothing

        int bulletsToLoad = bulletsPerMag - currentBullets;
        int bulletToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletToDeduct;
        currentBullets += bulletToDeduct;
    }


    private void TurnOnAttackPoint()
    {
        attackPointMelee.SetActive(true);
    }
    private void TurnOffAttackPoint()
    {
        if (attackPointMelee.activeInHierarchy)
        {
            attackPointMelee.SetActive(false);
        }
    }

}
