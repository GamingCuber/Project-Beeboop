using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator anim;

    public SpriteRenderer sr;

    void Update()
    {
        if (Input.GetKey(PlayerInputs.Instance.left) && !sr.flipX)
        {
            sr.flipX = true;
        }
        else if (Input.GetKey(PlayerInputs.Instance.right) && sr.flipX)
        {
            sr.flipX = false;
        }

        if (PlayerStateManager.Instance.getState().isGrounded && !anim.GetBool("isGrounded"))
        {
            anim.SetBool("isGrounded", true);
        }
        else if (anim.GetBool("isGrounded"))
        {
            anim.SetBool("isGrounded", false);
        }

        if (PlayerStateManager.Instance.getState().isGrounded && PlayerStateManager.Instance.getState().isMoving)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        if (PlayerStateManager.Instance.getState().isJumping && !PlayerStateManager.Instance.getState().isFalling)
        {
            anim.SetBool("isJumping", true);
        }
        else if (PlayerStateManager.Instance.getState().isJumping && PlayerStateManager.Instance.getState().isFalling)
        {
            anim.SetBool("isJumping", false);
        }

        if (PlayerStateManager.Instance.getState().isDashing && !anim.GetBool("isDashing"))
        {
            anim.SetBool("isDashing", true);
        }
        else if (!PlayerStateManager.Instance.getState().isDashing && anim.GetBool("isDashing"))
        {
            anim.SetBool("isDashing", false);
        }

        if (PlayerStateManager.Instance.getState().isFalling)
        {
            anim.SetBool("isFalling", true);
        }
        else
        {
            anim.SetBool("isFalling", false);
        }
    }
}
