using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudio : MonoBehaviour
{
   
    public AudioSource audioSource;
    public AudioClip shootSound, reloadSound;

    //Play a shoot sound until the end. No sudden drops, a full sound clip.
    public void PlayShootSound()
    {
        audioSource.PlayOneShot(shootSound);
        audioSource.volume = 0.2f;       
    }

    //Play a reload sound until the end. No sudden drops, a full sound clip.
    public void PlayReloadSound()
    {
        audioSource.PlayOneShot(reloadSound);
        audioSource.volume = 0.2f;       
    }

    
}
