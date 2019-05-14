using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAimDownSights : MonoBehaviour
{
    private float cameraFOV;
    private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        cameraFOV = Camera.main.fieldOfView;
        mainCam = Camera.main;
    }    

    public void ZoomIn(int fov, float adsSpeed)
    {
        mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, fov, adsSpeed * Time.deltaTime);
    }
    public void ZoomOut(float adsSpeed)
    {
        mainCam.fieldOfView = cameraFOV;
    }    
}
