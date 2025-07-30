using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour
{
    public static PlayerJump Instance;

    public Rigidbody2D rb;

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
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 1.1f, platformLayer)) //if player is close to ground theyre PlayerStateManager.Instance.getState().isGrounded!
        {
            canJump = true;
            PlayerStateManager.Instance.getState().isGrounded = true;
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

        if (PlayerInputs.Instance.playerController.Player.Jump.WasPressedThisFrame() && jumpsLeft > 0 && (canJump || isJumping))
        {
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
        jumpsLeft--;
        cancelJump(true);
        jumpCo = StartCoroutine(doJump());
    }

    private IEnumerator doJump() //itll addforce and watch for conditions to mess with the gravity
    {
        float time = 0;

        Vector3 initPos = this.gameObject.transform.position;

        PlayerData data = PlayerDataManager.Instance.getData();

        PlayerGravManager.Instance.setGrav(0);

        while (time < PlayerDataManager.Instance.getData().jumpTime)
        {
            time += Time.deltaTime;

            if (!PlayerInputs.Instance.playerController.Player.Jump.IsPressed()) //if they let go or player starts falling, increase grav so they fall faster
            {
                break;
            }

            Vector3 pos = this.gameObject.transform.position;

            if (time < data.jumpTime)
            {
                pos.y = Mathf.Lerp(initPos.y, initPos.y + (data.jumpHeight * (1f - data.percentApex)), (Mathf.Sin(Mathf.PI / 2f * (time / data.jumpTime))));

                if (time > data.jumpTime / 2)
                {
                    PlayerGravManager.Instance.setGrav(PlayerDataManager.Instance.getData().jumpFallGrav * Mathf.Lerp(0, 1, (time - data.jumpTime / 2f) / (data.jumpTime / 2f)));
                }
            }

            this.gameObject.transform.position = pos;

            RaycastHit2D coll = Physics2D.Raycast(this.transform.position, Vector2.up, 1.1f, platformLayer);

            if (coll.collider != null && !coll.collider.gameObject.TryGetComponent<PlatformEffector2D>(out PlatformEffector2D PE))
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        rb.linearDamping = 0;
        PlayerStateManager.Instance.getState().isFalling = true;

        if (preJumpCo != null)
        {
            StopCoroutine(preJumpCo);
        }
        preJumpCo = StartCoroutine(checkForPreInput());

        while (!PlayerStateManager.Instance.getState().isGrounded) //just stall till the player hits the ground to reset grav
        {
            yield return new WaitForEndOfFrame();
        }

        isJumping = false;
        PlayerStateManager.Instance.getState().isJumping = false;
        SoundManager.Instance.playPlayerSound("fall");
        yield break;
    }

    private IEnumerator coyoteTimer() //counts frames for when the player can jump off whilst not PlayerStateManager.Instance.getState().isGrounded
    {
        int count = 0;

        while (count < PlayerDataManager.Instance.getData().coyoteFrames)
        {
            count++;
            yield return new WaitForEndOfFrame();
        }

        canJump = false;
        coyoteCo = null;
        yield break;
    }

    private IEnumerator checkForPreInput()
    {
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

            yield return new WaitForEndOfFrame();
        }

        while (!PlayerStateManager.Instance.getState().isGrounded) //wait for player to hit the ground
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();

        if (wantsJump)
        {
            jump();
        }
    }

    private void resetJumps() //gives jumpsleft back
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
            Debug.Log("isjumping flase");
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
        while (PlayerDataManager.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }

        resetJumps();
    }
}
