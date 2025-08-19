using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinScreenManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text deathText;
    [SerializeField]
    private TMP_Text minText;
    [SerializeField]
    private TMP_Text secText;
    [SerializeField]
    private TMP_Text milText;
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

    private bool animDone = false; //for waiting when revealing deaths n tme

    public void quitButton()
    {
        Application.Quit();
    }

    public void replayButton()
    {
        LevelTransition.Instance.doTransition("StartMenu");
    }

    private void Start()
    {

        if (rankChars.Length != secondsPerRank.Length)
        {
            Debug.LogError("Make sure both arrays are the same size!!!!");
        }

        Invoke(nameof(doResults), 1f);
    }

    private void doResults()
    {
        int deathNumber = PlayerStateManager.Instance.getState().deathNumber;
        float totalTime = PlayerStateManager.Instance.getState().totalTime;

        float mins = (int)totalTime / 60;
        int sec = (int)(totalTime % 60);
        float milli = (float)Math.Round(totalTime - ((int)totalTime), 2);
        milli *= 100;
        milli = (int)milli;

        char currentSign = getSign(deathNumber);
        char currentLetter = getLetter(totalTime);
        string grade = string.Format("{0}", string.Format("{0}{1}", currentLetter, currentSign));

        TMP_Text[] text = { deathText, minText, secText, milText };
        float[] vals = { deathNumber, mins, sec, milli };

        StartCoroutine(revealResultsCo(text, vals, grade));
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
    // $20.
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

    //lol this is dumb but itll work
    private IEnumerator revealResultsCo(TMP_Text[] text, float[] vals, string grade)
    {
        WaitForSecondsRealtime animDelay = new WaitForSecondsRealtime(0.25f);
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        int index = 0;
        animDone = true;
        
        while (index < text.Length)
        {
            if (animDone)
            {
                yield return animDelay;
                StartCoroutine(animateTextCo(text[index], vals[index]));
                index++;
            }
            else
            {
                yield return wait;
            }
        }

        while (!animDone)
        {
            yield return wait;
        }
        
        yield return new WaitForSecondsRealtime(0.5f);

        rankText.gameObject.SetActive(true);
        rankText.text = grade;
        rankText.gameObject.GetComponent<Animator>().SetTrigger("Reveal");

        yield break;
    }

    private IEnumerator animateTextCo(TMP_Text text, float num)
    {
        animDone = false;

        float showingNum = 0;

        string showingText = "";

        while (showingNum < num)
        {
            showingText = "";
            showingNum++;

            //the mintext and death thing is there JUST bc it looks cleaner without the 0
            if (showingNum < 10 && text != minText && text != deathText)
            {
                showingText = "0" + showingNum;
            }
            else
            {
                showingText += showingNum;
            }

            text.text = showingText;

            yield return new WaitForSecondsRealtime(0.05f);
        }

        animDone = true;
        yield break;
    }
}
