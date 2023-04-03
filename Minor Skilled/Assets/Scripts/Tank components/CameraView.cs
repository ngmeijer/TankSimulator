using UnityEngine;

public class CameraView : MonoBehaviour
{
    public Camera ViewCam;

    public virtual void SetView(bool enabled)
    {
        ViewCam.gameObject.SetActive(enabled);
    }
}