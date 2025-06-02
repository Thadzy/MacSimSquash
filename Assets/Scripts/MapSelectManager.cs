using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectManager : MonoBehaviour
{
    // Called from UI Button
    public void SelectMap(int mapId)
    {
        PlayerPrefs.SetInt("SelectedMap", mapId);  // 🧠 Store map choice for next scene
        Debug.Log("Map " + mapId + " selected.");

        // Load gameplay scene
        SceneManager.LoadScene("MainGameScene"); // 🛠 Replace with your real gameplay scene name
    }
}
