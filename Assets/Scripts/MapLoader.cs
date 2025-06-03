using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public GameObject map1Prefab;
    public GameObject map2Prefab;
    public GameObject map3Prefab;

    public Transform mapRoot; // 👈 This will be set at runtime

    public TopViewCameraController cameraController; // Assign in Inspector

    void Start()
    {
        int selectedMap = PlayerPrefs.GetInt("SelectedMap", 1); // Default: 1

        GameObject loadedMap = null;

        switch (selectedMap)
        {
            case 1:
                loadedMap = Instantiate(map1Prefab, new Vector3(3.06f, 0.01f, 1.512f), Quaternion.Euler(-90f, 0f, -90f));
                break;
            case 2:
                loadedMap = Instantiate(map2Prefab, new Vector3(3.06f, 0.01f, 2.323f), Quaternion.Euler(-90f, 0f, -90f));
                break;
            case 3:
                loadedMap = Instantiate(map3Prefab, new Vector3(3.06f, 0.01f, 1.512f), Quaternion.Euler(-90f, 0f, -90f));
                break;
        }

        if (loadedMap != null)
        {
            mapRoot = loadedMap.transform;

            // 🔁 Assign map root to camera controller if needed
            if (cameraController != null)
                cameraController.mapRoot = mapRoot;
        }
    }
}
