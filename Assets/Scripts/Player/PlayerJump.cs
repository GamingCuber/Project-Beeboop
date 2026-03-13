using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class PlayerJump : MonoBehaviour
{
    public static PlayerJump Instance;

    public Rigidbody2D rb;
    public CircleCollider2D bottomCollider;

    private bool canJump = true;

    private bool isJumping = false;

    private int jumpsLeft;

    private Coroutine jumpCo;

    private Coroutine preJumpCo;

    private Coroutine coyoteCo;

    private LayerMask platformLayer;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        platformLayer = LayerMask.GetMask("Platform");
        StartCoroutine(waitForData());
    }
    void Update()
    {
        if (Physics2D.Raycast(this.transform.position + Vector3.left * 0.3f, Vector2.down, 1.1f, platformLayer) || Physics2D.Raycast(this.transform.position + Vector3.right * 0.3f, Vector2.down, 1.1f, platformLayer)) //if player is close to ground theyre PlayerStateManager.Instance.getState().isGrounded!
        {
            canJump = true;
            PlayerStateManager.Instance.getState().isGrounded = true;
            PlayerStateManager.Instance.getState().isJumping = false;
            rb.linearDamping = 0;

            if (jumpsLeft < PlayerDataManager.Instance.getData().jumpAmt && !isJumping)
            {
                resetJumps();
            }

            if (PlayerStateManager.Instance.getState().keepMomentum && !PlayerStateManager.Instance.getState().isHooked && PlayerStateManager.Instance.getState().isGrounded)
            {
                PlayerStateManager.Instance.getState().keepMomentum = false;
            }
        }
        else
        {
            PlayerStateManager.Instance.getState().isGrounded = false;
        }

        if (PlayerInputs.Instance.playerController.Player.Jump.WasPressedThisFrame() && !PlayerStateManager.Instance.getState().isDead && jumpsLeft > 0 && (canJump || isJumping))
        {
            if (PlayerStateManager.Instance.getState().canDoubleJump && jumpsLeft == 1)
            {
                VFXManager.Instance.playVFX("DoubleJump");
            }

            cancelJump(true);
            jump();
        }
        else if (PlayerInputs.Instance.playerController.Player.Jump.WasPressedThisFrame() && !canJump && jumpsLeft == 2)
        {
            jumpsLeft--;
            jump();
        }

        if (!PlayerStateManager.Instance.getState().isGrounded && canJump && coyoteCo == null && jumpsLeft == PlayerDataManager.Instance.getData().jumpAmt) //gives coyote time if they were recently on the ground, they isn't already a co running and if theyre on their first jump
        {
            coyoteCo = StartCoroutine(coyoteTimer());
        }
    }

    private void jump() //do a jump, cuz this functions gonna get called in more than one places
    {
        isJumping = true;
        PlayerStateManager.Instance.getState().isJumping = true;
        SoundManager.Instance.playPlayerSound("jump");
        jumpsLeft--;
        cancelJump(true);
        jumpCo = StartCoroutine(doJump());
    }

    private IEnumerator doJump() //itll addforce and watch for conditions to mess with the gravity
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float time = 0;

        Vector3 initPos = this.gameObject.transform.position;

        PlayerData data = PlayerDataManager.Instance.getData();

        PlayerGravManager.Instance.setGrav(0);

        while (time < PlayerDataManager.Instance.getData().jumpTime)
        {
            time += Time.deltaTime;

            RaycastHit2D coll = Physics2D.Raycast(this.transform.position, Vector2.up, 1.1f, platformLayer);

            if (coll.collider != null && coll.collider.gameObject.TryGetComponent<TilemapCollider2D>(out TilemapCollider2D TC))
            {
                break;
            }

            if (!PlayerInputs.Instance.playerController.Player.Jump.IsPressed() && time >= PlayerDataManager.Instance.getData().minJumpTime) //if they let go or player starts falling, increase grav so they fall faster
            {
                break;
            }

            Vector3 pos = this.gameObject.transform.position;

            if (time < data.jumpTime)
            {
                pos.y = Mathf.Lerp(initPos.y, initPos.y + data.jumpHeight, (Mathf.Sin(Mathf.PI / 2f * (time / data.jumpTime))));
            }

            this.gameObject.transform.position = pos;

            yield return wait;
        }

        rb.linearDamping = 0;

        yield return new WaitForSecondsRealtime(PlayerDataManager.Instance.getData().jumpFloatTime);
        PlayerStateManager.Instance.getState().isFalling = true;

        if (preJumpCo != null)
        {
            StopCoroutine(preJumpCo);
        }
        preJumpCo = StartCoroutine(checkForPreInput());

        while (!PlayerStateManager.Instance.getState().isGrounded) //just stall till the player hits the ground to reset grav
        {
            yield return wait;
        }

        isJumping = false;
        PlayerStateManager.Instance.getState().isJumping = false;
        SoundManager.Instance.playPlayerSound("fall");
        yield break;
    }

    private IEnumerator coyoteTimer() //counts frames for when the player can jump off whilst not PlayerStateManager.Instance.getState().isGrounded
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;

        float totalTime = PlayerDataManager.Instance.getData().coyoteTime;

        while (timer <= totalTime)
        {
            timer += Time.deltaTime;
            yield return wait;
        }

        canJump = false;
        coyoteCo = null;
        yield break;
    }

    private IEnumerator checkForPreInput()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        bool wantsJump = false; //if they preinput

        bool alreadyPressing = PlayerInputs.Instance.playerController.Player.Jump.IsPressed();

        while (isJumping)
        {
            if (alreadyPressing)
            {
                if (!PlayerInputs.Instance.playerController.Player.Jump.IsPressed())
                {
                    alreadyPressing = false;
                }
            }
            else
            {
                if (PlayerInputs.Instance.playerController.Player.Jump.IsPressed())
                {
                    wantsJump = true;
                }
                else
                {
                    wantsJump = false;
                }
            }

            if (Physics2D.Raycast(this.transform.position, Vector2.down, 2f, platformLayer))
            {
                break;
            }

            yield return wait;
        }

        while (!PlayerStateManager.Instance.getState().isGrounded) //wait for player to hit the ground
        {
            yield return wait;
        }

        yield return wait;

        if (wantsJump)
        {
            jump();
        }
    }

    public void resetJumps() //gives jumpsleft back
    {
        jumpsLeft = PlayerDataManager.Instance.getData().jumpAmt;
    }

    public void cancelJump(bool doubleJumping) //param is IF you want the player to be able to still doublejump after you cancel
    {
        if (jumpCo != null)
        {
            StopCoroutine(jumpCo);
        }

        if (!doubleJumping)
        {
            isJumping = false;
            PlayerStateManager.Instance.getState().isJumping = false;
        }

        rb.linearDamping = 0;
        rb.linearVelocityY = 0;
        PlayerGravManager.Instance.resetGrav();
        PlayerStateManager.Instance.getState().isFalling = false;
    }

    private IEnumerator waitForData() //wait for the data manager singleton to be initialized
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while (PlayerDataManager.Instance == null)
        {
            yield return wait;
        }

        resetJumps();
    }
}
