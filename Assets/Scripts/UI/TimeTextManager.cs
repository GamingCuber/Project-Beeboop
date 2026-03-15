using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeTextManager : MonoBehaviour
{
    public static TimeTextManager Instance;

    [SerializeField]
    private TMP_Text totalTimeText;
    [SerializeField]
    private TMP_Text levelTimeText;
    public GameObject timer;

    private Coroutine timerCo = null;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void enableTimer()
    {
        
    }

    public void disableTimer()
    {
        
    }

    private IEnumerator startTimer()
    {
        float timer = 0;

        yield break;
    }

    void Update()
    {
        timer.SetActive(PlayerStateManager.Instance.getState().wantsTimer);

        totalTimeText.text = convertToTimeString(PlayerStateManager.Instance.getState().totalTime);

        if (SceneManager.GetSceneByName("MainScene").isLoaded)
        {
            PlayerStateManager.Instance.getState().firstLevelTime += Time.deltaTime;
            levelTimeText.text = convertToTimeString(PlayerStateManager.Instance.getState().firstLevelTime);
        }
        else if (SceneManager.GetSceneByName("Dash Level").isLoaded)
        {
            PlayerStateManager.Instance.getState().secondLevelTime += Time.deltaTime;
            levelTimeText.text = convertToTimeString(PlayerStateManager.Instance.getState().secondLevelTime);
        }
        else if (SceneManager.GetSceneByName("Hook Level").isLoaded)
        {
            PlayerStateManager.Instance.getState().thirdLevelTime += Time.deltaTime;
            levelTimeText.text = convertToTimeString(PlayerStateManager.Instance.getState().thirdLevelTime);
        }
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
