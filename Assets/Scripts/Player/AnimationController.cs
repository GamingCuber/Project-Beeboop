using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour
{
    public Animator anim;

    public SpriteRenderer sr;

    void Update()
    {

        if ((PlayerInputs.Instance.pressingLeftButton && !sr.flipX) || (Input.GetAxis("Horizontal") < 0 && !sr.flipX)) //if they goin left, flip sprite, otherwise don't
        {
            sr.flipX = true;
        }
        else if ((PlayerInputs.Instance.pressingLeftButton && sr.flipX) || (Input.GetAxis("Horizontal") > 0 && sr.flipX))
        {
            sr.flipX = false;
        }

        if (PlayerStateManager.Instance.getState().isGrounded && !anim.GetBool("isGrounded")) //self explanatory
        {
            anim.SetBool("isGrounded", true);
        }
        else if (anim.GetBool("isGrounded"))
        {
            anim.SetBool("isGrounded", false);
        }

        if (PlayerStateManager.Instance.getState().isGrounded && PlayerStateManager.Instance.getState().isMoving) //if theyre on the floor and moving
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        if (PlayerStateManager.Instance.getState().isJumping && !PlayerStateManager.Instance.getState().isFalling) //if they jumping and not falling
        {
            anim.SetBool("isJumping", true);
        }
        else if (PlayerStateManager.Instance.getState().isJumping && PlayerStateManager.Instance.getState().isFalling || anim.GetBool("isDashing")) //jumping but falling
        {
            anim.SetBool("isJumping", false);
        }

        if (PlayerStateManager.Instance.getState().isDashing && !anim.GetBool("isDashing")) //self explanatory
        {
            anim.SetBool("isDashing", true);
        }
        else if (!PlayerStateManager.Instance.getState().isDashing && anim.GetBool("isDashing"))
        {
            anim.SetBool("isDashing", false);
        }

        if (PlayerStateManager.Instance.getState().isFalling) //self explanatory
        {
            anim.SetBool("isFalling", true);
        }
        else
        {
            anim.SetBool("isFalling", false);
        }
    }
}
