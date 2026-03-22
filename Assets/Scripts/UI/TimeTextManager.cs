using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeTextManager : MonoBehaviour
{
    public static TimeTextManager Instance;

    public GameObject timer;

    public TMP_Text mainTimer;

    public GameObject levelTimePre;

    private GameObject[] levelTimes;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        setUp();
    }

    void Update()
    {
        mainTimer.text = convertToTimeString(PlayerStateManager.Instance.getState().totalTime);
    }

    public void setUp()
    {
        LevelData level = GameDataManager.Instance.getLevelData();

        levelTimes = new GameObject[level.scenes.Length];

        for (int i = 0; i < level.scenes.Length; ++i)
        {
            GameObject newLevelTime = Instantiate(levelTimePre, timer.transform);
            levelTimes[i] = newLevelTime;
            levelTimes[i].transform.GetChild(0).GetComponent<TimerLevelText>().setUp();
            levelTimes[i].transform.GetChild(0).GetComponent<TimerLevelText>().setData(level.scenes[i]);
            levelTimes[i].transform.GetChild(0).GetComponent<TimerLevelText>().updateText();
            levelTimes[i].transform.localPosition = new Vector3(0, 11f - (32f * i), 0);

            if (i != GameDataManager.Instance.getLevelNumber())
            {
                levelTimes[i].transform.GetChild(0).GetComponent<TimerLevelText>().setColor(new Color32(78, 115, 70, 100));
            }
            else
            {
                levelTimes[i].transform.GetChild(0).GetComponent<TimerLevelText>().startTimer();
            }
        }
    }

    public void enableTimer()
    {
        
    }

    public void disableTimer()
    {
        
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
