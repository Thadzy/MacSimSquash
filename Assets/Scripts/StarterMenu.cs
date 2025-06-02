using UnityEngine;
using UnityEngine.SceneManagement;

public class StarterMenu : MonoBehaviour
{
    public void OnStartPressed()
    {
        SceneManager.LoadScene("MapSelectScene");
    }

    public void OnAboutPressed()
    {
        // Future: Show about dialog or credits
        Debug.Log("About clicked. Implement content here.");
    }

    public void OnSettingPressed()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    public void OnExitPressed()
    {
        Application.Quit();
        Debug.Log("Game Exited.");
    }
}
