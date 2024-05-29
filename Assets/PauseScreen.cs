
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public GameObject pauseScreen;

    // Start is called before the first frame update
    void Start()
    {
        pauseScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseScreen();
        }

    }

    private void TogglePauseScreen()
    {
        bool isActive = pauseScreen.activeSelf;
        pauseScreen.SetActive(!pauseScreen.activeSelf);
        Time.timeScale = isActive ? 1 : 0;
    }
}
