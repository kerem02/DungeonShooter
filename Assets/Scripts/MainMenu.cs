using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public AudioClip gameMusic; // The game music to play when starting the game
    private AudioManager audioManager; // Reference to the AudioManager

    private void Start()
    {
        audioManager = AudioManager.instance; // Get the instance of the AudioManager
    }

    // Method to start the game
    public void StartGame()
    {
        // Play the game music if AudioManager is available
        if (audioManager != null)
        {
            audioManager.PlayMusic(gameMusic);
        }

        // Load the game scene
        SceneManager.LoadScene("SampleScene");
    }

    // Method to quit the game
    public void QuitGame()
    {
        Debug.Log("Game is exiting"); // Log a message to the console
        Application.Quit(); // Quit the application
    }
}

