using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

//INPUTS N STUFF HAVE SENSITIVE STRINGS!!! CUZ IM TOO LAZY TO MAKE IT WORK BETTER

//BUT THE FORMATTING IS "slide" or "input" FIRST DEPENDING ON IF THE INPUT IS A SLIDER OR INPUTFIELD!

//THEN AFTER ITS THE NAME OF THE PLAYERPREF, SO IF WERE ADDING THE SCRIPT TO AN INPUT FOR SFX, IT WOULD BE "inputSFXVolume"

//BECAUSE ITS AN INPUTFIELD AND THE NAME OF THE PLAYERPREF FOR SFX IS "SFXVolume"

public class SettingsMenu : MonoBehaviour
{
    public GameObject MasterVolumeVolObj;

    public GameObject MusicVolObj;

    public GameObject SFXVolObj;

    private EventSystem eventSystem;

    private Coroutine checkCo;

    private void OnEnable()
    {
        Debug.Log("awake");
        Transform setting = transform.GetChild(2);

        MasterVolumeVolObj = setting.GetChild(1).gameObject;
        MusicVolObj = setting.GetChild(2).gameObject;
        SFXVolObj = setting.GetChild(3).gameObject;

        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(MasterVolumeVolObj.transform.GetChild(2).gameObject);

        if (Gamepad.current != null)
        {
            checkCo = StartCoroutine(checkForSelected());
        }
    }

    public void slideMasterVolume()
    {
        GameObject changed = MasterVolumeVolObj;
        Slider slider = changed.transform.GetChild(0).GetComponent<Slider>();
        TMP_InputField input = changed.transform.GetChild(1).GetComponent<TMP_InputField>();
        var newAmount = Mathf.RoundToInt(slider.value);


        Debug.Log(newAmount);

        PlayerPrefs.SetInt("MasterVolume", newAmount);

        updateVolumeUI(slider, input, "MasterVolume");
    }

    public void inputMasterVolume()
    {
        GameObject changed = MasterVolumeVolObj;
        Slider slider = changed.transform.GetChild(0).GetComponent<Slider>();
        TMP_InputField input = changed.transform.GetChild(1).GetComponent<TMP_InputField>();
        int newAmount = int.Parse(input.text);

        PlayerPrefs.SetInt("MasterVolume", newAmount);

        updateVolumeUI(slider, input, "MasterVolume");
    }

    public void changeMasterVolume(int amount)
    {
        GameObject changed = MasterVolumeVolObj;
        Slider slider = changed.transform.GetChild(0).GetComponent<Slider>();
        TMP_InputField input = changed.transform.GetChild(1).GetComponent<TMP_InputField>();
        var newAmount = PlayerPrefs.GetInt("MasterVolume") + amount;
        if (newAmount >= 100)
        {
            newAmount = 100;
        }
        else if (newAmount <= 0)
        {
            newAmount = 0;
        }

        PlayerPrefs.SetInt("MasterVolume", newAmount);

        updateVolumeUI(slider, input, "MasterVolume");

    }

    public void slideMusicVolume()
    {
        GameObject changed = MusicVolObj;
        Slider slider = changed.transform.GetChild(0).GetComponent<Slider>();
        TMP_InputField input = changed.transform.GetChild(1).GetComponent<TMP_InputField>();
        var newAmount = Mathf.RoundToInt(slider.value);

        PlayerPrefs.SetInt("MusicVolume", newAmount);

        updateVolumeUI(slider, input, "MusicVolume");
    }

