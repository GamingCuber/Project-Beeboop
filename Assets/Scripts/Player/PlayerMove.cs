using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody2D rb;

    private int dir; //left or right, -1 left, 1 right

    void Update()
    {
        if (!PlayerStateManager.Instance.getState().isHooked && !PlayerStateManager.Instance.getState().isDashing)
        {
            if (Input.GetKey(PlayerInputs.Instance.left) || Input.GetKey(PlayerInputs.Instance.right))
            {
                float xVelo = rb.linearVelocityX;

                float maxSpd = PlayerDataManager.Instance.getData().playerMaxSpd;

                float AFMult = 1; //add force mult, so i can make it close to 0 if the player is close to max speed

                if (Mathf.Abs(xVelo) > maxSpd - maxSpd / 4) //so if the player is approaching their max speed
                {
                    AFMult = 1 - (Mathf.Abs(xVelo) / maxSpd);
                }

                if (Input.GetKey(PlayerInputs.Instance.left))
                {
                    if (PlayerDataManager.Instance.getData().playerdirection == "right")
                    {
                        rb.linearVelocityX *= -1;
                    }

                    dir = -1;
                    PlayerDataManager.Instance.getData().playerdirection = "left";
                }
                else
                {
                    if (PlayerDataManager.Instance.getData().playerdirection == "left")
                    {
                        rb.linearVelocityX *= -1;
                    }

                    dir = 1;
                    PlayerDataManager.Instance.getData().playerdirection = "right";
                }

                rb.AddForceX(PlayerDataManager.Instance.getData().playerAcc * AFMult * dir, ForceMode2D.Force);
            }
            else if (PlayerStateManager.Instance.getState().isGrounded)
            {
                rb.linearVelocityX = 0;
            }
        }
    }
}
