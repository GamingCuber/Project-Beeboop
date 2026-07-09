using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeTextManager : MonoBehaviour
{
    public static TimeTextManager Instance;

    public GameObject timer;

    public Transform levelMask;

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
        checkEnable();
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
            GameObject newLevelTime = Instantiate(levelTimePre, levelMask.transform);
            levelTimes[i] = newLevelTime;
            levelTimes[i].transform.GetChild(0).GetComponent<TimerLevelText>().setUp();
            levelTimes[i].transform.GetChild(0).GetComponent<TimerLevelText>().setData(level.scenes[i]);
            levelTimes[i].transform.GetChild(0).GetComponent<TimerLevelText>().updateText();
            levelTimes[i].transform.localPosition = new Vector3(0, 32f - (32f * i), 0);
        }

        if (level.scenes.Length < 3)
        {
            int amtDiff = 3 - level.scenes.Length;

            RectTransform bgRect = timer.transform.GetChild(0).GetComponent<RectTransform>();
            bgRect.sizeDelta = new Vector2(bgRect.sizeDelta.x, bgRect.sizeDelta.y - 32f * amtDiff);
            bgRect.localPosition = new Vector3(0, bgRect.localPosition.y + 16f * amtDiff, 0);
        }

        offsetLevels();
    }

    public void enableTimer()
    {
        timer.SetActive(true);
        startTimer();
    }

    public void hideTimer()
    {
        Vector3 pos = new Vector3(999f, 999f, 0f);
        timer.transform.localPosition = pos;
    }

    public void showTimer()
    {
        Vector3 pos = new Vector3(-347f, -79.4f, 0f); //i got this from ingame scene
        timer.transform.localPosition = pos;
    }

    public void startTimer()
    {
        for (int i = 0; i < GameDataManager.Instance.getLevelData().scenes.Length; ++i)
        {
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

    private void offsetLevels()
    {
        int moveAmt = GameDataManager.Instance.getLevelNumber();
        int totalAmt = GameDataManager.Instance.getLevelData().scenes.Length;

        if (moveAmt <= 1) moveAmt = 0;
        else if (moveAmt >= totalAmt - 2) moveAmt = totalAmt - 3;
        else moveAmt = moveAmt - 1;

        for (int i = 0; i < levelTimes.Length; ++i)
        {
            levelTimes[i].transform.localPosition += Vector3.up * moveAmt * 32f;
        }
    }

    public void disableTimer()
    {
        timer.SetActive(false);
    }

    public void swapEnable()
    {
        PlayerStateManager psm = PlayerStateManager.Instance;

        if (PlayerStateManager.Instance.getState().wantsTimer) psm.getState().wantsTimer = false;
        else psm.getState().wantsTimer = true;

        checkEnable();
    }

    public void checkEnable()
    {
        if (PlayerStateManager.Instance.getState().wantsTimer) enableTimer();
        else disableTimer();
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
