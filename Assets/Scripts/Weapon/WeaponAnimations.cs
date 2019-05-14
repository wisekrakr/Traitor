using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimations : MonoBehaviour
{
    public Animator animator;

    public GameObject hitParticles;
    public GameObject bulletImpact;
    public ParticleSystem muzzleFlash;

    [HideInInspector]
    public bool isReloading;
    [HideInInspector]
    public bool isAiming;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
       

    private void FixedUpdate()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        //isReloading = info.IsName(AnimationTags.RELOAD_PARAMETER);
        animator.SetBool("Aim", isAiming);
        //isAiming = info.IsName("Aim");
    }

    public void Aim(bool canAim)
    {
        animator.SetBool(AnimationTags.AIM_PARAMETER, canAim);
        isAiming = canAim;
    }

    public void PlayShootAnimation()
    {
        animator.CrossFadeInFixedTime(AnimationTags.ATTACK_TRIGGER, 0.05f); //Shoot animation
        //animator.SetTrigger(AnimationTags.SHOOT_TRIGGER); //name of the trigger in the animator              
    }

    //Play Muzzle flash animation
    public void PlayMuzzleFlash()
    {
        muzzleFlash.Play();        
    }    

    //Animate reloading. If the animation is going, don't animate again.
    public void PlayReloadAnimation()
    {
        if (isReloading) return; //if we already are reloading, don't reload 
        animator.CrossFadeInFixedTime(AnimationTags.RELOAD_PARAMETER, 0.01f);
    }

    //Adds spark particles to a surface that the bullet hits (via RaycastHit)
    public void AddHitParticle(RaycastHit hit)
    {
        //Returns particle effect 90 degrees from where it hits
        GameObject hitParticlesEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        hitParticlesEffect.transform.SetParent(hit.transform);
    }

    //Adds bullethole particles to a surface that the bullet hits (via RaycastHit)
    public void AddBulletHoleParticle(RaycastHit hit)
    {
        //Returns bullethole effect perpendicular to the surface it hits
        GameObject bulletHoleEffect = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
        bulletHoleEffect.transform.SetParent(hit.transform);
    }

}
