using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public int CoolDown = 1;
    public Rigidbody2D rb;

    public bool debounce = false;

    void Update()
    {
        if (Input.GetKeyDown(PlayerInputs.Instance.dash) && debounce == false && PlayerStateManager.Instance.getState().canDash)
        {
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
        PlayerStateManager.Instance.getState().isDashing = true;
        PlayerGravManager.Instance.setGrav(0);

        Vector2 dashDir = Vector2.zero;

        if (Input.GetKey(PlayerInputs.Instance.up))
        {
            dashDir.y = -1;
        }
        else if (Input.GetKey(PlayerInputs.Instance.down))
        {
            dashDir.y = 1;
        }

        if (Input.GetKey(PlayerInputs.Instance.left))
        {
            dashDir.x = -1;
        }
        else if (Input.GetKey(PlayerInputs.Instance.right))
        {
            dashDir.x = 1;
        }

        dashDir.Normalize();

        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        rb.AddForce(dashDir * PlayerDataManager.Instance.getData().dashStr, ForceMode2D.Impulse);

        while (timer < PlayerDataManager.Instance.getData().dashTime)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        SoundManager.Instance.playsound("dash");
        PlayerMove.Instance.startMovement();
        PlayerGravManager.Instance.setGrav(PlayerDataManager.Instance.getData().jumpFallGrav);
        PlayerStateManager.Instance.getState().isDashing = false;

        yield break;
    }
}
