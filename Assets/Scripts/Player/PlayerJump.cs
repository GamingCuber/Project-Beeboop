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
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 1.01f, platformLayer)) //if player is close to ground theyre PlayerStateManager.Instance.getState().isGrounded!
        {
            canJump = true;
            PlayerStateManager.Instance.getState().isGrounded = true;
            rb.linearDamping = 0;

            if (jumpsLeft < PlayerDataManager.Instance.getData().jumpAmt && !isJumping)
            {
                resetJumps();
            }

            if (PlayerStateManager.Instance.getState().keepMomentum && Mathf.Abs(rb.linearVelocityY) < 0.05)
            {
                PlayerStateManager.Instance.getState().keepMomentum = false;
            }
        }
        else
        {
            PlayerStateManager.Instance.getState().isGrounded = false;
        }

        if (Input.GetKeyDown(PlayerInputs.Instance.jump) && jumpsLeft > 0 && (canJump || isJumping))
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
        cancelJump();
        jumpCo = StartCoroutine(doJump());
    }

    private IEnumerator doJump() //itll addforce and watch for conditions to mess with the gravity
    {
        rb.AddForceY(PlayerDataManager.Instance.getData().jumpStr, ForceMode2D.Impulse);

        while (true)
        {
            if (!Input.GetKey(PlayerInputs.Instance.jump) || rb.linearVelocityY < -0.5) //if they let go or player starts falling, increase grav so they fall faster
            {
                PlayerGravManager.Instance.setGrav(PlayerDataManager.Instance.getData().jumpFallGrav);
                PlayerStateManager.Instance.getState().isFalling = true;
                break;
            }
            else if (Mathf.Abs(rb.linearVelocityY) < 1.5) //if player is at the peak of their jump, so when the abs velo is low
            {
                PlayerGravManager.Instance.setGrav(PlayerDataManager.Instance.getData().jumpFloatGrav);
            }
            else
            {
                PlayerGravManager.Instance.resetGrav();
            }
            yield return new WaitForEndOfFrame();
        }

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
        SoundManager.Instance.playsound("fall");
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

        while (isJumping)
        {
            if (Input.GetKey(PlayerInputs.Instance.jump) && Physics2D.Raycast(this.transform.position, Vector2.down, 1.5f, platformLayer))
            {
                wantsJump = true;
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

    public void cancelJump() //resets grav, player y velo, redoes the jump coroutine
    {
        if (jumpCo != null)
        {
            StopCoroutine(jumpCo);
        }

        rb.linearVelocityY = 0;
        PlayerGravManager.Instance.resetGrav();
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
