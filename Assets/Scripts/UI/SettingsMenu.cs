using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

//INPUTS N STUFF HAVE SENSITIVE STRINGS!!! CUZ IM TOO LAZY TO MAKE IT WORK BETTER

//BUT THE FORMATTING IS "slide" or "input" FIRST DEPENDING ON IF THE INPUT IS A SLIDER OR INPUTFIELD!

//THEN AFTER ITS THE NAME OF THE PLAYERPREF, SO IF WERE ADDING THE SCRIPT TO AN INPUT FOR SFX, IT WOULD BE "inputSFXVolume"

//BECAUSE ITS AN INPUTFIELD AND THE NAME OF THE PLAYERPREF FOR SFX IS "SFXVolume"

public class SettingsMenu : MonoBehaviour
{
    public GameObject MasterVolumeVolObj;

    public GameObject MusicVolObj;

    public GameObject SFXVolObj;

    public TMP_Text VIDIOtext;

    private EventSystem eventSystem;

    private Coroutine checkCo;

    private bool canBlip = true;
    private Coroutine glowCo;

    private void OnEnable()
    {  
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

        StartCoroutine(waitForOverlay());
        
        updateAllSettings();

        startVIDIOGlow();
    }

    public void slideMasterVolume()
    {
        GameObject changed = MasterVolumeVolObj;
        Slider slider = changed.transform.GetChild(0).GetComponent<Slider>();
        TMP_InputField input = changed.transform.GetChild(1).GetComponent<TMP_InputField>();
        var newAmount = Mathf.RoundToInt(slider.value);

        PlayerPrefs.SetInt("MasterVolume", newAmount);

        updateVolumeUI(slider, input, "MasterVolume");
        updateOverlay("master");
    }

    public void inputMasterVolume()
    {
        GameObject changed = MasterVolumeVolObj;
        Slider slider = changed.transform.GetChild(0).GetComponent<Slider>();
        TMP_InputField input = changed.transform.GetChild(1).GetComponent<TMP_InputField>();
        int newAmount = int.Parse(input.text);

        PlayerPrefs.SetInt("MasterVolume", newAmount);

        updateVolumeUI(slider, input, "MasterVolume");
        updateOverlay("master");
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
        updateOverlay("master");
    }

    public void slideMusicVolume()
    {
        GameObject changed = MusicVolObj;
        Slider slider = changed.transform.GetChild(0).GetComponent<Slider>();
        TMP_InputField input = changed.transform.GetChild(1).GetComponent<TMP_InputField>();
        var newAmount = Mathf.RoundToInt(slider.value);

        PlayerPrefs.SetInt("MusicVolume", newAmount);

        updateVolumeUI(slider, input, "MusicVolume");
        updateOverlay("music");
    }

    public void inputMusicVolume()
    {
        GameObject changed = MusicVolObj;
        Slider slider = changed.transform.GetChild(0).GetComponent<Slider>();
        TMP_InputField input = changed.transform.GetChild(1).GetComponent<TMP_InputField>();
        int newAmount = int.Parse(input.text);

        PlayerPrefs.SetInt("MusicVolume", newAmount);

        updateVolumeUI(slider, input, "MusicVolume");
        updateOverlay("music");
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
        updateOverlay("music");
    }

    public void slideSFXVolume()
    {
        GameObject changed = SFXVolObj;
        Slider slider = changed.transform.GetChild(0).GetComponent<Slider>();
        TMP_InputField input = changed.transform.GetChild(1).GetComponent<TMP_InputField>();

        var newAmount = Mathf.RoundToInt(slider.value);

        PlayerPrefs.SetInt("SFXVolume", newAmount);

        updateVolumeUI(slider, input, "SFXVolume");
        updateOverlay("sfx");
    }

