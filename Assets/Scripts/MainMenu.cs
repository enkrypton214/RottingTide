
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource mainMenuChannel;
    public AudioClip mainMenuMusic;
    public TextMeshProUGUI highScoreUI;
    string newGameScene = "SampleScene";

    void Start()
    {
        mainMenuChannel.PlayOneShot(mainMenuMusic);
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"Top Wave Survived: {highScore}";
    }

    public void StartNewGame()
    {   mainMenuChannel.Stop();
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitApplication()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();
#endif
    }
}
