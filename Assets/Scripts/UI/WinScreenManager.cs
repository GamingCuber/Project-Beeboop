using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    public void backButton()
    {
        SceneManager.LoadScene("StartMenu");
        PlayerStateManager.Instance.getState().deathNumber = 0;
        PlayerStateManager.Instance.getState().totalTime = 0;
        PlayerStateManager.Instance.getState().canDash = false;
        PlayerStateManager.Instance.getState().canHook = false;
        PlayerStateManager.Instance.getState().canDoubleJump = false;
    }
}
