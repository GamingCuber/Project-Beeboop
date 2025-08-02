using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 0)]

public class PlayerData : ScriptableObject
{
    [Header("General Movement")]
    public float apexDampening; //linear dampening at apex to lower speed
    public float bounceForce;
    public float coyoteTime; //coyote time in real seconds
    public float maxHorizontalSpeed;
    public float maxNegativeVerticalSpeed;
    public float maxPositiveVerticalSpeed;
     public float playerAcc; //player acceleration (probably for addForce)
    public string playerDirection;
    public float playerMaxSpd; //self explanatory

    [Header("Dash")]
    public float dashCDTime; //how much real time is dash cd
    public float dashStr = 15;
    public float dashTime; //how long the speed of the dash is kept until its reset
    [Header("Hook")]
    public float dampeningPostHook;
    public float hookCancelDistance; //how close player has to be to hook for their hook to be cancelled
    public float hookDistanceLimit;
    public float hookPointCD; //seconds in which each point is on cd after used
    public float hookSpeed;
    [Header("Jump")]
    public float jumpApexTime;
    public int jumpAmt; //max jumps consecutively (so like double jump would be 2)
    public float jumpFallGrav; //grav when falling
    public float jumpHeight;
    public float jumpTime;
    public float minJumpTime; //minimum time of jump before cancel if u like feather the button
    public float percentApex; //percentage of the jumpheight that is slowed for apex floaty time

    public GameObject current_checkpoint;
}
