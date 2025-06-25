using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody2D rb;

    void Update()
    {
        if (!PlayerStateManager.Instance.getState().isHooked)
        {
            if (Input.GetKey(PlayerInputs.Instance.right))
            {
                PlayerDataManager.Instance.getData().playerdirection = "right";
                rb.AddForceX(PlayerDataManager.Instance.getData().playerAcc, ForceMode2D.Force);

            }

            else if (Input.GetKey(PlayerInputs.Instance.left))
            {
                PlayerDataManager.Instance.getData().playerdirection = "left";
                rb.AddForceX(-PlayerDataManager.Instance.getData().playerAcc, ForceMode2D.Force);

            }
            else
            {
                Vector2 velo = rb.linearVelocity; // G
                if (PlayerStateManager.Instance.getState().isDashing == false)
                {

                    velo.x = 0;
                }



                rb.linearVelocity = velo;
            }
        }
    }
}

