using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollectScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            Debug.Log("Is Collided");
            switch (collision.gameObject.GetComponent<CollectibleData>().upgrade)
            {
                case CollectibleData.UpgradeOptions.Dash:
                    PlayerStateManager.Instance.getState().canDash = true;

                    if (UpgradePopupManager.Instance != null) //this is just so nothing errors out if we havent set it up yet
                    {
                        UpgradePopupManager.Instance.showPopup("Dash", collision);
                    }
                    break;
                case CollectibleData.UpgradeOptions.Hook:
                    PlayerStateManager.Instance.getState().canHook = true;
                    if (UpgradePopupManager.Instance != null) //this is just so nothing errors out if we havent set it up yet
                    {
                        UpgradePopupManager.Instance.showPopup("Hookshot", collision);
                    }
                    break;
                case CollectibleData.UpgradeOptions.DoubleJump:
                    PlayerStateManager.Instance.getState().canDoubleJump = true;
                    PlayerDataManager.Instance.getData().jumpAmt = 2;
                    if (UpgradePopupManager.Instance != null) //this is just so nothing errors out if we havent set it up yet
                    {
                        UpgradePopupManager.Instance.showPopup("Double Jump", collision);
                    }
                    break;

            }
            Destroy(collision.gameObject);
        }



    }
}
