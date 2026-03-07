using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Unity.VisualScripting;

public class PlayerHook : MonoBehaviour
{
    public Rigidbody2D rb;

    public LineRenderer hookLineRenderer;

    private GameObject hookTarget;

    private GameObject[] hookObjects;

    private Dictionary<GameObject, bool> hookCooldowns = new Dictionary<GameObject, bool>(); //will assign each hook an individual cooldown, so u cant spam

    public float hookReturnSpeed;

    private Coroutine returnCo;

    private void Start()
    {
        hookTarget = GameObject.FindGameObjectWithTag("HookTarget");
        hookObjects = GameObject.FindGameObjectsWithTag("Hook");

        foreach (GameObject hook in hookObjects)
        {
            hookCooldowns[hook] = false;
        }
    }

    void Update()
    {
        if (PlayerStateManager.Instance.getState().canHook && !PlayerStateManager.Instance.state.pausedGame)
        {
            hookReaction();

            if (getClosestAvailHook() != null && PlayerInputs.Instance.playerController.Player.Hook.WasPressedThisFrame())
            {
                doHook();
            }
        }
    }


    private GameObject getClosestAvailHook() //so its both in range and not on cooldown
    {
        GameObject closestHook = null;

        if (hookObjects != null)
        {
            closestHook = hookObjects[0];
            foreach (var hookObject in hookObjects)
            {
                var currentHookDistanceFromPlayer = Mathf.Abs(Vector2.Distance(transform.position, hookObject.transform.position));
                var closestHookDistanceFromPlayer = Mathf.Abs(Vector2.Distance(transform.position, closestHook.transform.position));

                if (currentHookDistanceFromPlayer <= closestHookDistanceFromPlayer && !checkForCD(hookObject))
                {
                    closestHook = hookObject;
                }
            }
        }

        //if closest hook is within the hook range then return the hook AND is off cd (just b/c we initally set closest to hookobject[0] and it could be on cd), otherwise null
        if (Mathf.Abs(Vector2.Distance(transform.position, closestHook.transform.position)) < PlayerDataManager.Instance.getData().hookDistanceLimit && !checkForCD(closestHook))
        {
            spawnTarget(closestHook);
            return closestHook;
        }
        else
        {
            hideTarget();
            return null;
        }
    }

    private bool checkForCD(GameObject hook)
    {
        foreach (KeyValuePair<GameObject, bool> kv in hookCooldowns)
        {
            if (kv.Key == hook && kv.Value)
            {
                return true;
            }
            else if (kv.Key == hook && !kv.Value)
            {
                return false;
            }
        }
        return false;
    }

