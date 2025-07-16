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

    public GameObject transitionImg;

    public float transitionTime;

    private void Start()
    {
        optionGameObjects = new GameObject[optionData.Length];

        for (int i = 0; i < optionData.Length; i++)
        {
            GameObject newOption = Instantiate(optionPre, menuObject.transform.GetChild(1)); //put option as a child of the menu object in pause menu
            newOption.transform.localPosition = new Vector3(-800, 400 - (200 * i), 0); //magic numbers = position on canvas that i found already
            optionGameObjects[i] = newOption;

            Transform t = newOption.transform;

            //name tmp
            t.GetChild(0).GetComponent<TMP_Text>().text = optionData[i].optionName;

            //description tmp
            t.GetChild(1).GetComponent<TMP_Text>().text = optionData[i].description;

            //channel number
            t.GetChild(2).GetComponent<TMP_Text>().text = optionData[i].channelNumber;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            transition();
        }

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
                updateMenu();
            }
            else if (Input.GetKeyDown(PlayerInputs.Instance.up) || Input.GetKeyDown(PlayerInputs.Instance.right))
            {
                if (curOption != optionData.Length-1)
                {
                    curOption++;
                }
                updateMenu();
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
        GameObject curGO = optionGameObjects[curOption];

        foreach(GameObject g in optionGameObjects)
        {
            Image curOptionBG = g.transform.GetChild(3).GetComponent<Image>();

            if (g != curGO)
            {
                Color32 unselectedColor = new Color32(140, 140, 140, 100);

                curOptionBG.color = unselectedColor;
            }
            else
            {
                Color32 selectedColor = new Color32(255, 255, 255, 100);

                curOptionBG.color = selectedColor;
            }
        }

        Image menuBG = menuObject.transform.GetChild(0).GetComponent<Image>();

        menuBG.sprite = optionData[curOption].backgroundImg;
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
