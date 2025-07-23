using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove Instance;

    public Rigidbody2D rb;

    private bool isPaused = false; //used in dash, to stop movement so it doesn't influence the dash

    private int dir; //left or right, -1 left, 1 right

    float AFMult = 1; //add force mult, so i can make it close to 0 if the player is close to max speed
    private Gamepad gamePad;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }

    void Update()
    {
        if (gamePad != null)
        {
            gamePad = Gamepad.current;
        }
        else
        {
            gamePad = null;
        }



        if (!PlayerStateManager.Instance.getState().isHooked && !PlayerStateManager.Instance.getState().isDashing && !isPaused)
        {
            if (PlayerInputs.Instance.pressingLeftButton || PlayerInputs.Instance.pressingRightButton)
            {
                float xVelo = rb.linearVelocityX;

                float maxSpd = PlayerDataManager.Instance.getData().playerMaxSpd;

                if (Mathf.Abs(xVelo) > maxSpd - maxSpd / 4) //so if the player is approaching their max speed
                {
                    AFMult = 1 - (Mathf.Abs(xVelo) / maxSpd);
                }
                else
                {
                    AFMult = 1;
                }

                if (PlayerInputs.Instance.pressingLeftButton)
                {
                    if (PlayerDataManager.Instance.getData().playerDirection == "right")
                    {
                        if (PlayerStateManager.Instance.getState().isGrounded)
                        {
                            rb.linearVelocityX /= 4;
                        }
                        else if (PlayerStateManager.Instance.getState().isJumping)
                        {
                            rb.linearVelocityX /= 4.5f;
                        }
                    }

                    dir = -1;
                    PlayerDataManager.Instance.getData().playerDirection = "left";
                }
                else
                {
                    if (PlayerDataManager.Instance.getData().playerDirection == "left")
                    {
                        if (PlayerStateManager.Instance.getState().isGrounded)
                        {
                            rb.linearVelocityX /= 3;
                        }
                        else if (PlayerStateManager.Instance.getState().isJumping)
                        {
                            rb.linearVelocityX /= 3.5f;
                        }
                    }

                    dir = 1;
                    PlayerDataManager.Instance.getData().playerDirection = "right";
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
