using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    private float time;

    private float timeLeft = 0;

    public TMP_Text timerText;

    public RectTransform chargeBar;
    public float chargeMaxX;
    public float chargeMinX;

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
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            PlayerStateManager.Instance.getState().totalTime += Time.deltaTime;
            Debug.Log(PlayerStateManager.Instance.getState().totalTime);

            //setText(Mathf.RoundToInt(timeLeft));

            timerText.text = ((int)(100 * timeLeft / time)).ToString() + "%";

            moveBar(timeLeft);

            TimerGearManager.Instance.setGearSpeed(timeLeft, time);

            yield return new WaitForEndOfFrame();
        }

        gameLost();

        yield break;
    }

    private void setText(float secs) //for like actual time, keeping just in case we pivot back
    {
        string time = "";

        float mins = (int)secs / 60;
        float sec = secs % 60;

        time += mins + ":";

        if (sec < 10)
        {
            time += "0" + sec;
        }
        else
        {
            time += sec;
        }

        timerText.text = time;
    }

    private void moveBar(float timeLeft)
    {
        Vector3 barPos = chargeBar.localPosition;
        barPos.x = Mathf.Lerp(chargeMaxX, chargeMinX, timeLeft / time);
        chargeBar.localPosition = barPos;
    }

    private void gameLost()
    {
        PlayerStateManager.Instance.getState().gameLost = true;
        SceneManager.LoadScene("LoseMenu");
    }

    public float getTimeLeft()
    {
        return timeLeft;
    }

    private IEnumerator waitForGears()
    {
        while (TimerGearManager.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }

        timeLeft = GameDataManager.Instance.getTimeLeft();
        time = GameDataManager.Instance.getTotalTime();

        StartCoroutine(startTimer());
    }
}
