using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    private float dingAmount = 4;

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
        float interval = time / dingAmount;

        float count = interval;

        for (int i = 0; i < dingAmount; i++)
        {
            if (timeLeft > count)
            {
                intervalsHit[count] = false;
            }
            else
            {
                intervalsHit[count] = true;
            }
            count += interval;
        }
    }

    private void checkIntervals()
    {
        foreach (KeyValuePair<float, bool> kv in intervalsHit)
        {
            if (kv.Key < timeLeft && !kv.Value)
            {
                SoundManager.Instance.playPlayerSound("batteryAlert");
                break;
            }
        }
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

        StartCoroutine(startTimer());
    }
}
