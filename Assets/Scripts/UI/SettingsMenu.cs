using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;

//INPUTS N STUFF HAVE SENSITIVE STRINGS!!! CUZ IM TOO LAZY TO MAKE IT WORK BETTER

//BUT THE FORMATTING IS "slide" or "input" FIRST DEPENDING ON IF THE INPUT IS A SLIDER OR INPUTFIELD!

//THEN AFTER ITS THE NAME OF THE PLAYERPREF, SO IF WERE ADDING THE SCRIPT TO AN INPUT FOR SFX, IT WOULD BE "inputSFXVolume"

//BECAUSE ITS AN INPUTFIELD AND THE NAME OF THE PLAYERPREF FOR SFX IS "SFXVolume"

public class SettingsMenu : MonoBehaviour
{
    public GameObject MasterVolObj;

    public GameObject MusicVolObj;

    public GameObject SFXVolObj;

    private bool loading = false;
    private EventSystem eventSystem;


    private void Awake()
    {
        Transform setting = transform.GetChild(2);

        MasterVolObj = setting.GetChild(1).gameObject;
        MusicVolObj = setting.GetChild(2).gameObject;
        SFXVolObj = setting.GetChild(3).gameObject;

        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(MasterVolObj.transform.GetChild(2).gameObject);
        Debug.Log(eventSystem);
    }

    //THIS IS ACTUALLY BUNS OK BUT IT SHOULD WORK!
    public void settingChanged(string typeAndPref)
    {
        if (!loading)
        {
            string inputType = typeAndPref.Substring(0, 5);
            string prefName = typeAndPref.Substring(5, typeAndPref.Length - 5);

            GameObject changed = null;

            int newVal = 0;

            switch (prefName)
            {
                case "MasterVolume":
                    changed = MasterVolObj;
                    break;
                case "MusicVolume":
                    changed = MusicVolObj;
                    break;
                case "SFXVolume":
                    changed = SFXVolObj;
                    break;
            }

            Slider slider = changed.transform.GetChild(0).GetComponent<Slider>();
            TMP_InputField input = changed.transform.GetChild(1).GetComponent<TMP_InputField>();

            if (inputType == "slide")
            {
                newVal = Mathf.RoundToInt(slider.value);
                PlayerPrefs.SetInt(prefName, newVal);
            }
            else if (inputType == "input")
            {
                newVal = int.Parse(input.text);
                PlayerPrefs.SetInt(prefName, newVal);
            }

            updateVolumeUI(slider, input, prefName);
        }
    }

    public void changeMasterVolume(int amount)
    {
        GameObject changed = MasterVolObj;
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
    }

    public void hideMenu()
    {
        PauseMenu pause = this.gameObject.GetComponentInParent<PauseMenu>();

        pause.hideSettings();
        pause.showOptions();
    }

    public void updateAllSettings()
    {
        StartCoroutine(loadMenuCo());
    }

    private IEnumerator loadMenuCo()
    {
        loading = true;

        Slider slider;
        TMP_InputField input;

        slider = MasterVolObj.transform.GetChild(0).GetComponent<Slider>();
        input = MasterVolObj.transform.GetChild(1).GetComponent<TMP_InputField>();
        updateVolumeUI(slider, input, "MasterVolume");

        slider = MusicVolObj.transform.GetChild(0).GetComponent<Slider>();
        input = MusicVolObj.transform.GetChild(1).GetComponent<TMP_InputField>();
        updateVolumeUI(slider, input, "MusicVolume");

        slider = SFXVolObj.transform.GetChild(0).GetComponent<Slider>();
        input = SFXVolObj.transform.GetChild(1).GetComponent<TMP_InputField>();
        updateVolumeUI(slider, input, "SFXVolume");

        loading = false;

        yield break;
    }
}
