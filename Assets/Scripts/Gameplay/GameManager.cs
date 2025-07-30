using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (PlayerStateManager.Instance.getState().gameLost)
        {
            StartCoroutine(waitForState());
        }
    }

    // Yes I know this code is disgusting. No, I'm not going to change it
    public void resetState()
    {
        PlayerStateManager.Instance.getState().keepMomentum = false;
        PlayerStateManager.Instance.getState().isDead = false;
        PlayerStateManager.Instance.getState().isGrounded = false;
        PlayerStateManager.Instance.getState().isDashing = false;
        PlayerStateManager.Instance.getState().isHooked = false;
        PlayerStateManager.Instance.getState().isJumping = false;
        PlayerStateManager.Instance.getState().isFalling = false;
        PlayerStateManager.Instance.getState().canDash = false;
        PlayerStateManager.Instance.getState().canDoubleJump = false;
        PlayerStateManager.Instance.getState().canHook = false;
        PlayerDataManager.Instance.getData().jumpAmt = 1;

        //im assuming we just reset when u first load in after u lose
        PlayerStateManager.Instance.getState().gameLost = false;
    }

    private IEnumerator waitForState()
    {
        while (PlayerStateManager.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }

        resetState();
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
    }

    public void resumeGame()
    {
        Time.timeScale = 1; 
    }
}
