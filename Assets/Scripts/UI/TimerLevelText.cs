using UnityEngine;
using UnityEngine.UI;

using TMPro;
using static LevelData;
using System;
using System.Collections;

public class TimerLevelText : MonoBehaviour
{
    private Scene data;

    public Image background;
    public TMP_Text levelNameText;

    private TMP_Text deltaTimeText;

    private TMP_Text levelTimeText;

    private float time = 0;
    
    public void setUp()
    {
        background = transform.GetComponent<Image>();
        levelNameText = transform.GetChild(0).GetComponent<TMP_Text>();
        levelTimeText = transform.GetChild(1).GetComponent<TMP_Text>();
        deltaTimeText = transform.GetChild(2).GetComponent<TMP_Text>();

    }
    public void startTimer()
    {
        StartCoroutine(timerCo());
    }

    public IEnumerator timerCo()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while(true)
        {
            updateText();
            levelTimeText.text = convertToTimeString(GameDataManager.Instance.curLevel.scenes[GameDataManager.Instance.getLevelNumber()].sceneTime);
            yield return wait;
        }
    }

    public void setData(Scene data)
    {
        this.data = data;
        updateText();
    }

    public void setColor(Color32 color)
    {
        background.color = color;
    }

    public void updateText()
    {
        levelTimeText.text = convertToTimeString(data.sceneTime);
        levelNameText.text = data.displayName;
        deltaTimeText.text = "0";
    }

    private string convertToTimeString(float secs)
    {
        string time = "";

        float mins = (int)secs / 60;
        int sec = (int)(secs % 60);
        float milli = (float)Math.Round(secs - ((int)secs), 2);

        time += mins + ":";

        if (sec < 10)
        {
            time += "0" + sec;
        }
        else
        {
            time += sec;
        }
        time += ":";
        milli *= 100;
        milli = (int)milli;
        if (milli < 10)
        {
            time += "0" + milli;
        }
        else
        {
            time += milli;
        }

        return time.Substring(0, 7);
    }
}
