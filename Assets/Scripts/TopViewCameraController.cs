using UnityEngine;

public class TopViewCameraController : MonoBehaviour
{
    public Camera topViewCamera;
    public float desiredMapWidth = 3.0f;  // in meters
    public float desiredMapHeight = 2.0f;

    void Start()
    {
        if (topViewCamera.orthographic)
        {
            FitFullMap(topViewCamera, desiredMapWidth, desiredMapHeight);
        }
    }
    void FitFullMap(Camera cam, float mapWidth, float mapHeight)
    {
        float aspect = cam.aspect;
        float sizeByHeight = mapHeight / 2f;
        float sizeByWidth = mapWidth / (2f * aspect);
        cam.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth);
    }

}
