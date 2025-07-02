using UnityEngine;

public class PlayerSpeedCheck : MonoBehaviour
{
    public Rigidbody2D rb;

    void Update()
    {
        if (rb.linearVelocityX >= 0)
        {
            if (rb.linearVelocityX >= PlayerDataManager.Instance.getData().maxHorizontalSpeed)
            {
                rb.linearVelocityX = PlayerDataManager.Instance.getData().maxHorizontalSpeed;
            }
        }
        else
        {
            if (rb.linearVelocityX <= -PlayerDataManager.Instance.getData().maxHorizontalSpeed)
            {
                rb.linearVelocityX = -PlayerDataManager.Instance.getData().maxHorizontalSpeed;
            }
        }

        if (rb.linearVelocityY >= 0)
        {
            if (rb.linearVelocityY >= PlayerDataManager.Instance.getData().maxPositiveVerticalSpeed)
            {
                rb.linearVelocityY = PlayerDataManager.Instance.getData().maxPositiveVerticalSpeed;
            }
        }
        else
        {
            if (rb.linearVelocityY <= -PlayerDataManager.Instance.getData().maxNegativeVerticalSpeed)
            {
                rb.linearVelocityY = PlayerDataManager.Instance.getData().maxNegativeVerticalSpeed;
            }
        }

    }

}
