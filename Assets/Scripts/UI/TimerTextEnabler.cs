using UnityEngine;

public class TimerTextEnabler : MonoBehaviour
{
    public void reverseWantTimer()
    {
        if (PlayerStateManager.Instance.getState().wantsTimer)
        {
            PlayerStateManager.Instance.getState().wantsTimer = false;
        }
        else
        {
            PlayerStateManager.Instance.getState().wantsTimer = true;

        }
    }
}
