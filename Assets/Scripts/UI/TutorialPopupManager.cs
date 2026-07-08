using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

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

    private RectTransform panelTransform;
    public float moveTime;
    public Vector3 hiddenPosition;
    public Vector3 showingPosition;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        upgradePopUp = GameObject.FindGameObjectWithTag("UpgradePopUp");
        descriptionText = upgradePopUp.transform.Find("DescriptionText").GetComponent<TMP_Text>();
        upgradeVideo = upgradePopUp.transform.Find("UpgradeVideoPlayer").GetComponent<VideoPlayer>();
        panelTransform = upgradePopUp.GetComponent<RectTransform>();
        
        GameObject input = upgradePopUp.transform.Find("Input").gameObject;

        inputIcon = input.transform.Find("InputIcon").GetComponent<Image>();
        inputText = input.transform.Find("InputText2").GetComponent<TMP_Text>();

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
        upgradeVideo.Play();
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

        while (timer < time)
        {
            timer += Time.deltaTime;

            float percent = Mathf.Sin(Mathf.PI/2 * timer/time);

            Vector3 pos = Vector3.Lerp(from, to, percent);

            panelTransform.anchoredPosition = pos;

            yield return wait;
        }
    }
}
