using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    public int time; //TOTAL in seconds

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

        StartCoroutine(startTimer());
    }

    private IEnumerator startTimer()
    {
        float timeLeft = time;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

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
        SceneManager.LoadScene("LoseScreen");
    }
}
