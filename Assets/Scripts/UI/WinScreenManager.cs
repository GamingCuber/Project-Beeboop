using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text deathText;
    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private TMP_Text rankText;
    [SerializeField]
    [Tooltip("When editing either rankChars or secondsPerRank, make sure both have the same number of fields")]
    private char[] rankChars;
    [SerializeField]
    [Tooltip("When editing either rankChars or secondsPerRank, make sure both have the same number of fields")]
    private float[] secondsPerRank;
    [SerializeField]
    [Tooltip("First is for +, Second is for blank, Third is assumed to be anything above")]
    private int[] deathsPerSign;

    public void backButton()
    {
        SceneManager.LoadScene("StartMenu");
    }

    private void Start()
    {

        if (rankChars.Length != secondsPerRank.Length)
        {
            Debug.LogError("Make sure both arrays are the same size!!!!");
        }
        int deathNumber = PlayerStateManager.Instance.getState().deathNumber;
        float totalTime = PlayerStateManager.Instance.getState().totalTime;

        char currentSign = getSign(deathNumber);
        char currentLetter = getLetter(totalTime);

        deathText.text = string.Format("Deaths - {0}", deathNumber);
        timeText.text = string.Format("Time - {0}", convertToTimeString(totalTime));
        rankText.text = string.Format("Rank - {0}", string.Format("{0}{1}", currentLetter, currentSign));

    }

    private char getLetter(float totalTime)
    {
        for (var i = 0; i < secondsPerRank.Length; i++)
        {
            if (totalTime < secondsPerRank[i])
            {
                return rankChars[i];
            }
        }
        return rankChars[rankChars.Length - 1];
    }

    private char getSign(int deathNumber)
    {
        if (deathNumber < deathsPerSign[0])
        {
            return '+';
        }
        else if (deathNumber < deathsPerSign[1])
        {
            return ' ';
        }
        else
        {
            return '-';
        }
    }
    // Thanks Kevin!
    private string convertToTimeString(float secs)
    {
        string time = "";

        float mins = (int)secs / 60;
        int sec = (int)(secs % 60);
        float milli = (float)Math.Round(secs - ((int)secs), 2);
        Debug.Log(milli);

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

        return time;
    }
}
