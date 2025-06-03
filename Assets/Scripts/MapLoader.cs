using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject map1Prefab;
    public GameObject map2Prefab;
    public GameObject map3Prefab;

    [Header("Spawn Points")]
    public Vector3 map1SpawnPos = new Vector3(3.06f, 0.01f, 1.512f);
    public Vector3 map2SpawnPos = new Vector3(3.06f, 0.01f, 2.323f);
    public Vector3 map3SpawnPos = new Vector3(3.06f, 0.01f, 1.512f);

    [Header("Rotation for All Maps")]
    public Vector3 mapRotation = new Vector3(-90f, 0f, -90f); // Euler angles

    void Start()
    {
        int selectedMap = PlayerPrefs.GetInt("SelectedMap", 1);
        GameObject prefabToLoad = null;
        Vector3 spawnPos = Vector3.zero;

        switch (selectedMap)
        {
            case 1:
                prefabToLoad = map1Prefab;
                spawnPos = map1SpawnPos;
                break;
            case 2:
                prefabToLoad = map2Prefab;
                spawnPos = map2SpawnPos;
                break;
            case 3:
                prefabToLoad = map3Prefab;
                spawnPos = map3SpawnPos;
                break;
            default:
                Debug.LogWarning("Invalid map selection. Defaulting to Map 1.");
                prefabToLoad = map1Prefab;
                spawnPos = map1SpawnPos;
                break;
        }

        if (prefabToLoad != null)
        {
            Quaternion rotation = Quaternion.Euler(mapRotation);
            GameObject mapInstance = Instantiate(prefabToLoad, spawnPos, rotation);
            mapInstance.name = $"Map{selectedMap}_Instance";

            Debug.Log($"Spawned Map {selectedMap} at {spawnPos} with rotation {mapRotation}");
        }
        else
        {
            Debug.LogError("No map prefab assigned.");
        }
    }
}
