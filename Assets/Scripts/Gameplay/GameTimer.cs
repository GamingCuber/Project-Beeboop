using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    private float time;

    public float timeLeft;

    public TMP_Text timerText;

    public RectTransform chargeBar;

    public float chargeMaxX;
    public float chargeMinX;

    public float maxVertSpeed;

    private Dictionary<float, bool> intervalsHit = new Dictionary<float, bool>();
    private float[] dingPercentTimes = {75, 50, 25, 15, 10, 5, 3, 2, 1};

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        StartCoroutine(waitForGears());
    }

    private IEnumerator startTimer()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            PlayerStateManager.Instance.getState().totalTime += Time.deltaTime;

            timerText.text = ((int)(100 * timeLeft / time)).ToString() + "%";

            moveBar(timeLeft);

            TimerGearManager.Instance.setGearSpeed(timeLeft, time);

            yield return wait;
        }

        gameLost();

        yield break;
    }

    private void moveBar(float timeLeft)
    {
        Vector3 barPos = chargeBar.localPosition;
        barPos.x = Mathf.Lerp(chargeMaxX, chargeMinX, timeLeft / time);
        barPos.y += maxVertSpeed * Time.deltaTime;

        if (barPos.y >= 120) //-93 top, 120 bottom, found by moving charge in editor
        {
            barPos.y = -93f;
        }

        chargeBar.localPosition = barPos;
    }

    private void gameLost()
    {
        PlayerStateManager.Instance.getState().gameLost = true;
        LevelTransition.Instance.doTransition("LoseMenu");
        MusicManager.Instance.transitionSong("test");
    }

    public float getTimeLeft()
    {
        return timeLeft;
    }

    private void initializeList()
    {
        for (int i = 0; i < dingPercentTimes.Length; i++)
        {
            float percent = dingPercentTimes[i] / 100;

            float dingTime = time * percent;

            intervalsHit[dingTime] = false;
        }
    }

    //will allow the timer to beep again for these intervals (when u recharge back up)
    private void updateIntervalList()
    {
        float curTimeLeft = timeLeft;

        List<float> times = new List<float>();

        //to get all the ding times that have already went off
        foreach(KeyValuePair<float, bool> kv in intervalsHit)
        {
            if (kv.Key < curTimeLeft && kv.Value)
            {
                times.Add(kv.Key);
            }
        }

        //turn them all to true again
        for (int i = 0; i < times.Count; i++)
        {
            intervalsHit[times[i]] = false;
        }
    }

    private IEnumerator IntervalChecks()
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1f);

        while (true)
        {
            checkIntervals();
            yield return wait;
        }
    }

    private void checkIntervals()
    {
        foreach (KeyValuePair<float, bool> kv in intervalsHit)
        {
            if (kv.Key >= timeLeft && !kv.Value)
            {
                intervalsHit[kv.Key] = true;
                SoundManager.Instance.playPlayerSound("batteryAlert");
                StartCoroutine(flashChargeColor());
                break;
            }
        }
    }

    private IEnumerator flashChargeColor()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        Image[] imgs = new Image[chargeBar.childCount];

        for (int i = 0; i < imgs.Length; i++)
        {
            imgs[i] = chargeBar.GetChild(i).GetComponent<Image>();
        }

        float timer = 0;
        float flashTime = 0.2f;

        int flashCount = 0;
        int totalCount = 3;

        bool onRed = false;

        Color32 color = imgs[0].color;

        while (flashCount < totalCount)
        {
            timer += Time.deltaTime;

            if (!onRed)
            {
                color.g = (byte)Mathf.Lerp(255, 0, timer / flashTime);
                color.b = (byte)Mathf.Lerp(255, 0, timer / flashTime);
            }
            else
            {
                color.g = (byte)Mathf.Lerp(0, 255, timer / flashTime);
                color.b = (byte)Mathf.Lerp(0, 255, timer / flashTime);
            }

            for (int e = 0; e < imgs.Length; e++)
            {
                imgs[e].color = color;
            }

            if (timer >= flashTime)
            {
                timer = 0;

                if (!onRed)
                {
                    onRed = true;
                }
                else
                {
                    onRed = false;
                    flashCount++;
                }
            }

            yield return wait;
        }
        yield break;
    }

    public void addTime(float time)
    {
        StartCoroutine(addTimeCo(time));
    }

    //dripfeeding the time so it increases in increments instead of it all being added at once
    private IEnumerator addTimeCo(float time)
    {
        float maxAdd = time/10;

        float timeLeftToAdd = time;

        TimerGearManager.Instance.setMult(5f);
        TimerGearManager.Instance.reverseGears();

        while (timeLeftToAdd > 0)
        {
            timeLeftToAdd -= maxAdd;

            if (timeLeft + maxAdd > this.time)
            {
                timeLeft = this.time;
            }
            else
            {
                timeLeft += maxAdd;
            }

            yield return new WaitForSecondsRealtime(0.1f);
        }

        TimerGearManager.Instance.reverseGears();
        TimerGearManager.Instance.resetMult();

        updateIntervalList();
    }

    private IEnumerator waitForGears()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while (TimerGearManager.Instance == null)
        {
            yield return wait;
        }

        timeLeft = GameDataManager.Instance.getTimeLeft();
        time = GameDataManager.Instance.getTotalTime();
        initializeList();
        StartCoroutine(IntervalChecks());

        StartCoroutine(startTimer());
    }
}
