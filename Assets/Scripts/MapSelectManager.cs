using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectManager : MonoBehaviour
{
    public void SelectMap(int mapIndex)
    {
        PlayerPrefs.SetInt("SelectedMap", mapIndex);
        PlayerPrefs.Save();

        Debug.Log("Selected Map: " + mapIndex);

        // Now load the gameplay scene
        SceneManager.LoadScene("MainGameScene"); // Ensure this scene exists
    }

    public void BackToStart()
    {
        SceneManager.LoadScene("StarterScene");
    }
}
