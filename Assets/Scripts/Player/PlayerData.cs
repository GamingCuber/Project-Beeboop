using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 0)]

public class PlayerData : ScriptableObject
{
    public float playerAcc; //player acceleration (probably for addForce)
    public float playerMaxSpd; //self explanatory
    public int coyoteFrames; //frames of coyote time
    public int jumpAmt; //max jumps consecutively (so like double jump would be 2)
    public float jumpHeight;
    public float jumpTime;
    public float jumpApexTime;
    public float percentApex; //percentage of the jumpheight that is slowed for apex floaty time
    public float apexDampening; //linear dampening at apex to lower speed
    public float jumpFallGrav; //grav when falling
    public float dashStr = 15;
    public float dashTime; //how long the speed of the dash is kept until its reset
    public float dashCDFrames; //how many frames dash is on cd
    public string playerdirection;
    public float hookDistanceLimit;
    public float hookSpeed;
    public float dampeningPostHook;
    public float maxHorizontalSpeed;
    public float maxNegativeVerticalSpeed;
    public float maxPositiveVerticalSpeed;

    public GameObject current_checkpoint;
}
