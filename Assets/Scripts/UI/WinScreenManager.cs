using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WinScreenManager : MonoBehaviour
{
    #region Results variables
    [Header("Results Variables")]

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

    #endregion

    #region Leaderboard variables
    [Header("Leaderboard Variables")]

    public GameObject scorePoolParent;

    public GameObject scorePre;

    private GameObject[] scores;

    private GameObject curScoreObj; //the actual like rank score thing in the leaderboard

    private Dictionary<GameObject, int> nameObjs = new Dictionary<GameObject, int>();

    //so we can use these from the board data manager
    public struct Score
    {
        public string name;
        public float time;

        public Score(string n, float t)
        {
            name = n;
            time = t;
        }
    }

    #endregion

    [Header("ignore")]
    //for easter egg
    public Image vidio;
    public Sprite jose;
    public GameObject replayGO;
    public GameObject quitGO;
    public GameObject nameSelectionGO;
    public GameObject allScoresGO;

    private void Start()
    {
        if (rankChars.Length != secondsPerRank.Length)
        {
            Debug.LogError("Make sure both arrays are the same size!!!!");
        }

        hideButtons();

        Invoke(nameof(doResults), 1f);
    }

    public void quitButton()
    {
        Application.Quit();
    }

    public void replayButton()
    {
        LevelTransition.Instance.doTransition("StartMenu");
    }

    //hide quit n replay so they have to do smt about high score
    public void hideButtons()
    {
        replayGO.SetActive(false);
        quitGO.SetActive(false);
    }

    public void showLeaderboard()
    {
        nameSelectionGO.SetActive(true);
        EventSystem.current.SetSelectedGameObject(nameSelectionGO.transform.GetChild(1).gameObject); //sets selected to first letter
        StartCoroutine(checkInputs());
        nameSelectionGO.transform.parent.GetComponent<Animator>().SetTrigger("show");
        Invoke(nameof(startMovingScores), 2f);
    }

    public void showButtons()
    {
        replayGO.SetActive(true);
        quitGO.SetActive(true);
        nameSelectionGO.SetActive(false);
        EventSystem.current.SetSelectedGameObject(replayGO); //sets selected to replay button
    }

    #region Results stuff

    private void doResults()
    {
        initializeBoard();

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
    // Dang it.
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
        if (milli <= 10)
        {
            time += "0" + milli;
        }
        else
        {
            time += milli;
        }

        return time.Substring(0, 7);
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

        if (grade == "S+" || grade == "V+")
        {
            vidio.sprite = jose;
        }

        rankText.gameObject.SetActive(true);
        rankText.text = grade;
        rankText.gameObject.GetComponent<Animator>().SetTrigger("Reveal");

        yield return new WaitForSecondsRealtime(1f);

        showLeaderboard();

        yield break;
    }

    private IEnumerator animateTextCo(TMP_Text text, float num)
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.05f);
        WaitForSecondsRealtime fasterWait = new WaitForSecondsRealtime(0.025f);
        WaitForSecondsRealtime fastestWait = new WaitForSecondsRealtime(0.01f);

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

            if (num <= 25)
            {
                yield return wait;
            }
            else if (num <= 50)
            {
                yield return fasterWait;
            }
            else
            {
                yield return fastestWait;
            }

        }

        animDone = true;
        yield break;
    }

    #endregion

    private void initializeBoard()
    {
        LeaderboardDataManager.levelScoreData wrap = LeaderboardDataManager.Instance.getCurData();

        Debug.Log(LeaderboardDataManager.Instance.curLevelData.scores.Count);

        //weird name so we know which one the person just got
        wrap.scores.Add(new LeaderboardDataManager.Score("curScore", PlayerStateManager.Instance.getState().totalTime));

        Debug.Log(LeaderboardDataManager.Instance.curLevelData.scores.Count);

        wrap = sortWrap(wrap);

        scores = new GameObject[wrap.scores.Count];

        for (int i = 0; i < wrap.scores.Count; i++)
        {
            GameObject newObj = Instantiate(scorePre, scorePoolParent.transform);

            scores[i] = newObj;

            GameObject rankGO = newObj.transform.GetChild(0).gameObject;
            GameObject nameGO = newObj.transform.GetChild(1).gameObject;
            GameObject timeGO = newObj.transform.GetChild(2).gameObject;

            TMP_Text rankText = rankGO.GetComponent<TMP_Text>();
            TMP_Text nameText = nameGO.GetComponent<TMP_Text>();
            TMP_Text timeText = timeGO.GetComponent<TMP_Text>();

            string name = "";

            if (wrap.scores[i].name == "curScore") //here to cache current score obj and fix name
            {
                curScoreObj = newObj;
                name = "AAA";

                Color32 highlightColor = new Color32((byte)44, (byte)136, (byte)86, (byte)255);
                rankText.color = highlightColor;
                nameText.color = highlightColor;
                timeText.color = highlightColor;
            }
            else
            {
                name = wrap.scores[i].name;
            }

            rankText.text = (i + 1).ToString(); //i+1 to start at rank 1 instead of 0
            nameText.text = name;
            timeText.text = convertToTimeString(wrap.scores[i].time);

            newObj.transform.localPosition = new Vector3(12f, 285 - 65 * i, 0f); //numbers found by just looking in editor
        }
    }

    public LeaderboardDataManager.levelScoreData sortWrap(LeaderboardDataManager.levelScoreData wrap)
    {
        LeaderboardDataManager.levelScoreData returnWrap = wrap;

        //bubble sort to get scores in order from least to greatest
        for (int i = 1; i < returnWrap.scores.Count; i++)
        {
            for (int j = 0; j < returnWrap.scores.Count - 1; j++)
            {
                if (returnWrap.scores[j].time > returnWrap.scores[j + 1].time)
                {
                    LeaderboardDataManager.Score temp = returnWrap.scores[j];
                    returnWrap.scores[j] = returnWrap.scores[j + 1];
                    returnWrap.scores[j + 1] = temp;
                }
            }
        }

        return returnWrap;
    }

    public void saveScore()
    {
        cleanWrap();
        updateBoardColor(false);
        LeaderboardDataManager.Instance.addScoreToJSON(getName(), PlayerStateManager.Instance.getState().totalTime);
    }

    public void cancelScore()
    {
        updateBoardColor(true);
    }

    private void cleanWrap() //get rid of duplicate score value (that was made to cache curScoreObj, w/ name of curScore)
    {
        var wrap = LeaderboardDataManager.Instance.getCurData();

        for (int i = 0; i < wrap.scores.Count; i++)
        {
            if (wrap.scores[i].name == "curScore")
            {
                wrap.scores.RemoveAt(i);
                break;
            }
        }
    }

    private string getName()
    {
        char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        string name = "";

        foreach (KeyValuePair<GameObject, int> kv in nameObjs)
        {
            name += letters[kv.Value];
        }
        return name;
    }

    //to update the name in the leaderboard when theyre switching name
    private void updateBoardName(string name)
    {
        curScoreObj.transform.GetChild(1).GetComponent<TMP_Text>().text = name;
    }

    //to update the color of the curscore visually to have feedback on save/cancel
    private void updateBoardColor(bool cancelled)
    {
        Color32 textColor;

        if (cancelled)
        {
            textColor = Color.black;
        }
        else
        {
            textColor = Color.white;
        }

        for (int i = 0; i < curScoreObj.transform.childCount; i++)
        {
            curScoreObj.transform.GetChild(i).GetComponent<TMP_Text>().color = textColor;
        }

        showButtons(); //doing it here cuz cancel and save calls here lololo
    }

    private void startMovingScores()
    {
        StartCoroutine(moveScores());
    }

    //moves the leaderboard so you can see your score if youre lower than top 10
    private IEnumerator moveScores()
    {
        int index = 0;
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == curScoreObj)
            {
                index = i;
                break;
            }
        }

        if (index <= 10)
        {
            yield break;
        }

        Vector3 goalPos = new Vector3(0f, 66 * (index - 5), 0f);
        Vector3 curPos = Vector3.zero;

        int distFromBottom = scores.Length - index;

        if (distFromBottom < 5)
        {
            goalPos += Vector3.down * 66 * (5 - distFromBottom);
        }

        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;
        float moveTime = 0.75f;

        while (timer < moveTime)
        {
            timer += Time.deltaTime;

            float y = Mathf.Lerp(0, goalPos.y, Mathf.Sin(Mathf.PI / 2 * (timer / moveTime)));
            curPos.y = y;

            allScoresGO.transform.localPosition = curPos;

            yield return wait;
        }

        yield break;
    }

    //for leaderboard name selection
    private IEnumerator checkInputs()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        var tags = GameObject.FindGameObjectsWithTag("NameSelect");

        for (int i = 0; i < tags.Length; i++)
        {
            nameObjs.Add(tags[i], 0);
        }

        GameObject lastButton = EventSystem.current.firstSelectedGameObject;

        GameObject curSelected = lastButton;

        while (true)
        {
            curSelected = EventSystem.current.currentSelectedGameObject;

            if (curSelected == null)
            {
                EventSystem.current.SetSelectedGameObject(lastButton);
            }
            else if (lastButton != curSelected)
            {
                lastButton = curSelected;
            }

            if (curSelected != null && nameObjs.ContainsKey(curSelected))
            {
                if (PlayerInputs.Instance.playerController.Player.Up.WasPressedThisFrame())
                {
                    if (nameObjs[curSelected] - 1 < 0)
                    {
                        nameObjs[curSelected] = letters.Length - 1;
                    }
                    else
                    {
                        nameObjs[curSelected] -= 1;
                    }
                    curSelected.GetComponent<TMP_Text>().text = letters[nameObjs[curSelected]].ToString();
                    updateBoardName(getName());
                    StartCoroutine(checkForHold(1, curSelected));
                }
                else if (PlayerInputs.Instance.playerController.Player.Down.WasPressedThisFrame())
                {
                    if (nameObjs[curSelected] + 1 == letters.Length)
                    {
                        nameObjs[curSelected] = 0;
                    }
                    else
                    {
                        nameObjs[curSelected] += 1;
                    }
                    curSelected.GetComponent<TMP_Text>().text = letters[nameObjs[curSelected]].ToString();
                    updateBoardName(getName());
                    StartCoroutine(checkForHold(-1, curSelected));
                }
            }

            yield return wait;
        }
    }

    private IEnumerator checkForHold(int dir, GameObject curSelected) //dir: 1 is up, -1 is down
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;

        float waitTime = 0.75f; //time to wait before we go fast when theyre holding

        while (timer < waitTime)
        {
            timer += Time.deltaTime;

            if (!PlayerInputs.Instance.pressingUpButton && !PlayerInputs.Instance.pressingDownButton)
            {
                yield break;
            }

            yield return wait;
        }

        //thisll run if theyre for sure holding down a direction

        WaitForSecondsRealtime holdWait = new WaitForSecondsRealtime(0.1f);

        char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        while (true)
        {
            if (dir == 1 && PlayerInputs.Instance.pressingUpButton)
            {
                if (nameObjs[curSelected] - 1 < 0)
                {
                    nameObjs[curSelected] = letters.Length - 1;
                }
                else
                {
                    nameObjs[curSelected] -= 1;
                }
                curSelected.GetComponent<TMP_Text>().text = letters[nameObjs[curSelected]].ToString();
                updateBoardName(getName());
            }
            else if (dir == -1 && PlayerInputs.Instance.pressingDownButton)
            {
                if (nameObjs[curSelected] + 1 == letters.Length)
                {
                    nameObjs[curSelected] = 0;
                }
                else
                {
                    nameObjs[curSelected] += 1;
                }
                curSelected.GetComponent<TMP_Text>().text = letters[nameObjs[curSelected]].ToString();
                updateBoardName(getName());
            }
            else
            {
                yield break;
            }

            yield return holdWait;
        }
    }
}
