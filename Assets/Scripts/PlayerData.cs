using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 0)]

public class PlayerData : ScriptableObject
{
    public float playerAcc; //player acceleration (probably for addForce)
    public float playerMaxSpd; //self explanatory
    public float jumpStr; //probably used in the addForce upwards
    public int jumpAmt; //max jumps consecutively (so like double jump would be 2)
}
