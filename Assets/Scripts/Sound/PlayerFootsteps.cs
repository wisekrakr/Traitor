using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    private AudioSource footstepSound;

    [SerializeField] private AudioClip[] footstepClips;
    
    private CharacterController characterController;
    
    [HideInInspector] public float volumeMin, volumeMax;

    private float accumulatedDistance;
    [HideInInspector] public float stepDistance;    

    // Start is called before the first frame update
    void Awake()
    {
        footstepSound = GetComponent<AudioSource>();
        characterController = GetComponentInParent<CharacterController>();        
    }
        
    // Update is called once per frame
    void Update()
    {        
        FootStepSound();        
    }

    void FootStepSound()
    {
        if (!characterController.isGrounded)
            return;

        if(characterController.velocity.sqrMagnitude > 0)
        {                        
            //the distance of how far we have gone. The movement until we play the next footstep sound (stepDistance).
            accumulatedDistance += Time.deltaTime;

            if (accumulatedDistance > stepDistance)
            {
                footstepSound.volume = Random.Range(volumeMin, volumeMax);
                footstepSound.clip = footstepClips[Random.Range(0, footstepClips.Length)];
                footstepSound.Play();
               
                accumulatedDistance = 0f;
            }
        }
        else
        {
            accumulatedDistance = 0f;
        }
    }    
}
