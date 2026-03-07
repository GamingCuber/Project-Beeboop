using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 0)]

public class PlayerData : ScriptableObject
{
    [Header("General Movement")]
    [Tooltip("Linear dampening at apex to lower speed")]
    public float apexDampening;
    public float bounceForce;
    [Tooltip("Measured in real-time seconds")]
    public float coyoteTime;
    public float maxHorizontalSpeed;
    public float maxNegativeVerticalSpeed;
    public float maxPositiveVerticalSpeed;
    public float playerAcceleration; //player acceleration (probably for addForce)
    public string playerDirection;
    public float playerMaxSpd; //self explanatory

    [Tooltip("Max lateral movement when flying w/ hook")]
    public float playerMaxHookSpd;

    [Header("Dash")]
    public float dashCooldownTime; //how much real time is dash cd
    public float dashStrength = 15;
    public float dashTime; //how long the speed of the dash is kept until its reset
    [Header("Hook")]
    public float dampeningPostHook;
    public float hookCancelDistance; //how close player has to be to hook for their hook to be cancelled
    public float hookDistanceLimit;
    public float hookPointCooldown; //seconds in which each point is on cd after used
    public float hookSpeed;

    public float initHookStrength = 15;
    [Header("Jump")]
    public int jumpAmt; //max jumps consecutively (so like double jump would be 2)
    public float jumpFallGrav; //grav when falling
    public float jumpHeight;
    public float jumpTime;
    public float minJumpTime; //minimum time of jump before cancel if u like feather the button
    [Tooltip("Time before fall gravity applies to jump")]
    public float jumpFloatTime; 

    public GameObject currentCheckpoint;
}