    public void inputSFXVolume()
    {
        GameObject changed = SFXVolObj;
        Slider slider = changed.transform.GetChild(0).GetComponent<Slider>();
        TMP_InputField input = changed.transform.GetChild(1).GetComponent<TMP_InputField>();

        int newAmount = int.Parse(input.text);

        PlayerPrefs.SetInt("SFXVolume", newAmount);

        updateVolumeUI(slider, input, "SFXVolume");
        updateOverlay("sfx");
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
        updateOverlay("sfx");
    }

    public void updateVolumeUI(Slider slider, TMP_InputField input, string prefName)
    {
        slider.value = PlayerPrefs.GetInt(prefName, 100);
        input.text = PlayerPrefs.GetInt(prefName, 100).ToString();

        MusicManager.Instance.volumeUpdated();

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.volumeUpdated();
            playBlip();
            blipCooldown();
        }
    }

    public void hideMenu()
    {
        PauseMenu pause = this.gameObject.GetComponentInParent<PauseMenu>();

        tryStopCheckCo();
        stopVIDIOGlow();

        if (SceneManager.GetActiveScene().name.Equals("StartMenu")) 
        {
            StartMenuManager.Instance.hideOptions();
            return;
        }

        pause.hideSettings();
        pause.showOptions();
    }
    private void tryStopCheckCo()
    {
        if (checkCo != null)
        {
            StopCoroutine(checkCo);
        }
        checkCo = null;
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

    public void updateOverlay(String name)
    {
        SettingsOverlayManager manager = SettingsOverlayManager.Instance;

        if (manager == null) return;

        switch (name){
            case("master"):
                manager.updateValue("master", PlayerPrefs.GetInt("MasterVolume"));
                break;
            case("music"):
                manager.updateValue("music", PlayerPrefs.GetInt("MusicVolume"));
                break;
            case("sfx"):
                manager.updateValue("sfx", PlayerPrefs.GetInt("SFXVolume"));
                break;
        }
    }

    // private IEnumerator checkButtonHold()
    // {
        
    // }

    private IEnumerator waitForOverlay()
    {
        Debug.Log("HI");

        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while (SettingsOverlayManager.Instance == null)
        {
            Debug.Log("waiting for overlay");
            yield return wait;
        }

        SettingsOverlayManager manager = SettingsOverlayManager.Instance;

        if (manager.isInitialized()) yield break;

        manager.createOverlay("master", MasterVolumeVolObj.transform.GetChild(0).position);
        manager.createOverlay("music", MusicVolObj.transform.GetChild(0).position);
        manager.createOverlay("sfx", SFXVolObj.transform.GetChild(0).position);

        yield return wait;

        manager.updateValue("master", PlayerPrefs.GetInt("MasterVolume"));
        manager.updateValue("music", PlayerPrefs.GetInt("MusicVolume"));
        manager.updateValue("sfx", PlayerPrefs.GetInt("SFXVolume"));

        manager.doneInitializing();
    }

    private void playBlip()
    {
        if (SoundManager.Instance == null || !canBlip) return;

        SoundManager.Instance.playSoundFX("uiBlip", Vector3.zero, 0, 10, 0.4f, true);
    }

    //to prevent multiple of them playing at once
    private void blipCooldown()
    {
        if (canBlip) StartCoroutine(blipCooldownCo());
    }

    private IEnumerator blipCooldownCo()
    {
        canBlip = false;
        yield return new WaitForSecondsRealtime(0.1f);
        canBlip = true;
    }

    //purely visual thing cuz i thought it'd look cool

    private void startVIDIOGlow()
    {
        if (glowCo == null) glowCo = StartCoroutine(VIDIOGlowCo());
    }

    private void stopVIDIOGlow()
    {
        if (glowCo != null) StopCoroutine(glowCo);
        glowCo = null;
    }

    private IEnumerator VIDIOGlowCo()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        Color32 initColor = new Color32(20, 77, 23, 255);
        Color32 glowColor = new Color32(24, 255, 0, 255);

        float time = 0;

        while (true)
        {
            time += Time.unscaledDeltaTime;

            VIDIOtext.color = Color32.Lerp(initColor, glowColor, (Mathf.Sin(time) + 1) / 2);

            yield return wait;
        }
    }
}
