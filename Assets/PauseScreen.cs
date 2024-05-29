using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public GameObject pauseScreen; // Reference to the pause screen UI

    // Start is called before the first frame update
    void Start()
    {
        pauseScreen.SetActive(false); // Hide the pause screen at the start
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseScreen(); // Toggle the pause screen when the Escape key is pressed
        }
    }

    // Method to toggle the pause screen
    private void TogglePauseScreen()
    {
        bool isActive = pauseScreen.activeSelf; // Check if the pause screen is currently active
        pauseScreen.SetActive(!pauseScreen.activeSelf); // Toggle the active state of the pause screen
        Time.timeScale = isActive ? 1 : 0; // Pause or resume the game based on the active state of the pause screen
    }
}
