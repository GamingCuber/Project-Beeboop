using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove Instance;

    public Rigidbody2D rb;

    private bool isPaused = false; //used in dash, to stop movement so it doesn't influence the dash

    private int dir; //left or right, -1 left, 1 right

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (!PlayerStateManager.Instance.getState().isHooked && !PlayerStateManager.Instance.getState().isDashing && !isPaused)
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
                        if (PlayerStateManager.Instance.getState().isGrounded)
                        {
                            rb.linearVelocityX /= 3;
                        }
                        else if (PlayerStateManager.Instance.getState().isJumping)
                        {
                            rb.linearVelocityX /= 2f;
                        }
                    }

                    dir = -1;
                    PlayerDataManager.Instance.getData().playerdirection = "left";
                }
                else
                {
                    if (PlayerDataManager.Instance.getData().playerdirection == "left")
                    {
                        if (PlayerStateManager.Instance.getState().isGrounded)
                        {
                            rb.linearVelocityX /= 3;
                        }
                        else if (PlayerStateManager.Instance.getState().isJumping)
                        {
                            rb.linearVelocityX /= 2f;
                        }
                    }

                    dir = 1;
                    PlayerDataManager.Instance.getData().playerdirection = "right";
                }

                rb.AddForceX(PlayerDataManager.Instance.getData().playerAcc * AFMult * dir, ForceMode2D.Force);
            }
            else if (!PlayerStateManager.Instance.getState().keepMomentum)
            {
                rb.linearVelocityX = 0;
            }
        }
    }

    public void stopMovement()
    {
        isPaused = true;
    }

    public void startMovement()
    {
        isPaused = false;
    }
}
