using System;
using UnityEngine;

public class SettingsSliderOverlay : MonoBehaviour
{
    public GameObject blipPre;

    public RectTransform[] blips;

    void Start()
    {
        Vector3 blipPos = new Vector3(-100f, 0 ,0);
        blips = new RectTransform[20];

        for (int i = 0; i < blips.Length; ++i)
        {
            GameObject newBlip = Instantiate(blipPre, this.transform);
            newBlip.transform.localPosition = blipPos;
            blipPos += Vector3.right * 10.4f;
            blips[i] = newBlip.GetComponent<RectTransform>();
        }
    }

    public void updateValue(float val)
    {
        int numTillDown = (int)Mathf.Floor(blips.Length/100f * val);//number of blips to be shrunk

        Vector2 upSize = new Vector2(8f, 20f);
        Vector2 downSize = new Vector2(8f, 8f);

        for (int i = 0; i < blips.Length; ++i)
        {
            if (i < numTillDown) blips[i].sizeDelta = upSize;
            else blips[i].sizeDelta = downSize;
        }

        if (val % 5 == 0) return;

        Vector2 midSize = new Vector2(8f, Mathf.Lerp(8, 20, val % 5 / 5 ));
        blips[numTillDown].sizeDelta = midSize;
    }
}
