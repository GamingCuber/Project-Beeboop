using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class TutorialPopupManager : MonoBehaviour
{
    [Serializable]
    public struct dataEnum
    {
        public tutorialOptions option;
        public TutorialData data;
    }

    public enum tutorialOptions
    {
        doubleJump,
        dash,
        hook
    }
    private tutorialOptions currentTutorial;

    public static TutorialPopupManager Instance;
    private WaitForEndOfFrame wait = new WaitForEndOfFrame();
    public dataEnum[] dataOptions;

    private GameObject upgradePopUp;
    private TMP_Text descriptionText;
    private VideoPlayer upgradeVideo;

    private TMP_Text tabText;

    private Image defaultInputIcon;
    private TMP_Text defaultInputText;

    private Image dashInputIcon;
    private TMP_Text dashInputText;

    private RectTransform panelTransform;
    public float moveTime;
    public Vector3 hiddenPosition;
    public Vector3 showingPosition;

    private PlayableDirector director;
    public PlayableAsset show;
    public PlayableAsset hide;
    private TutorialData curData;
    public bool isTutorialUIUp;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        upgradePopUp = GameObject.FindGameObjectWithTag("UpgradePopUp");

        GameObject tutorialBody = upgradePopUp.transform.Find("Mask").Find("TutorialBody").gameObject;

        descriptionText = tutorialBody.transform.Find("DescriptionText").GetComponent<TMP_Text>();
        upgradeVideo = upgradePopUp.transform.Find("UpgradeVideoPlayer").GetComponent<VideoPlayer>();
        panelTransform = upgradePopUp.GetComponent<RectTransform>();

        GameObject defaultInput = tutorialBody.transform.Find("DefaultInput").gameObject;

        defaultInputIcon = defaultInput.transform.Find("InputIcon").GetComponent<Image>();
        defaultInputText = defaultInput.transform.Find("InputText2").GetComponent<TMP_Text>();

        GameObject dashInput = tutorialBody.transform.Find("DashInput").gameObject;

        dashInputIcon = dashInput.transform.Find("InputIcon").GetComponent<Image>();
        dashInputText = dashInput.transform.Find("InputText2").GetComponent<TMP_Text>();

        tabText = upgradePopUp.transform.Find("TopBar").Find("System Update").GetComponent<TMP_Text>();

        director = upgradePopUp.GetComponent<PlayableDirector>();

        hidePanel();
        isTutorialUIUp = false;
    }

    private void Update()
    {
        if (PlayerStateManager.Instance.state.pausedGame)
        {
            turnOffPanel();
        }
        else
        {
            turnOnPanel();
        }
    }

    public void showTutorial(tutorialOptions option)
    {
        isTutorialUIUp = true;
        upgradePopUp.SetActive(true);
        currentTutorial = option;

        TutorialData data = null;

        foreach (dataEnum d in dataOptions)
        {
            if (d.option == option)
            {
                data = d.data;
                curData = data;
                break;
            }
        }

        if (data == null)
        {
            Debug.Log("make the scriptable object dummy");
            return;
        }

        descriptionText.text = data.descriptionText;
        upgradeVideo.clip = data.tutorialVideo;

        //ok sound effect a lil loud in comparison to everything else
        //so imma play sound with the other function... its kinda scuffed
        
        //SoundManager.Instance.playPlayerSound("tutorialUIPop");
        SoundManager.Instance.playSoundFX("tutorialUIPop", Vector3.zero, 0, 9999f, 0.25f, true);

        if (option == tutorialOptions.dash)
        {
            GameObject dashInput = dashInputIcon.transform.parent.gameObject;
            dashInput.SetActive(true);
            dashInputIcon.sprite = data.inputIcon;
            dashInputText.text = "to " + data.inputName + "!";
        }
        else
        {
            GameObject defaultInput = defaultInputIcon.transform.parent.gameObject;
            defaultInput.SetActive(true);
            defaultInputIcon.sprite = data.inputIcon;
            defaultInputText.text = "to " + data.inputName + "!";
        }

        StartCoroutine(movePanel(hiddenPosition, showingPosition));
    }

    public void hidePanel()
    {
        panelTransform.position = showingPosition;
        upgradePopUp.SetActive(false);
        isTutorialUIUp = false;
    }

    private IEnumerator movePanel(Vector3 from, Vector3 to)
    {
        float timer = 0;
        float time = moveTime;


        Vector3 pos = to;

        StartCoroutine(namePanelEffect());

        if (PlayerStateManager.Instance.getState().wantsTimer) TimeTextManager.Instance.hideTimer();

        while (timer < time)
        {
            if (!PlayerStateManager.Instance.state.pausedGame)
            {
                timer += Time.deltaTime;

                float percent = Mathf.Sin(Mathf.PI / 2 * timer / time);

                pos = Vector3.Lerp(from, to, percent);

                panelTransform.anchoredPosition = pos;
            }
            else
            {
                panelTransform.anchoredPosition = pos;
            }
            yield return wait;
        }

    }

    private IEnumerator namePanelEffect()
    {
        float waitTime = 0.5f;

        float dotTime = 0.5f;

        if (!PlayerStateManager.Instance.state.pausedGame)
        {
            StartCoroutine(writeToPanel(". . .", dotTime));
            yield return new WaitForSecondsRealtime(dotTime + 1.25f * waitTime);

            float sysUpdTime = 0.45f;
            StartCoroutine(writeToPanel("SYSTEM UPDATE", sysUpdTime));
            yield return new WaitForSecondsRealtime(sysUpdTime + waitTime);

            float upgTime = 0.35f;
            StartCoroutine(writeToPanel(curData.topText.ToUpper(), upgTime));
            yield return new WaitForSecondsRealtime(upgTime + waitTime);

            director.playableAsset = show;
            director.Play();

            if (!upgradeVideo.enabled) upgradeVideo.enabled = true;
            upgradeVideo.Play();

            yield return new WaitForSecondsRealtime(1.5f);

            StartCoroutine(waitToHide());

            yield break;
        }
        else
        {
            yield return null;
        }

    }

    private IEnumerator writeToPanel(string str, float time)
    {
        char[] cArr = str.ToCharArray();
        float timePerChar = time / cArr.Length;

        float timer = 0;
        int i = 0;

        string s = "";

        while (i < cArr.Length)
        {

            if (timer < timePerChar) timer += Time.deltaTime;
            else
            {
                if (!PlayerStateManager.Instance.state.pausedGame)
                {
                    s += cArr[i];
                    tabText.text = s;
                    ++i;
                }
                else
                {
                    tabText.text = tabText.text;
                }


                timer = 0;
            }

            yield return wait;
        }
    }

    private IEnumerator waitToHide()
    {
        var gotInput = false;
        var passedCollider = TutorialCollider.Instance == null ? true : false;
        
        while (!gotInput || !passedCollider)
        {
            switch (currentTutorial)
            {
                case tutorialOptions.dash:
                    if (PlayerInputs.Instance.playerController.Player.Dash.WasPressedThisFrame()) gotInput = true;
                    yield return wait;
                    break;
                case tutorialOptions.hook:
                    if (PlayerInputs.Instance.playerController.Player.Hook.WasPressedThisFrame()) gotInput = true;
                    yield return wait;
                    break;
                case tutorialOptions.doubleJump:
                    if (PlayerInputs.Instance.playerController.Player.Jump.WasPressedThisFrame()) gotInput = true;
                    yield return wait;
                    break;
            }

            if (!passedCollider) passedCollider = TutorialCollider.Instance.getReachedPoint();

            if (gotInput && passedCollider) break;
        }

        if (!PlayerStateManager.Instance.state.pausedGame && gotInput)
        {
            director.playableAsset = hide;
            director.Play();

            if (PlayerStateManager.Instance.getState().wantsTimer) TimeTextManager.Instance.showTimer();

            yield return new WaitForSecondsRealtime(1f);
            hidePanel();
        }
        else
        {
            yield return null;
        }


    }

    private void turnOffPanel()
    {
        var ParentObject = gameObject;

        for (var i = 0; i < ParentObject.transform.childCount; i++)
        {
            ParentObject.transform.GetChild(i).localScale = new Vector3(0, 0, 0);
        }
    }

    private void turnOnPanel()
    {
        var ParentObject = gameObject;
        for (var i = 0; i < ParentObject.transform.childCount; i++)
        {
            ParentObject.transform.GetChild(i).localScale = new Vector3(1, 1, 1);
        }
    }
}
