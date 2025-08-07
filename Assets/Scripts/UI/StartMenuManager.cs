using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public GameObject settingsObj;

    private void Start()
    {
        MusicManager.Instance.fadeIn();
        MusicManager.Instance.playMusic("StartScreen");
    }

    public void startCutscene()
    {
        LevelTransition.Instance.doTransition("Cutscene");
        MusicManager.Instance.transitionSong("Cutscene");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void openOptions()
    {
        settingsObj.SetActive(true);
    }

    public void hideOptions()
    {
        settingsObj.SetActive(false);
    }
}
