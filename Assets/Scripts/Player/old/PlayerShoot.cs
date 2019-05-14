using UnityEngine;

public class PlayerShoot : MonoBehaviour{

    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask mask;

    private void Start()
    {
        if (cam == null)
        {
            Debug.Log("PlayerShoot: No camera");
            this.enabled = false;
        }
    }

    private void Update()
    {

        if (Input.GetButton("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            Debug.Log("Hit: " + hit.collider.name);
        }
    }
}
