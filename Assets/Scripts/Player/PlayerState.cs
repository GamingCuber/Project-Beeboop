using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState", menuName = "PlayerState", order = 1)]
public class PlayerState : ScriptableObject
{
    public bool isGrounded = false;
    public bool isDashing = false;
    public bool isHooked = false;

}
