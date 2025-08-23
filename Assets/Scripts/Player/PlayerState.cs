using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState", menuName = "PlayerState", order = 1)]
public class PlayerState : ScriptableObject
{
    [Header("General States revolving around the Player")]
    public bool isGrounded = false;
    public bool isMoving = false;
    public bool isDashing = false;
    public bool isHooked = false;
    public bool isJumping = false;
    public bool isFalling = false;
    public bool isDead = false;
    public bool keepMomentum = false;
    public bool canDash = false;
    public bool canDoubleJump = false;
    public bool canHook = false;
    public bool gameLost = false;
    public bool pausedGame = false;
    public bool wantsTimer = false;
    [Header("Ranking Data")]
    public int deathNumber = 0;
    public float totalTime = 0f;
    public float firstLevelTime = 0f;
    public float secondLevelTime = 0f;

}
