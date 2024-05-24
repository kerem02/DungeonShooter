using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public AudioClip gameMusic;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
    }
    public void StartGame()
    {
        if(audioManager != null)
        {
            audioManager.PlayMusic(gameMusic);
        }
        
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Debug.Log("Game is exiting");
        Application.Quit();
    }
}
