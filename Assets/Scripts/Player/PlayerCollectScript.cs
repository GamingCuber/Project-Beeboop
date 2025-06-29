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
                    break;
                case CollectibleData.UpgradeOptions.Hook:
                    PlayerStateManager.Instance.getState().canHook = true;
                    break;
                case CollectibleData.UpgradeOptions.DoubleJump:
                    PlayerStateManager.Instance.getState().canDoubleJump = true;
                    PlayerDataManager.Instance.getData().jumpAmt = 2;
                    break;

            }
            Destroy(collision.gameObject);
        }



    }
}
