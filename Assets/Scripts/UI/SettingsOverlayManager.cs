using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SettingsOverlayManager : MonoBehaviour
{
    public static SettingsOverlayManager Instance;

    private Dictionary<String, SettingsSliderOverlay> overlays;

    public GameObject overlayPre;

    public bool initialized = false;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        overlays = new Dictionary<string, SettingsSliderOverlay>();
    }

    public bool isInitialized()
    {
        return initialized;
    }

    public void doneInitializing()
    {
        Debug.Log("Done");
        initialized = true;
    }

    public void createOverlay(String name, Vector3 pos)
    {
        GameObject newOverlay = Instantiate(overlayPre, this.transform);
        newOverlay.transform.position = pos;
        overlays.Add(name, newOverlay.GetComponent<SettingsSliderOverlay>());
    }

    public void updateValue(String name, float val)
    {
        overlays[name].updateValue(val);
    }
}
