using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public GameObject MasterVolObj;

    public GameObject MusicVolObj;

    public GameObject SFXVolObj;


    //pure buns but it works ok
    public void updateAllSettings()
    {
        Slider slider;
        TMP_Text text;

        float masterVol = PlayerPrefs.GetFloat("MasterVolume",100);
        slider = MasterVolObj.transform.GetChild(0).GetComponent<Slider>();
        text = MasterVolObj.transform.GetChild(1).GetComponent<TMP_Text>();
        updateVolumeUI(slider, text, masterVol);

        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 100);
        slider = MusicVolObj.transform.GetChild(0).GetComponent<Slider>();
        text = MusicVolObj.transform.GetChild(1).GetComponent<TMP_Text>();
        updateVolumeUI(slider, text, musicVol);

        float SFXVol = PlayerPrefs.GetFloat("SFXVolume", 100);
        slider = SFXVolObj.transform.GetChild(0).GetComponent<Slider>();
        text = SFXVolObj.transform.GetChild(1).GetComponent<TMP_Text>();
        updateVolumeUI(slider, text, SFXVol);
    }

    public void valueUpdated(string prefName)
    {
        GameObject choiceObj = null;

        float newVal = 0;

        switch (prefName)
        {
            case "MasterVolume":
                choiceObj = MasterVolObj;
                break;
            case "MusicVolume":
                choiceObj = MusicVolObj;
                break;
            case "SFXVolume":
                choiceObj = SFXVolObj;
                break;
        }

        Slider slider = choiceObj.transform.GetChild(0).GetComponent<Slider>();
        TMP_Text text = choiceObj.transform.GetChild(1).GetComponent<TMP_Text>();

        if (slider.value != PlayerPrefs.GetFloat(prefName))
        {
            newVal = slider.value;
        }
        else
        {
            float.TryParse(text.text, out float result);
            newVal = result;
        }

        newVal = Mathf.RoundToInt(newVal);

        PlayerPrefs.SetFloat(prefName, newVal);
        updateVolumeUI(slider, text, newVal);
    }

    public void updateVolumeUI(Slider slider, TMP_Text text, float value)
    {
        slider.value = value;
        text.text = value.ToString();
    }
}
