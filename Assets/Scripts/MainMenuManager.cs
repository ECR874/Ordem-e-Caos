using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("Waypoint");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
