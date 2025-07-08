using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance;

    public PlayerState state;

    public Rigidbody2D rb;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        resetUpgrades();
    }

    public PlayerState getState()
    {
        return state;
    }

    private void Update()
    {
        if (rb.linearVelocityY < 0 && !state.isFalling && !state.isGrounded && !state.isDashing && !state.isHooked && !state.isJumping)
        {
            state.isFalling = true;
        }
        else if (state.isFalling && state.isGrounded)
        {
            state.isFalling = false;
        }

        if ((Input.GetKey(PlayerInputs.Instance.left) || Input.GetKey(PlayerInputs.Instance.right)) && !state.isMoving) //check if player is moving by seeing if they input anything lolo
        {
            state.isMoving = true;
        }
        else if (!Input.GetKey(PlayerInputs.Instance.left) && !Input.GetKey(PlayerInputs.Instance.right) && state.isMoving)
        {
            state.isMoving = false;
        }
    }

    private void resetUpgrades()
    {
        state.canDash = false;
        state.canDoubleJump = false;
        state.canHook = false;
        StartCoroutine(waitForData());
    }

    private IEnumerator waitForData()
    {
        while (PlayerDataManager.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }

        PlayerDataManager.Instance.getData().jumpAmt = 1;
    }
}
