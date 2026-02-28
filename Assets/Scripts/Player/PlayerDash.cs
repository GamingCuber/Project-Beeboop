using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public static PlayerDash Instance;

    public Rigidbody2D rb;

    public bool debounce = false;

    private bool onCooldown;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        //need this to be dashbuttondown
        if (PlayerInputs.Instance.playerController.Player.Dash.WasPressedThisFrame() && debounce == false && PlayerStateManager.Instance.getState().canDash && !onCooldown && !PlayerStateManager.Instance.getState().isDead)
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

    public void resetDash()
    {
        debounce = false;
    }

    public IEnumerator DashCoroutine()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;

        debounce = true;
        onCooldown = true;
        Vector2 dashDir = Vector2.zero;

        PlayerStateManager.Instance.getState().isDashing = true;
        PlayerStateManager.Instance.getState().isFalling = false;

        if (PlayerInputs.Instance.pressingUpButton)
        {
            dashDir.y = 1;
        }
        else if (PlayerInputs.Instance.pressingDownButton)
        {
            dashDir.y = -1;
        }

        if (PlayerInputs.Instance.pressingLeftButton)
        {
            dashDir.x = -1;
        }
        else if (PlayerInputs.Instance.pressingRightButton)
        {
            dashDir.x = 1;
        }

        dashDir.Normalize();
        dashDir.y *= 5f;

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

        rb.AddForce(dashDir * PlayerDataManager.Instance.getData().dashStrength, ForceMode2D.Impulse);

        DashAfterimage.Instance.doAfterimage();

        while (timer < PlayerDataManager.Instance.getData().dashTime)
        {
            timer += Time.deltaTime;
            yield return wait;
        }

        PlayerStateManager.Instance.getState().isDashing = false;
        PlayerStateManager.Instance.getState().isFalling = true;

        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        SoundManager.Instance.playPlayerSound("dash");
        PlayerMove.Instance.startMovement();

        StartCoroutine(cooldownTimer());

        yield break;
    }

    private IEnumerator cooldownTimer()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;

        float totalTime = PlayerDataManager.Instance.getData().dashCooldownTime;

        while (timer <= totalTime)
        {
            timer += Time.deltaTime;
            yield return wait;
        }

        onCooldown = false;
        yield break;
    }
}
