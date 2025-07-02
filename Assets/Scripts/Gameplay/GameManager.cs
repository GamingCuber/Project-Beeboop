using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resetState();
    }

    // Yes I know this code is disgusting. No, I'm not going to change it
    private void resetState()
    {
        PlayerStateManager.Instance.getState().keepMomentum = false;
        PlayerStateManager.Instance.getState().isGrounded = false;
        PlayerStateManager.Instance.getState().isDashing = false;
        PlayerStateManager.Instance.getState().isHooked = false;
        PlayerStateManager.Instance.getState().isJumping = false;
        PlayerStateManager.Instance.getState().isFalling = false;
        PlayerStateManager.Instance.getState().canDash = false;
        PlayerStateManager.Instance.getState().canDoubleJump = false;
        PlayerStateManager.Instance.getState().canHook = false;


    }
}
