using UnityEngine;
using TMPro;

public class SettingsUIManager : MonoBehaviour
{
    [Header("UI Input Fields")]
    public TMP_InputField dropHeightInput;       // In meters
    public TMP_InputField corInput;              // Between 0 - 1
    public TMP_InputField springConstantInput;   // In N.m/deg

    [Header("Default Values")]
    public float defaultDropHeight = 2.0f;
    public float defaultCOR = 0.5f;
    public float defaultSpringConstant = 10f;

    void Start()
    {
        // Load saved values or use defaults
        dropHeightInput.text = PlayerPrefs.GetFloat("DropHeight", defaultDropHeight).ToString("F2");
        corInput.text = PlayerPrefs.GetFloat("COR", defaultCOR).ToString("F2");
        springConstantInput.text = PlayerPrefs.GetFloat("SpringConstant", defaultSpringConstant).ToString("F2");
    }

    public void OnApplyPressed()
    {
        float dropH = float.TryParse(dropHeightInput.text, out float dh) ? Mathf.Max(dh, 0.3f) : defaultDropHeight;
        float cor = float.TryParse(corInput.text, out float c) ? Mathf.Clamp01(c) : defaultCOR;
        float k = float.TryParse(springConstantInput.text, out float s) ? s : defaultSpringConstant;

        PlayerPrefs.SetFloat("DropHeight", dropH);
        PlayerPrefs.SetFloat("COR", cor);
        PlayerPrefs.SetFloat("SpringConstant", k);
        PlayerPrefs.Save();

        Debug.Log($"Settings Saved: DropHeight={dropH}, COR={cor}, SpringConstant={k}");
    }

    public void OnBackPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGameScene");
    }
}
