using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class BSideManager : MonoBehaviour
{
    public static BSideManager Instance;

    private Button backButton;

    public Transform optionsParent;

    public GameObject optionPre;

    public float moveTime;

    private GameObject[] options;

    private Coroutine optionMoveCo = null; //this one is the big coroutine that keeps track of which button is selected

    private Coroutine moveCo = null; //this is the coroutine to move the options

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        backButton = optionsParent.parent.parent.GetChild(0).GetComponent<Button>();

        StartCoroutine(waitForGameData());
    }

    private void Update()
    {
        if (optionsParent.gameObject.activeInHierarchy && optionMoveCo == null)
        {
            if (EventSystem.current.currentSelectedGameObject != backButton.gameObject)
            {
                optionMoveCo = StartCoroutine(moveOptions());
            }
        }
        else if (EventSystem.current.currentSelectedGameObject == backButton.gameObject && optionMoveCo != null)
        {
            StopCoroutine(optionMoveCo);
            optionMoveCo = null;
        }
    }

    private IEnumerator moveOptions()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        Button[] optionButtons = new Button[options.Length];

        int curButtonIndex = 0;

        float offsetMult = 1f;

        moveCo = null;

        for (int b = 0; b < optionButtons.Length; b++)
        {
            optionButtons[b] = options[b].transform.GetChild(1).GetComponent<Button>();
        }

        while(true)
        {
            if (optionButtons[curButtonIndex].gameObject != EventSystem.current.currentSelectedGameObject)
            {
                for (int i = 0; i < options.Length; i++)
                {
                    if (optionButtons[i].gameObject == EventSystem.current.currentSelectedGameObject)
                    {
                        stopMove();
                        curButtonIndex = i;
                        break;
                    }
                }
            }
            else
            {
                if (moveCo == null)
                {
                    if (curButtonIndex == 0)
                    {
                        offsetMult = 0f;
                    }
                    else if (curButtonIndex == options.Length - 1)
                    {
                        offsetMult = 1f;
                    }
                    else if (curButtonIndex > 0)
                    {
                        offsetMult = 1f;
                    }

                    moveCo = StartCoroutine(move(curButtonIndex, optionsParent.transform.localPosition, offsetMult));
                }
            }

            yield return wait;
        }
    }

    private IEnumerator move(int curInd, Vector3 initPos, float offsetMult)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;

        float time = moveTime;

        //i honestly have no idea how i got to this formula, i just guess and checked until it was right
        Vector3 targetPos = Vector3.zero + (Vector3.right * (curInd * (-275 * offsetMult) + (-275 * (curInd - 1)) * offsetMult));

        if (curInd == options.Length-1)
        {
            targetPos += Vector3.right * 275;
        }

        while (timer < time)
        {
            timer += Time.deltaTime;

            float x = Mathf.Lerp(initPos.x, targetPos.x, timer / time);

            optionsParent.transform.localPosition = Vector3.zero + (Vector3.right * x);

            yield return wait;
        }

        yield break;
    }

    private void stopMove()
    {
        if (moveCo != null)
        {
            StopCoroutine(moveCo);
        }
        moveCo = null;
    }

    private IEnumerator waitForGameData()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while (GameDataManager.Instance == null)
        {
            yield return wait;
        }

        //creating bside options in game time

        options = new GameObject[GameDataManager.Instance.levels.Length-1];

        for (int i = 1; i < GameDataManager.Instance.levels.Length; i++)
        {
            GameObject newOption = Instantiate(optionPre, optionsParent);

            newOption.transform.localPosition = new Vector3(-271 + 550*(i-1), 100, 0);

            BSideOption bsideOpt = newOption.GetComponent<BSideOption>();

            options[i - 1] = newOption;

            bsideOpt.optionData = GameDataManager.Instance.levels[i];

            bsideOpt.setupButton();

            newOption.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GameDataManager.Instance.levels[i].levelCover;
            newOption.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = GameDataManager.Instance.levels[i].levelName;
        }

        //connecting back button's nav with newly made buttons

        Debug.Log(backButton);

        Navigation backNav = backButton.navigation;
        backNav.selectOnRight = options[0].transform.GetChild(1).GetComponent<Button>();
        backNav.selectOnLeft = options[options.Length - 1].transform.GetChild(1).GetComponent<Button>();
        backButton.navigation = backNav;

        //setting up the button nav
        Navigation newNav = new Navigation();
        newNav.mode = Navigation.Mode.Explicit;

        for (int e = 0; e < options.Length; e++)
        {
            Button curButton = options[e].transform.GetChild(1).GetComponent<Button>();

            if (e == 0)
            {
                newNav.selectOnLeft = options[options.Length - 1].transform.GetChild(1).GetComponent<Button>();
            }
            else if (e == options.Length - 1)
            {
                newNav.selectOnRight = options[0].transform.GetChild(1).GetComponent<Button>();
            }

            if (newNav.selectOnLeft == null)
            {
                newNav.selectOnLeft = options[e - 1].transform.GetChild(1).GetComponent<Button>();
            }

            if (newNav.selectOnRight == null)
            {
                newNav.selectOnRight = options[e + 1].transform.GetChild(1).GetComponent<Button>();
            }


            newNav.selectOnUp = backButton;
            newNav.selectOnDown = backButton;

            curButton.navigation = newNav;

            newNav.selectOnDown = null;
            newNav.selectOnUp = null;
            newNav.selectOnLeft = null;
            newNav.selectOnRight = null;
        }

        yield break;
    }
}
