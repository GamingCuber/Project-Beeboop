using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 0)]

public class PlayerData : ScriptableObject
{
    public float playerAcc; //player acceleration (probably for addForce)
    public float playerMaxSpd; //self explanatory
    public float coyoteTime; //coyote time in real seconds
    public int jumpAmt; //max jumps consecutively (so like double jump would be 2)
    public float jumpHeight;
    public float jumpTime;
    public float minJumpTime; //minimum time of jump before cancel if u like feather the button
    public float jumpApexTime;
    public float percentApex; //percentage of the jumpheight that is slowed for apex floaty time
    public float apexDampening; //linear dampening at apex to lower speed
    public float jumpFallGrav; //grav when falling
    public float dashStr = 15;
    public float dashTime; //how long the speed of the dash is kept until its reset
    public float dashCDTime; //how much real time is dash cd
    public string playerDirection;
    public float hookDistanceLimit;
    public float hookCancelDistance; //how close player has to be to hook for their hook to be cancelled
    public float hookSpeed;
    public float hookPointCD; //seconds in which each point is on cd after used
    public float dampeningPostHook;
    public float maxHorizontalSpeed;
    public float maxNegativeVerticalSpeed;
    public float maxPositiveVerticalSpeed;
    public float bounceForce;

    public GameObject current_checkpoint;
}
