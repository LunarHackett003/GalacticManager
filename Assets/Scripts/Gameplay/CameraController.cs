using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform CamTransform;
    public bool useMainCam;

    private void Awake()
    {
        if (useMainCam)
        {
            CamTransform = Camera.main.transform;
        }
    }


    private void LateUpdate()
    {
        
    }

}
