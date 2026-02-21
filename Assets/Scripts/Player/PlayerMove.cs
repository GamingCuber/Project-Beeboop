using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove Instance;

    public Rigidbody2D rb;

    private bool isPaused = false; //used in dash, to stop movement so it doesn't influence the dash

    private int dir; //left or right, -1 left, 1 right

    float AFMult = 1; //add force mult, so i can make it close to 0 if the player is close to max speed

    private bool dirSwitched = false; //used for when they are pressing both left n right for latest input

    private Coroutine turnCheckCo;

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

        if (rb.linearVelocityY < 0 && !PlayerStateManager.Instance.getState().isFalling && !PlayerStateManager.Instance.getState().isGrounded && !PlayerStateManager.Instance.getState().isDashing && !PlayerStateManager.Instance.getState().isHooked && !PlayerStateManager.Instance.getState().isJumping)
        {
           PlayerStateManager.Instance.getState().isFalling = true;
        }
        else if (PlayerStateManager.Instance.getState().isFalling &&PlayerStateManager.Instance.getState().isGrounded)
        {
            PlayerStateManager.Instance.getState().isFalling = false;
        }

        if (PlayerInputs.Instance.pressingLeftButton || PlayerInputs.Instance.pressingRightButton) //check if player is moving by seeing if they input anything lolo
        {
           PlayerStateManager.Instance.getState().isMoving = true;
        }
        else if ((!PlayerInputs.Instance.pressingLeftButton && !PlayerInputs.Instance.pressingRightButton) && PlayerStateManager.Instance.getState().isMoving)
        {
           PlayerStateManager.Instance.getState().isMoving = false;
        }

        if (!PlayerStateManager.Instance.getState().isHookPulling && !PlayerStateManager.Instance.getState().isDashing && !isPaused && !PlayerStateManager.Instance.getState().isDead)
        {
            //to STOP player
            if (!PlayerStateManager.Instance.getState().isHooked && //so we don't stop mid hook
                !PlayerStateManager.Instance.getState().isMoving || //if theyre not choosing a direction to move
               rb.linearVelocityX > PlayerDataManager.Instance.getData().maxHorizontalSpeed / 1.5 && PlayerInputs.Instance.pressingLeftButton || //for the ice skating, if players going right but holding left
               rb.linearVelocityX < -PlayerDataManager.Instance.getData().maxHorizontalSpeed / 1.5 && PlayerInputs.Instance.pressingRightButton) //for ice skating, if players going left but holding right
            {
                rb.linearVelocityX = 0;
                tryEndTurnCheck();
            }
            else if (PlayerInputs.Instance.pressingLeftButton || PlayerInputs.Instance.pressingRightButton)
            {
                float xVelo = rb.linearVelocityX;

                //changes max speed so player can move faster midair when hooking
                float maxSpd = !PlayerStateManager.Instance.getState().isHooked ?
                    PlayerDataManager.Instance.getData().playerMaxSpd : PlayerDataManager.Instance.getData().playerMaxHookSpd;

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
                        tryEndTurnCheck();

                        dirSwitched = true;

                        if (PlayerStateManager.Instance.getState().isGrounded)
                        {
                            VFXManager.Instance.playVFX("Turn");
                        }

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
                        startTurnCheck();
                    }
                    else if (PlayerInputs.Instance.pressingRightButton)
                    {
                        dir = 1;
                        PlayerDataManager.Instance.getData().playerDirection = "right";
                        startTurnCheck();
                    }
                }
                else
                {
                    if (!PlayerInputs.Instance.pressingLeftButton || !PlayerInputs.Instance.pressingRightButton)
                    {
                        dirSwitched = false;
                    }
                }


                rb.AddForceX(PlayerDataManager.Instance.getData().playerAcceleration * AFMult * dir, ForceMode2D.Force);
            }
        }
        else if (PlayerStateManager.Instance.getState().isHooked && !PlayerStateManager.Instance.getState().isHookPulling)
        {
            if (PlayerInputs.Instance.pressingLeftButton || PlayerInputs.Instance.pressingRightButton)
            {
                dir = PlayerInputs.Instance.pressingLeftButton ? -1 : 1;

                PlayerDataManager.Instance.getData().playerDirection = dir == -1 ? "left" : "right";

                float xVelo = rb.linearVelocityX;

                if (xVelo * dir < 0)
                {
                    rb.linearVelocityX /= 1.05f;
                }

                rb.AddForceX(dir * 10, ForceMode2D.Force);
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

    private void startTurnCheck()
    {
        if (turnCheckCo == null)
        {
            turnCheckCo = StartCoroutine(checkTurn());
        }
    }

    private void tryEndTurnCheck()
    {
        if (turnCheckCo != null)
        {
            StopCoroutine(turnCheckCo);
        }
        turnCheckCo = null;
    }

    private IEnumerator checkTurn()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        string startDir = PlayerDataManager.Instance.getData().playerDirection;

        while (true)
        {
            if (PlayerDataManager.Instance.getData().playerDirection != startDir)
            {
                if (PlayerStateManager.Instance.getState().isGrounded)
                {
                    VFXManager.Instance.playVFX("Turn");
                }

                if (PlayerStateManager.Instance.getState().isGrounded)
                {
                    rb.linearVelocityX /= 4;
                }
                else if (PlayerStateManager.Instance.getState().isJumping)
                {
                    rb.linearVelocityX /= 4.5f;
                }
                break;
            }

            yield return wait;
        }

        yield break;
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
