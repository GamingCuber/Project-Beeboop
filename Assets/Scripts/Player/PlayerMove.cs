using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove Instance;

    public Rigidbody2D rb;

    private bool isPaused = false; //used in dash, to stop movement so it doesn't influence the dash

    private int dir; //left or right, -1 left, 1 right

    float AFMult = 1; //add force mult, so i can make it close to 0 if the player is close to max speed

    private bool dirSwitched = false; //used for when they are pressing both left n right for latest input

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
            //to STOP player
            if (!PlayerStateManager.Instance.getState().isMoving || //if theyre not choosing a direction to move
               rb.linearVelocityX > PlayerDataManager.Instance.getData().maxHorizontalSpeed / 1.5 && PlayerInputs.Instance.pressingLeftButton || //for the ice skating, if players going right but holding left
               rb.linearVelocityX < -PlayerDataManager.Instance.getData().maxHorizontalSpeed / 1.5 && PlayerInputs.Instance.pressingRightButton) //for ice skating, if players going left but holding right
            {
                rb.linearVelocityX = 0;
            }
            else if (PlayerInputs.Instance.pressingLeftButton || PlayerInputs.Instance.pressingRightButton)
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

                string latestDir = PlayerDataManager.Instance.getData().playerDirection;

                if (!dirSwitched)
                {
                    if (PlayerInputs.Instance.pressingLeftButton && PlayerInputs.Instance.pressingRightButton)
                    {
                        dirSwitched = true;

                        if (PlayerStateManager.Instance.getState().isGrounded)
                        {
                            rb.linearVelocityX /= 4;
                        }
                        else if (PlayerStateManager.Instance.getState().isJumping)
                        {
                            rb.linearVelocityX /= 4.5f;
                        }

                        dir = -dirStringtoInt(latestDir);
                        PlayerDataManager.Instance.getData().playerDirection = dirInttoString(dir);
                    }
                    else if (PlayerInputs.Instance.pressingLeftButton)
                    {
                        dir = -1;
                        PlayerDataManager.Instance.getData().playerDirection = "left";
                    }
                    else if (PlayerInputs.Instance.pressingRightButton)
                    {
                        dir = 1;
                        PlayerDataManager.Instance.getData().playerDirection = "right";
                    }
                }
                else
                {
                    if (!PlayerInputs.Instance.pressingLeftButton || !PlayerInputs.Instance.pressingRightButton)
                    {
                        dirSwitched = false;
                    }
                }


                rb.AddForceX(PlayerDataManager.Instance.getData().playerAcc * AFMult * dir, ForceMode2D.Force);
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

    private int dirStringtoInt(string s)
    {
        if (s == "left")
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }

    private string dirInttoString(int i)
    {
        if (i < 0)
        {
            return "left";
        }
        else
        {
            return "right";
        }
    }
}
