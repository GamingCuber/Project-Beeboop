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
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            switch (collision.gameObject.GetComponent<CollectibleData>().upgrade)
            {
                case CollectibleData.UpgradeOptions.Dash:
                    PlayerStateManager.Instance.getState().canDash = true;
                    AbilityCooldownManager.Instance.abilityUnlocked("dash");

                    if (TutorialPopupManager.Instance != null) TutorialPopupManager.Instance.showTutorial(TutorialPopupManager.tutorialOptions.dash);

                    if (UpgradePopupManager.Instance != null) //this is just so nothing errors out if we havent set it up yet
                    {
                        UpgradePopupManager.Instance.showPopup("Dash", collision);
                    }
                    break;
                case CollectibleData.UpgradeOptions.Hook:
                    PlayerStateManager.Instance.getState().canHook = true;

                    if (TutorialPopupManager.Instance != null) TutorialPopupManager.Instance.showTutorial(TutorialPopupManager.tutorialOptions.hook);

                    if (UpgradePopupManager.Instance != null) //this is just so nothing errors out if we havent set it up yet
                    {
                        UpgradePopupManager.Instance.showPopup("Hookshot", collision);
                    }
                    break;
                case CollectibleData.UpgradeOptions.DoubleJump:
                    PlayerStateManager.Instance.getState().canDoubleJump = true;
                    PlayerDataManager.Instance.getData().jumpAmt = 2;

                    if (TutorialPopupManager.Instance != null) TutorialPopupManager.Instance.showTutorial(TutorialPopupManager.tutorialOptions.doubleJump);

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
}
