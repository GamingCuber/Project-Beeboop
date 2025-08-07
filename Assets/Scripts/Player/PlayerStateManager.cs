using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance;

    public PlayerState state;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }

    public PlayerState getState()
    {
        return state;
    }


}
