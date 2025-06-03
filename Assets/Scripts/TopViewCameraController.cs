using UnityEngine;

public class TopViewCameraController : MonoBehaviour
{
    public Camera topViewCamera;
    public Transform mapRoot; // 🧠 Set dynamically from MapLoader
    public float padding = 0.5f;

    void Start()
    {
        if (topViewCamera.orthographic && mapRoot != null)
        {
            Bounds bounds = CalculateBounds(mapRoot);
            FitCameraToBounds(bounds);
        }
    }

    Bounds CalculateBounds(Transform root)
    {
        Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
        Bounds bounds = renderers[0].bounds;

        foreach (Renderer rend in renderers)
        {
            bounds.Encapsulate(rend.bounds);
        }

        return bounds;
    }

    void FitCameraToBounds(Bounds bounds)
    {
        float height = bounds.size.z + padding;
        float width = bounds.size.x + padding;

        float aspect = topViewCamera.aspect;
        float sizeByHeight = height / 2f;
        float sizeByWidth = width / (2f * aspect);

        topViewCamera.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth);
        topViewCamera.transform.position = new Vector3(bounds.center.x, topViewCamera.transform.position.y, bounds.center.z);
    }
}