    private void hookReaction()
    {
        foreach (GameObject hook in hookObjects)
        {
            float dist = Vector2.Distance(transform.position, hook.transform.position);

            // if hook within range, then it can be red or green, otherwise white
            if (dist < PlayerDataManager.Instance.getData().hookDistanceLimit)
            {
                //if individual hook is on cd, red, else green
                if (checkForCD(hook))
                {
                    hook.GetComponent<SpriteRenderer>().color = Color.red;
                }
                else
                {
                    hook.GetComponent<SpriteRenderer>().color = Color.green;
                }
            }
            else
            {
                hook.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    private void putOnCooldown(GameObject hook)
    {
        StartCoroutine(hookCooldownCo(hook));
    }

    private IEnumerator hookCooldownCo(GameObject hook)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;
        float totalTime = PlayerDataManager.Instance.getData().hookPointCooldown;

        hookCooldowns[hook] = true;

        while (timer < totalTime)
        {
            timer += Time.deltaTime;

            yield return wait;
        }

        hookCooldowns[hook] = false;
        yield break;
    }

    private void doHook()
    {
        SoundManager.Instance.playPlayerSound("Hook");
        StartCoroutine(hookCo());
    }

    private IEnumerator hookCo()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        GameObject hook = getClosestAvailHook();

        putOnCooldown(hook);

        PlayerStateManager.Instance.getState().isHooked = true;
        PlayerStateManager.Instance.getState().isHookPulling = true;
        PlayerStateManager.Instance.getState().keepMomentum = true;

        PlayerJump.Instance.resetJumps();
        PlayerDash.Instance.resetDash();

        rb.linearDamping = 0;

        // Resets player force for a frame, then continues movement in the next line
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (returnCo != null)
        {
            StopCoroutine(returnCo);
            returnCo = null;
        }

        hookLineRenderer.enabled = true;

        Vector2 hookVector = (hook.transform.position - transform.position).normalized;

        float distFromHook = Vector3.Distance(this.transform.position, hook.transform.position);

        if (distFromHook <= PlayerDataManager.Instance.getData().maxInitBoostRange)
        {
            float sqrtMax = Mathf.Sqrt(PlayerDataManager.Instance.getData().maxInitBoostRange);
            float boostForce = (sqrtMax - Mathf.Sqrt(distFromHook))/sqrtMax;
            rb.AddForce(hookVector * PlayerDataManager.Instance.getData().initHookStrength * boostForce, ForceMode2D.Impulse);
            Debug.Log("power is " + boostForce * PlayerDataManager.Instance.getData().initHookStrength + "w/ distance " + distFromHook);
        }

        while (PlayerStateManager.Instance.getState().isHooked)
        {
            // Calculates Vector between player and HookPoint for applying Force
            hookVector = (hook.transform.position - transform.position).normalized;
            rb.AddForce(hookVector * PlayerDataManager.Instance.getData().hookSpeed, ForceMode2D.Force);

            // Draws line between Player and Hook Point
            hookLineRenderer.SetPosition(0, transform.position);
            hookLineRenderer.SetPosition(1, hook.transform.position);

            // if player is too close to hook or lets go of hook, they detach
            float playerHookDist = Mathf.Abs(Vector2.Distance(transform.position, hook.transform.position));
            if (!PlayerInputs.Instance.playerController.Player.Hook.IsPressed() || playerHookDist < PlayerDataManager.Instance.getData().hookCancelDistance)
            {
                break;
            }

            if (!hookLineRenderer.enabled)
            {
                hookLineRenderer.enabled = true;
            }

            yield return wait;
        }

        // Makes you slower when you let go of the hook
        PlayerStateManager.Instance.getState().isHookPulling = false;
        rb.linearDamping = PlayerDataManager.Instance.getData().dampeningPostHook;
        // rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        // rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        yield return new WaitForSecondsRealtime(0.1f);

        returnCo = StartCoroutine(hookReturn(hook));

        // Increases grav so you fall faster
        PlayerStateManager.Instance.getState().isFalling = true;

        //wait till player hits ground
        while (!PlayerStateManager.Instance.getState().isGrounded)
        {
            yield return wait;
        }

        PlayerStateManager.Instance.getState().isHooked = false;

        rb.linearDamping = 0;

        yield break;
    }

    //just qol, for the little indicator to show what hook ur locked on to
    private void spawnTarget(GameObject hook)
    {
        if (hookTarget != null)
        {
            hookTarget.SetActive(true);

            hookTarget.transform.position = hook.transform.position;
        }
    }

    private void hideTarget()
    {
        if (hookTarget != null)
        {
            hookTarget.SetActive(false);
        }
    }

    private IEnumerator hookReturn(GameObject hook)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        Vector3 initHookPos = hook.transform.position;

        Vector3 hookReturnPos = initHookPos;

        float returnTime = 0;

        while (Vector3.Distance(transform.position, hookReturnPos) > 0.5)
        {
            returnTime += Time.deltaTime;

            hookReturnPos = Vector3.MoveTowards(hookReturnPos, transform.position, hookReturnSpeed * (returnTime * 10) * Time.deltaTime);
            hookLineRenderer.SetPosition(0, transform.position);
            hookLineRenderer.SetPosition(1, hookReturnPos);

            yield return wait;
        }

        hookLineRenderer.enabled = false;

        yield break;
    }
}
