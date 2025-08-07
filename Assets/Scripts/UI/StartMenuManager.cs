using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    private void Start()
    {
        MusicManager.Instance.fadeIn();
        MusicManager.Instance.playMusic("StartScreen");
    }

    public void startGame()
    {
        LevelTransition.Instance.doTransition("MainScene");
        MusicManager.Instance.transitionSong("LevelMusic");

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
