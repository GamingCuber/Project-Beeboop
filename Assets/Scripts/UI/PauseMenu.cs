using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuObject;

    private GameObject optionsOject;

    private GameObject settingsObject;

    private bool menuActive = false;

    private bool settingsActive = false;

    public PauseOption[] optionData;

    private GameObject[] optionGameObjects;

    public GameObject optionPre;

    private int curOption = 0;

    private GameObject player;

    private int loopedInt; //for turning off the looped ambience sound effect

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
        
    public GameObject switchImg;

    public float switchFrames;

    public int maxSwitchOpacity;

    private void Start()
    {
        optionGameObjects = new GameObject[7];

        optionsOject = menuObject.transform.GetChild(2).gameObject;
        settingsObject = menuObject.transform.GetChild(3).gameObject;

        for (int i = 0; i < 7; i++)
        {
            GameObject newOption = Instantiate(optionPre, optionsOject.transform); //put option as a child of the menu object in pause menu
            optionGameObjects[i] = newOption;
            optionGameObjects[i].transform.localPosition = optionPos[i];
            optionGameObjects[i].transform.localScale = new Vector3(0.7f, 0.7f, 1f);
        }

        player = GameObject.FindGameObjectWithTag("Player");
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
            if (!settingsActive)
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
                    switchEffect();
                    SoundManager.Instance.playSoundFX("crtClick", player.transform.position, 0, 10, 1);
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
                    switchEffect();
                    SoundManager.Instance.playSoundFX("crtClick", player.transform.position, 0, 10, 1);
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
                        case 1:
                            showSettings();
                            hideOptions();
                            break;

                    }
                }
            }
            else if (settingsActive)
            {
                // settings stuff
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
        SoundManager.Instance.playSoundFX("crtOn", player.transform.position, 0, 10, 1);
        SoundManager.Instance.playLoopedSound("crtAmbience", player.transform.position, 0, 10, 1f, out int index);
        loopedInt = index;
    }

    private void hideMenu()
    {
        resetMenu();
        menuObject.SetActive(false);
        GameManager.Instance.resumeGame();
        menuActive = false;
        SoundManager.Instance.playSoundFX("crtOff", player.transform.position, 0, 10, 1);
        SoundManager.Instance.stopLoopSound(loopedInt);
    }

    private void resetMenu()
    {
        curOption = 0;
        hideSettings();
        showOptions();
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
                return 5 + num;
            }
            else
            {
                return num;
            }
        }
        else
        {
            int num = curOption + 2;


            if (num >= optionData.Length)
            {
                return num % 5;
            }
            else
            {
                return num;
            }
        }
    }

    private void showSettings()
    {
        settingsActive = true;
        settingsObject.SetActive(true);
        settingsObject.GetComponent<SettingsMenu>().updateAllSettings();
    }

    public void hideSettings()
    {
        settingsActive = false;
        settingsObject.SetActive(false);
    }

    public void showOptions()
    {
        optionsOject.SetActive(true);
    }

    public void hideOptions()
    {
        optionsOject.SetActive(false);
    }


    private void switchEffect()
    {
        StartCoroutine(switchEffectCo());
    }

    private IEnumerator switchEffectCo()
    {
        float moveYDist = 200;

        float minX = 950;
        float maxX = -950;
        
        float minY = -518;
        float maxY = 518 - moveYDist;

        float Y = 0;

        float startingY = Random.Range(minY, maxY);

        float targetY = startingY + moveYDist;

        float curFrames = 0;

        float a = 255;

        RectTransform rect = switchImg.GetComponent<RectTransform>();

        RawImage img = switchImg.GetComponent<RawImage>();

        Vector3 newPos = rect.position;

        newPos.x = Random.Range(minX, maxX);

        Color32 startColor = img.color;
        startColor.a = (byte)255;
        img.color = startColor;

        switchImg.SetActive(true);

        while (curFrames != switchFrames)
        {
            curFrames++;

            Y = Mathf.Lerp(startingY, targetY, curFrames / switchFrames);
            newPos.y = Y;
            rect.transform.localPosition = newPos;

            if (curFrames > switchFrames/2)
            {
                a = Mathf.Lerp(maxSwitchOpacity, 0, (curFrames - (switchFrames / 2)) / (switchFrames / 2));
                Color32 c = img.color;
                c.a = (byte)a;
                img.color = c;
            }


            yield return new WaitForEndOfFrame();
        }

        switchImg.SetActive(false);
    }
}
