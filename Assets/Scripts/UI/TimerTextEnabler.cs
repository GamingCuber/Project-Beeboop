using System;
using UnityEngine;

public class TimerTextEnabler : MonoBehaviour
{
    Boolean initializing = true;
    public void reverseWantTimer()
    {
        Debug.Log("reverse");
        if (!PlayerStateManager.Instance.getState().wantsTimer && initializing)
        {
            initializing = false;
            return;
        }
        else if (initializing)
        {
            initializing = false;
        }
        
        if (TimeTextManager.Instance != null) 
        {
            TimeTextManager.Instance.swapEnable();
        }
        else
        {
            PlayerStateManager.Instance.getState().wantsTimer = !PlayerStateManager.Instance.getState().wantsTimer;
        }
    }
}
