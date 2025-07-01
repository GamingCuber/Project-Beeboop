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
    }
}
