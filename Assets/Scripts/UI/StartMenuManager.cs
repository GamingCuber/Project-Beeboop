using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void openOptions()
    {
        // TODO: Put Options Menu Stuff in Here
    }
}