    public void inputMusicVolume()
    {
        GameObject changed = MusicVolObj;
        Slider slider = changed.transform.GetChild(0).GetComponent<Slider>();
        TMP_InputField input = changed.transform.GetChild(1).GetComponent<TMP_InputField>();
        int newAmount = int.Parse(input.text);

        PlayerPrefs.SetInt("MusicVolume", newAmount);

        updateVolumeUI(slider, input, "MusicVolume");
    }
    public void changeMusicVolume(int amount)
    {
        GameObject changed = MusicVolObj;
        Slider slider = changed.transform.GetChild(0).GetComponent<Slider>();
        TMP_InputField input = changed.transform.GetChild(1).GetComponent<TMP_InputField>();

        var newAmount = PlayerPrefs.GetInt("MusicVolume") + amount;
        if (newAmount >= 100)
        {
            newAmount = 100;
        }
        else if (newAmount <= 0)
        {
            newAmount = 0;
        }

        PlayerPrefs.SetInt("MusicVolume", newAmount);

        updateVolumeUI(slider, input, "MusicVolume");

    }

    public void slideSFXVolume()
    {
        GameObject changed = SFXVolObj;
        Slider slider = changed.transform.GetChild(0).GetComponent<Slider>();
        TMP_InputField input = changed.transform.GetChild(1).GetComponent<TMP_InputField>();

        var newAmount = Mathf.RoundToInt(slider.value);

        PlayerPrefs.SetInt("SFXVolume", newAmount);

        updateVolumeUI(slider, input, "SFXVolume");
    }

    public void inputSFXVolume()
    {
        GameObject changed = SFXVolObj;
        Slider slider = changed.transform.GetChild(0).GetComponent<Slider>();
        TMP_InputField input = changed.transform.GetChild(1).GetComponent<TMP_InputField>();

        int newAmount = int.Parse(input.text);

        PlayerPrefs.SetInt("SFXVolume", newAmount);

        updateVolumeUI(slider, input, "SFXVolume");
    }

    public void changeSFXVolume(int amount)
    {
        GameObject changed = SFXVolObj;
        Slider slider = changed.transform.GetChild(0).GetComponent<Slider>();
        TMP_InputField input = changed.transform.GetChild(1).GetComponent<TMP_InputField>();

        var newAmount = PlayerPrefs.GetInt("SFXVolume") + amount;
        if (newAmount >= 100)
        {
            newAmount = 100;
        }
        else if (newAmount <= 0)
        {
            newAmount = 0;
        }

        PlayerPrefs.SetInt("SFXVolume", newAmount);

        updateVolumeUI(slider, input, "SFXVolume");
    }

    public void updateVolumeUI(Slider slider, TMP_InputField input, string prefName)
    {
        slider.value = PlayerPrefs.GetInt(prefName, 100);
        input.text = PlayerPrefs.GetInt(prefName, 100).ToString();

        MusicManager.Instance.volumeUpdated();

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.volumeUpdated();
        }
    }

    public void hideMenu()
    {
        PauseMenu pause = this.gameObject.GetComponentInParent<PauseMenu>();

        StopCoroutine(checkCo);
        pause.hideSettings();
        pause.showOptions();
    }

    public void updateAllSettings()
    {
        StartCoroutine(loadMenuCo());
    }

    private IEnumerator loadMenuCo()
    {
        Slider slider;
        TMP_InputField input;

        slider = MasterVolumeVolObj.transform.GetChild(0).GetComponent<Slider>();
        input = MasterVolumeVolObj.transform.GetChild(1).GetComponent<TMP_InputField>();
        updateVolumeUI(slider, input, "MasterVolume");

        slider = MusicVolObj.transform.GetChild(0).GetComponent<Slider>();
        input = MusicVolObj.transform.GetChild(1).GetComponent<TMP_InputField>();
        updateVolumeUI(slider, input, "MusicVolume");

        slider = SFXVolObj.transform.GetChild(0).GetComponent<Slider>();
        input = SFXVolObj.transform.GetChild(1).GetComponent<TMP_InputField>();
        updateVolumeUI(slider, input, "SFXVolume");

        yield break;
    }

    //to make sure theres always something selected
    private IEnumerator checkForSelected()
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.1f);

        GameObject lastSelected = EventSystem.current.currentSelectedGameObject;

        while (true)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(lastSelected);
            }
            else if (lastSelected != EventSystem.current.currentSelectedGameObject && lastSelected != null)
            {
                lastSelected = EventSystem.current.currentSelectedGameObject;
            }

            yield return wait;
        }
    }
}
