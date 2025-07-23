using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public Rigidbody2D rb;

    public bool debounce = false;

    private bool onCooldown;

    void Update()
    {
        if (PlayerInputs.Instance.pressingDashButton && debounce == false && PlayerStateManager.Instance.getState().canDash && !onCooldown)
        {
            PlayerJump.Instance.cancelJump(false);
            StartCoroutine(DashCoroutine());
        }

        if (debounce && PlayerStateManager.Instance.getState().isGrounded)
        {
            debounce = false;
            PlayerGravManager.Instance.resetGrav();
        }
    }


    public IEnumerator DashCoroutine()
    {
        float timer = 0;

        debounce = true;
        onCooldown = true;
        Vector2 dashDir = Vector2.zero;

        PlayerStateManager.Instance.getState().isDashing = true;
        PlayerStateManager.Instance.getState().isFalling = false;

        if (PlayerInputs.Instance.pressingUpButton || Input.GetAxis("Vertical") > 0)
        {
            dashDir.y = -1;
        }
        else if (PlayerInputs.Instance.pressingDownButton || Input.GetAxis("Vertical") < 0)
        {
            dashDir.y = 1;
        }

        if (PlayerInputs.Instance.pressingLeftButton || Input.GetAxis("Horizontal") < 0)
        {
            dashDir.x = -1;
        }
        else if (PlayerInputs.Instance.pressingRightButton || Input.GetAxis("Horizontal") > 0)
        {
            dashDir.x = 1;
        }

        dashDir.Normalize();

        if (dashDir == Vector2.zero)
        {
            PlayerMove.Instance.startMovement();
            PlayerStateManager.Instance.getState().isDashing = false;
            PlayerStateManager.Instance.getState().isFalling = true;
            StartCoroutine(cooldownTimer());
            yield break;
        }

        PlayerGravManager.Instance.setGrav(0);

        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        rb.AddForce(dashDir * PlayerDataManager.Instance.getData().dashStr, ForceMode2D.Impulse);

        DashAfterimage.Instance.doAfterimage();

        while (timer < PlayerDataManager.Instance.getData().dashTime)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        SoundManager.Instance.playPlayerSound("dash");
        PlayerMove.Instance.startMovement();
        PlayerStateManager.Instance.getState().isDashing = false;
        PlayerStateManager.Instance.getState().isFalling = true;

        StartCoroutine(cooldownTimer());

        yield break;
    }

    private IEnumerator cooldownTimer()
    {
        int frames = 0;

        while (frames < PlayerDataManager.Instance.getData().dashCDFrames)
        {
            frames++;
            yield return new WaitForEndOfFrame();
        }

        onCooldown = false;
        yield break;
    }
}
