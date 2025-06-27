using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 0)]

public class PlayerData : ScriptableObject
{
    public float playerAcc; //player acceleration (probably for addForce)
    public float playerMaxSpd; //self explanatory
    public int coyoteFrames; //frames of coyote time
    public float jumpStr; //probably used in the addForce upwards
    public int jumpAmt; //max jumps consecutively (so like double jump would be 2)
    public float jumpFloatGrav; //grav at the apex of the jump
    public float jumpFallGrav; //grav when falling
    public float dashStr = 15;
    public float dashTime; //how long the speed of the dash is kept until its reset
    public string playerdirection;

    public int upgradescollected = 0;
    public float hookDistanceLimit;
    public float hookSpeed;
    public float dampeningPostHook;
}
