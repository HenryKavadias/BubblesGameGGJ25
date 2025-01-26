using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject PausePrefab;
    public void Quit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PausePrefab.SetActive(false);
    }

    public void Pause()
    {
        PausePrefab.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }
}
