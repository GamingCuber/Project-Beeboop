using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuObject;

    private bool menuActive = false;

    public PauseOption[] optionData;

    private GameObject[] optionGameObjects;

    public GameObject optionPre;

    private int curOption = 0;

    private Vector3[] optionPos = //this is a bad way to do this but
    {
        //new Vector3(-1245, 380, 0),
        //new Vector3(-858, 380, 0),
        //new Vector3(-470, 380, 0),
        //new Vector3(0, 350, 0),
        //new Vector3(470, 380, 0),
        //new Vector3(858, 380, 0),
        //new Vector3(1245, 380, 0)

        new Vector3(-1024, 470, 0),
        new Vector3(-760, 416, 0),
        new Vector3(-620, 240, 0),
        new Vector3(-513, 0, 0),
        new Vector3(-620, -240, 0),
        new Vector3(-760, -416, 0),
        new Vector3(-1024, -470, 0)
    };
        
    public GameObject transitionImg;

    public float transitionTime;

    private void Start()
    {
        optionGameObjects = new GameObject[7];

        for (int i = 0; i < 7; i++)
        {
            GameObject newOption = Instantiate(optionPre, menuObject.transform.GetChild(1)); //put option as a child of the menu object in pause menu
            optionGameObjects[i] = newOption;
            optionGameObjects[i].transform.localPosition = optionPos[i];
            optionGameObjects[i].transform.localScale = new Vector3(0.7f, 0.7f, 1f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !menuActive)
        {
            showMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menuActive)
        {
            hideMenu();
        }

        if (menuActive)
        {
            //to move the selected thing
            if (Input.GetKeyDown(PlayerInputs.Instance.down) || Input.GetKeyDown(PlayerInputs.Instance.left))
            {
                if (curOption != 0)
                {
                    curOption--;
                }
                else
                {
                    curOption = optionData.Length - 1;
                }

                pushList();
                updateMenu();
                shiftOptions(1);
            }
            else if (Input.GetKeyDown(PlayerInputs.Instance.up) || Input.GetKeyDown(PlayerInputs.Instance.right))
            {
                if (curOption != optionData.Length - 1)
                {
                    curOption++;
                }
                else
                {
                    curOption = 0;
                }

                pullList();
                updateMenu();
                shiftOptions(-1);
            }

            //to select an option
            if (Input.GetKeyDown(PlayerInputs.Instance.jump))
            {
                switch (curOption)
                {
                    case 0: //resume
                        resetMenu();
                        hideMenu();
                        break;
                }
            }
        }
    }

    private void showMenu()
    {
        initializeOptions();
        updateMenu();
        GameManager.Instance.pauseGame();
        menuObject.SetActive(true);
        menuActive = true;
    }

    private void hideMenu()
    {
        resetMenu();
        menuObject.SetActive(false);
        GameManager.Instance.resumeGame();
        menuActive = false;
    }

    private void resetMenu()
    {
        curOption = 0;
        updateMenu();
    }

    private void updateMenu()
    {
        Image menuBG = menuObject.transform.GetChild(0).GetComponent<Image>();

        Color32 color;

        int i = 0;

        foreach (GameObject g in optionGameObjects)
        {
            if (i != 3)
            {
                color = new Color32(140, 140, 140, 100);
            }
            else
            {
                color = new Color32(255, 255, 255, 100);
            }

            g.transform.GetChild(3).GetComponent<Image>().color = color;

            i++;
        }
        

        menuBG.sprite = optionData[curOption].backgroundImg;
    }

    private void shiftOptions(int dir)
    {
        StartCoroutine(shiftOptionsCo(dir));
    }

    IEnumerator shiftOptionsCo(int dir) //-1 up, 1 down
    {
        float startTime = Time.realtimeSinceStartup;

        float timeChanged = 0;

        float moveTime = 0.2f;

        Vector3[] initPos = new Vector3[7];

        int i = 0;
        foreach (GameObject option in optionGameObjects)
        {
            initPos[i] = option.transform.localPosition;
            i++;
        }

        int exclude = 0;
        if (dir == -1)
        {
            exclude = optionGameObjects.Length-1;
            optionGameObjects[optionGameObjects.Length-1].SetActive(false);
        }
        else if (dir == 1)
        {
            exclude = 0;
            optionGameObjects[0].SetActive(false);
        }

        for (int ii = 0; ii < optionGameObjects.Length; ii++)
        {
            if (ii != exclude)
            {
                optionGameObjects[ii].SetActive(true);
            }
        }

        updateNewOption(dir);

        while (true)
        {
            timeChanged = Time.realtimeSinceStartup - startTime;
            for (int e = 0; e < optionGameObjects.Length; e++)
            {
                float newX = Mathf.Lerp(initPos[e].x, optionPos[e].x, timeChanged / moveTime);
                float newY = Mathf.Lerp(initPos[e].y, optionPos[e].y, timeChanged / moveTime);
                Vector3 newPos = optionGameObjects[e].transform.localPosition;
                newPos.y = newY;
                newPos.x = newX;

                optionGameObjects[e].transform.localPosition = newPos;

                if (e == 3)
                {
                    float newScale = Mathf.Lerp(.7f, 1f, timeChanged / moveTime);
                    optionGameObjects[e].transform.localScale = new Vector3(newScale, newScale, newScale);
                }
                else if (e == 2 && dir == -1 || e == 4 && dir == 1)
                {
                    //optionGameObjects[e].transform.GetChild(3).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    float newScale = Mathf.Lerp(1f, .75f, timeChanged / moveTime);
                    optionGameObjects[e].transform.localScale = new Vector3(newScale, newScale, newScale);
                }
                else if (e == 1 && dir == -1 || e == 5 && dir == 1)
                {
                    float newScale = Mathf.Lerp(.75f, .4f, timeChanged / moveTime);
                    optionGameObjects[e].transform.localScale = new Vector3(newScale, newScale, newScale);
                }
                else if (e == 2 && dir == 1 || e == 4 && dir == -1)
                {
                    float newScale = Mathf.Lerp(.4f, .75f, timeChanged / moveTime);
                    optionGameObjects[e].transform.localScale = new Vector3(newScale, newScale, newScale);
                }
            }

            if (timeChanged >= moveTime)
            {
                yield break;
            }

            yield return null;

        }
    }

    private void pushList() //pushes the list to the right, last index -> first
    {
        GameObject lastCover = optionGameObjects[optionGameObjects.Length - 1];

        for (int i = optionGameObjects.Length - 1; i > 0; i--)
        {
            optionGameObjects[i] = optionGameObjects[i - 1];
        }

        optionGameObjects[0] = lastCover;
    }

    private void pullList() //pulls list to the left, first index -> last 
    {
        GameObject firstCover = optionGameObjects[0];

        for (int i = 0; i < optionGameObjects.Length - 1; i++)
        {
            optionGameObjects[i] = optionGameObjects[i + 1];
        }

        optionGameObjects[optionGameObjects.Length - 1] = firstCover;

    }

    //change the option coming in
    private void updateNewOption(int dir)
    {
        if (dir == 1)
        {
            updateOptionData(optionGameObjects[1], optionData[wrapNum(dir)]);
        }
        else
        {
            updateOptionData(optionGameObjects[optionGameObjects.Length-2], optionData[wrapNum(dir)]);
        }
    }

    //this is buns but it works so
    private void initializeOptions()
    {
        shiftOptions(1);
        shiftOptions(-1);

        updateOptionData(optionGameObjects[3], optionData[0]);
        updateOptionData(optionGameObjects[4], optionData[1]);
        updateOptionData(optionGameObjects[5], optionData[2]);
        updateOptionData(optionGameObjects[2], optionData[4]);
        updateOptionData(optionGameObjects[1], optionData[3]);
    }

    private void updateOptionData(GameObject option, PauseOption data)
    {
        Transform trans = option.transform;

        trans.GetChild(0).GetComponent<TMP_Text>().text = data.optionName;
        trans.GetChild(1).GetComponent<TMP_Text>().text = data.description;
        trans.GetChild(2).GetComponent<TMP_Text>().text = data.channelNumber;
    }

    private int wrapNum(int dir)
    {
        if (dir == 1)
        {
            int num = curOption - 2;

            if (num < 0)
            {
                Debug.Log(5 + num);
                return 5 + num;
            }
            else
            {
                Debug.Log(num);
                return num;
            }
        }
        else
        {
            int num = curOption + 2;


            if (num >= optionData.Length)
            {
                Debug.Log(num % 5);
                return num % 5;
            }
            else
            {
                Debug.Log(num);
                return num;
            }
        }
    } 

    private void transition()
    {
        StartCoroutine(zoomInTransition());
    }
    private IEnumerator zoomInTransition()
    {
        transitionImg.SetActive(true);

        RectTransform rect = transitionImg.GetComponent<RectTransform>();

        Vector2 minVector = Vector2.zero;
        Vector2 maxVector = Vector2.zero;

        float timer = 0;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;

            float x = Mathf.Lerp(0, 1920/2, timer / transitionTime);
            float y = Mathf.Lerp(0, 1080 / 2, timer / transitionTime);

            minVector.x = x;
            maxVector.x = -x;
            minVector.y = y;
            maxVector.y = -y;

            rect.offsetMin = minVector;
            rect.offsetMax = maxVector;

            yield return new WaitForEndOfFrame();
        }

        transitionImg.SetActive(false);
        yield break;
    }
}
