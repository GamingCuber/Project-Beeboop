using System.Collections;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayerCollectScript : MonoBehaviour
{
    [SerializeField]
    private float secondsUntilDisable;
    [SerializeField]
    private string topHookText;
    [SerializeField]
    private string topDoubleJumpText;
    [SerializeField]
    private string topJumpText;
    [SerializeField]
    private string topDashText;
    [SerializeField]
    private string hookDescriptionText;
    [SerializeField]
    private string doubleJumpDescriptionText;
    [SerializeField]
    private string jumpDescriptionText;
    [SerializeField]
    private string dashDescriptionText;
    [SerializeField]
    private VideoClip hookTutorialVideo;
    [SerializeField]
    private VideoClip doubleJumpTutorialVideo;
    [SerializeField]
    private VideoClip jumpTutorialVideo;
    [SerializeField]
    private VideoClip dashTutorialVideo;
    [SerializeField]
    private Vector2 targetPosition;
    [SerializeField]
    private float totalMovetime;
    [SerializeField]
    private bool isLeft = false;

    // List of UI Object that will be manipulated
    private GameObject upgradePopUp;
    private TMP_Text topText;
    private TMP_Text descriptionText;
    private VideoPlayer upgradeVideo;
    private RectTransform panelTransform;

    void Start()
    {
        upgradePopUp = GameObject.FindGameObjectWithTag("UpgradePopUp");
        topText = upgradePopUp.transform.Find("TopText").GetComponent<TMP_Text>();
        descriptionText = upgradePopUp.transform.Find("DescriptionText").GetComponent<TMP_Text>();
        upgradeVideo = upgradePopUp.transform.Find("UpgradeVideoPlayer").GetComponent<VideoPlayer>();
        panelTransform = upgradePopUp.GetComponent<RectTransform>();
        setDisabled();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            switch (collision.gameObject.GetComponent<CollectibleData>().upgrade)
            {
                case CollectibleData.UpgradeOptions.Dash:
                    PlayerStateManager.Instance.getState().canDash = true;
                    AbilityCooldownManager.Instance.abilityUnlocked("dash");

                    if (collision.gameObject.GetComponent<CollectibleData>().showPopup && upgradePopUp != null)
                    {
                        if(upgradePopUp != null) upgradePopUp.SetActive(true);
                        topText.SetText(topDashText);
                        descriptionText.SetText(dashDescriptionText);
                        upgradeVideo.clip = dashTutorialVideo;
                        Invoke(nameof(setDisabled), secondsUntilDisable);
                        StartCoroutine(movePanel());
                    }

                    if (UpgradePopupManager.Instance != null) //this is just so nothing errors out if we havent set it up yet
                    {
                        UpgradePopupManager.Instance.showPopup("Dash", collision);
                    }
                    break;
                case CollectibleData.UpgradeOptions.Hook:
                    PlayerStateManager.Instance.getState().canHook = true;
                    if (collision.gameObject.GetComponent<CollectibleData>().showPopup && upgradePopUp != null)
                    {
                        upgradePopUp.SetActive(true);
                        topText.SetText(topHookText);
                        descriptionText.SetText(hookDescriptionText);
                        upgradeVideo.clip = hookTutorialVideo;
                        Invoke(nameof(setDisabled), secondsUntilDisable);
                        StartCoroutine(movePanel());
                    
                    }

                    if (UpgradePopupManager.Instance != null) //this is just so nothing errors out if we havent set it up yet
                    {
                        UpgradePopupManager.Instance.showPopup("Hookshot", collision);
                    }
                    break;
                case CollectibleData.UpgradeOptions.DoubleJump:
                    PlayerStateManager.Instance.getState().canDoubleJump = true;
                    PlayerDataManager.Instance.getData().jumpAmt = 2;
                    if (collision.gameObject.GetComponent<CollectibleData>().showPopup && upgradePopUp != null)
                    {
                        upgradePopUp.SetActive(true);
                        topText.SetText(topDoubleJumpText);
                        descriptionText.SetText(doubleJumpDescriptionText);
                        upgradeVideo.clip = doubleJumpTutorialVideo;
                        Invoke(nameof(setDisabled), secondsUntilDisable);
                        StartCoroutine(movePanel());
                    }

                    if (UpgradePopupManager.Instance != null) //this is just so nothing errors out if we havent set it up yet
                    {
                        UpgradePopupManager.Instance.showPopup("Double Jump", collision);
                    }
                    break;
                case CollectibleData.UpgradeOptions.Time:
                    GameTimer.Instance.addTime(collision.gameObject.GetComponent<CollectibleData>().time);
                    SoundManager.Instance.playSoundFX("batteryPickup", Vector3.zero, 0, 200, 0.15f, true);
                    break;

            }
            Destroy(collision.gameObject);
        }
    }

    IEnumerator movePanel()
    {
        float t = 0;
        Vector3 initialPanelPosition = panelTransform.localPosition;
        while (t <= totalMovetime)
        {
            t += Time.deltaTime;
            panelTransform.localPosition = Vector2.Lerp(initialPanelPosition, targetPosition, t / totalMovetime);
            if (t == totalMovetime)
            {
                resetPanelPosition();
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void resetPanelPosition()
    {
        Invoke(nameof(setDisabled), 5f);
    }
    private void setDisabled()
    {
        if (isLeft)
        {
            panelTransform.localPosition = new Vector3(-1000, 0, 0);
        } else
        {
            panelTransform.localPosition = new Vector3(1000, 0, 0);
        }
        upgradePopUp.SetActive(false);
    }
}
