using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Playables;

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

    public static TutorialPopupManager Instance;
    private WaitForEndOfFrame wait = new WaitForEndOfFrame();
    public dataEnum[] dataOptions;
    
    private GameObject upgradePopUp;
    private TMP_Text descriptionText;
    private VideoPlayer upgradeVideo;
    private Image inputIcon;
    private TMP_Text inputText;
    private TMP_Text tabText;

    private RectTransform panelTransform;
    public float moveTime;
    public Vector3 hiddenPosition;
    public Vector3 showingPosition;

    private PlayableDirector director;
    public PlayableAsset show;
    public PlayableAsset hide;
    private TutorialData curData;

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
        
        GameObject input = tutorialBody.transform.Find("Input").gameObject;

        inputIcon = input.transform.Find("InputIcon").GetComponent<Image>();
        inputText = input.transform.Find("InputText2").GetComponent<TMP_Text>();

        tabText = upgradePopUp.transform.Find("TopBar").Find("System Update").GetComponent<TMP_Text>();

        director = upgradePopUp.GetComponent<PlayableDirector>();

        hidePanel();
    }

    public void showTutorial(tutorialOptions option)
    {
        upgradePopUp.SetActive(true);

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
        inputIcon.sprite = data.inputIcon;
        inputText.text = "to " + data.inputName + "!";

        StartCoroutine(movePanel(hiddenPosition, showingPosition));
    }

    private void hidePanel()
    {
        panelTransform.position = showingPosition;
        upgradePopUp.SetActive(false);
    }

    private IEnumerator movePanel(Vector3 from, Vector3 to)
    {
        float timer = 0;
        float time = moveTime;

        StartCoroutine(namePanelEffect());

        if (PlayerStateManager.Instance.getState().wantsTimer) TimeTextManager.Instance.hideTimer();

        while (timer < time)
        {
            timer += Time.deltaTime;

            float percent = Mathf.Sin(Mathf.PI/2 * timer/time);

            Vector3 pos = Vector3.Lerp(from, to, percent);

            panelTransform.anchoredPosition = pos;

            yield return wait;
        }
    }

    private IEnumerator namePanelEffect()
    {
        float waitTime = 0.5f;

        float dotTime = 0.75f;
        StartCoroutine(writeToPanel(". . .", dotTime));
        yield return new WaitForSecondsRealtime(dotTime + 1.5f * waitTime);      

        float sysUpdTime = 0.5f;
        StartCoroutine(writeToPanel("SYSTEM UPDATE", sysUpdTime));
        yield return new WaitForSecondsRealtime(sysUpdTime + waitTime);

        float upgTime = 0.5f;
        StartCoroutine(writeToPanel(curData.topText.ToUpper(), upgTime));
        yield return new WaitForSecondsRealtime(upgTime + waitTime);   

        director.playableAsset = show;
        director.Play();
        upgradeVideo.Play();

        StartCoroutine(waitToHide());
        yield break;
    }

    private IEnumerator writeToPanel(string str, float time)
    {
        char[] cArr = str.ToCharArray();
        float timePerChar = time/cArr.Length;

        float timer = 0;
        int i = 0;

        string s = "";

        while (i < cArr.Length)
        {
            if (timer < timePerChar) timer += Time.deltaTime;
            else 
            {
                s += cArr[i];
                tabText.text = s;
                ++i;

                timer = 0;
            }

            yield return wait;
        }
    }

    private IEnumerator waitToHide()
    {
        float tutorialTime = 4f;
        yield return new WaitForSecondsRealtime(tutorialTime);

        director.playableAsset = hide;
        director.Play();
        
        if (PlayerStateManager.Instance.getState().wantsTimer) TimeTextManager.Instance.showTimer();

        yield return new WaitForSecondsRealtime(1f);
        hidePanel();
    }
}
