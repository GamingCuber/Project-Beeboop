using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    private Sprite hookImage;
    [SerializeField]
    private Sprite doubleJumpImage;
    [SerializeField]
    private Sprite jumpImage;
    [SerializeField]
    private Sprite dashImage;
    // List of UI Object that will be manipulated

    private GameObject upgradePopUp;
    private TMP_Text topText;
    private TMP_Text descriptionText;
    private Sprite upgradeSprite;

    void Start()
    {
        upgradePopUp = GameObject.FindGameObjectWithTag("UpgradePopUp");
        Debug.Log(upgradePopUp);
        topText = upgradePopUp.transform.Find("TopText").GetComponent<TMP_Text>();
        descriptionText = upgradePopUp.transform.Find("DescriptionText").GetComponent<TMP_Text>();
        upgradeSprite = upgradePopUp.transform.Find("UpgradeImage").GetComponent<Image>().sprite;
        upgradePopUp.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            switch (collision.gameObject.GetComponent<CollectibleData>().upgrade)
            {
                case CollectibleData.UpgradeOptions.Dash:
                    PlayerStateManager.Instance.getState().canDash = true;

                    if (collision.gameObject.GetComponent<CollectibleData>().showPopup)
                    {
                        upgradePopUp.SetActive(true);
                        topText.SetText(topDashText);
                        descriptionText.SetText(dashDescriptionText);
                        upgradeSprite = dashImage;
                        Invoke(nameof(setDisabled), secondsUntilDisable);
                    }

                    if (UpgradePopupManager.Instance != null) //this is just so nothing errors out if we havent set it up yet
                    {
                        UpgradePopupManager.Instance.showPopup("Dash", collision);
                    }
                    break;
                case CollectibleData.UpgradeOptions.Hook:
                    PlayerStateManager.Instance.getState().canHook = true;
                    if (collision.gameObject.GetComponent<CollectibleData>().showPopup)
                    {
                        upgradePopUp.SetActive(true);
                        topText.SetText(topHookText);
                        descriptionText.SetText(hookDescriptionText);
                        upgradeSprite = hookImage;
                        Invoke(nameof(setDisabled), secondsUntilDisable);
                    }

                    if (UpgradePopupManager.Instance != null) //this is just so nothing errors out if we havent set it up yet
                    {
                        UpgradePopupManager.Instance.showPopup("Hookshot", collision);
                    }
                    break;
                case CollectibleData.UpgradeOptions.DoubleJump:
                    PlayerStateManager.Instance.getState().canDoubleJump = true;
                    PlayerDataManager.Instance.getData().jumpAmt = 2;
                    if (collision.gameObject.GetComponent<CollectibleData>().showPopup)
                    {
                        upgradePopUp.SetActive(true);
                        topText.SetText(topDoubleJumpText);
                        descriptionText.SetText(doubleJumpDescriptionText);
                        upgradeSprite = doubleJumpImage;
                        Invoke(nameof(setDisabled), secondsUntilDisable);
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

    private void setDisabled()
    {
        upgradePopUp.SetActive(false);
    }
}
