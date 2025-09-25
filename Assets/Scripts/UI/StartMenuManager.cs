using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class StartMenuManager : MonoBehaviour
{
    public static StartMenuManager Instance;

    public GameObject settingsObj;

    public GameObject firstSettingsButton;

    public GameObject menuSelect;

    public GameObject levelSelect;

    public GameObject bSideSelect;

    private GameObject lastButton;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        MusicManager.Instance.fadeIn();
        MusicManager.Instance.playMusic("StartScreen");
    }

    public void startCutscene()
    {
        LevelTransition.Instance.doTransition("Cutscene");
        MusicManager.Instance.transitionSong("Cutscene");
        GameDataManager.Instance.setLevelData("Story");
    }

    public void startBSide(string dataName)
    {
        GameDataManager.Instance.setLevelData(dataName);
        LevelTransition.Instance.doTransition(GameDataManager.Instance.curLevel.scenes[0].sceneName);
        MusicManager.Instance.transitionSong(GameDataManager.Instance.curLevel.scenes[0].sceneSong);
    }

    public void startCredits()
    {
        MusicManager.Instance.fadeOut();
        LevelTransition.Instance.doTransition("Credits");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void openOptions()
    {
        settingsObj.SetActive(true);

        if (Gamepad.current != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSettingsButton);
        }
    }

    public void hideOptions()
    {
        settingsObj.SetActive(false);

        if (Gamepad.current != null)
        {
            EventSystem.current.SetSelectedGameObject(menuSelect.transform.GetChild(1).gameObject);
        }
    }

    public void openLevelSelect()
    {
        levelSelect.SetActive(true);
        menuSelect.SetActive(false);

        if (Gamepad.current != null)
        {
            EventSystem.current.SetSelectedGameObject(levelSelect.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject);
        }
    }

    public void hideLevelSelect()
    {
        levelSelect.SetActive(false);
        menuSelect.SetActive(true);

        if (Gamepad.current != null)
        {
            EventSystem.current.SetSelectedGameObject(menuSelect.transform.GetChild(0).gameObject);
        }
    }

    public void showBSide()
    {
        levelSelect.SetActive(false);
        bSideSelect.SetActive(true);

        if (Gamepad.current != null)
        {
            EventSystem.current.SetSelectedGameObject(bSideSelect.transform.GetChild(0).gameObject);
        }
    }

    public void hideBSide()
    {
        levelSelect.SetActive(true);
        bSideSelect.SetActive(false);

        if (Gamepad.current != null)
        {
            EventSystem.current.SetSelectedGameObject(levelSelect.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject);
        }
    }

    private void Update()
    {
        if (Gamepad.current != null)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(lastButton);
            }
            else if (lastButton != EventSystem.current.currentSelectedGameObject)
            {
                lastButton = EventSystem.current.currentSelectedGameObject;
            }
        }
    }
}
