using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    public int time; //TOTAL in seconds

    public TMP_Text timerText;

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

            setText(Mathf.RoundToInt(timeLeft));

            TimerGearManager.Instance.setGearSpeed(timeLeft, time);

            yield return new WaitForEndOfFrame();
        }

        gameLost();

        yield break;
    }

    private void setText(float secs)
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

    private void gameLost()
    {
        SceneManager.LoadScene("LoseScreen");
    }
}
