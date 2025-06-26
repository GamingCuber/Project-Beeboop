using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour
{
    public Rigidbody2D rb;

    private bool grounded = false;

    private bool canJump = true;

    private bool isJumping = false;

    private int jumpsLeft;

    private Coroutine jumpCo;

    private Coroutine preJumpCo;

    private Coroutine coyoteCo;

    private LayerMask platformLayer;

    private void Start()
    {
        platformLayer = LayerMask.GetMask("Platform");
        StartCoroutine(waitForData());
    }

    void Update()
    {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 1.05f, platformLayer)) //if player is close to ground theyre grounded!
        {
            canJump = true;
            grounded = true;
            
        }
        else
        {
            grounded = false;
        }

        if (!grounded && canJump && coyoteCo == null && jumpsLeft == PlayerDataManager.Instance.getData().jumpAmt) //gives coyote time if they were recently on the ground, they isn't already a co running and if theyre on their first jump
        {
            coyoteCo = StartCoroutine(coyoteTimer());
        }

        if (Input.GetKeyDown(PlayerInputs.Instance.jump) && jumpsLeft > 0 && (canJump || isJumping))
        {
            jump();
        }
    }

    private void jump() //do a jump, cuz this functions gonna get called in more than one places
    {
        isJumping = true;
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

        while (!grounded) //just stall till the player hits the ground to reset grav
        {
            yield return new WaitForEndOfFrame();
        }

        isJumping = false;
        SoundManager.Instance.playsound("fall");
        PlayerGravManager.Instance.resetGrav();
        resetJumps();
        yield break;
    }

    private IEnumerator coyoteTimer() //counts frames for when the player can jump off whilst not grounded
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

        while (!grounded) //wait for player to hit the ground
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

    private void cancelJump() //resets grav, player y velo, redoes the jump coroutine
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
