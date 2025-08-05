using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerHook : MonoBehaviour
{
    public Rigidbody2D rb;

    public LineRenderer hookLineRenderer;

    public GameObject hookTarget;

    private GameObject[] hookObjects;

    private Dictionary<GameObject, bool> hookCooldowns = new Dictionary<GameObject, bool>(); //will assign each hook an individual cooldown, so u cant spam

    public float hookReturnSpeed;

    private Coroutine returnCo;

    private void Start()
    {
        hookObjects = GameObject.FindGameObjectsWithTag("Hook");

        foreach (GameObject hook in hookObjects)
        {
            hookCooldowns[hook] = false;
        }
    }

    void Update()
    {
        if (PlayerStateManager.Instance.getState().canHook)
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
        float timer = 0;
        float totalTime = PlayerDataManager.Instance.getData().hookPointCooldown;

        hookCooldowns[hook] = true;

        while (timer < totalTime)
        {
            timer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        hookCooldowns[hook] = false;
        yield break;
    }

    private void doHook()
    {
        StartCoroutine(hookCo());
    }

    private IEnumerator hookCo()
    {
        GameObject hook = getClosestAvailHook();

        putOnCooldown(hook);

        PlayerStateManager.Instance.getState().isHooked = true;
        PlayerStateManager.Instance.getState().keepMomentum = true;

        rb.linearDamping = 0;

        // Resets player force for a frame, then continues movement in the next line
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (returnCo != null)
        {
            Debug.Log("stopped co");
            StopCoroutine(returnCo);
            returnCo = null;
        }

        hookLineRenderer.enabled = true;

        while (PlayerStateManager.Instance.getState().isHooked)
        {
            // Calculates Vector between player and HookPoint for applying Force
            Vector2 hookVector = (hook.transform.position - transform.position).normalized;
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

            yield return new WaitForEndOfFrame();
        }

        // Makes you slower when you let go of the hook
        rb.linearDamping = PlayerDataManager.Instance.getData().dampeningPostHook;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        yield return new WaitForSecondsRealtime(0.1f);

        returnCo = StartCoroutine(hookReturn(hook));

        // Increases grav so you fall faster
        PlayerStateManager.Instance.getState().isFalling = true;

        //wait till player hits ground
        while (!PlayerStateManager.Instance.getState().isGrounded)
        {
            yield return new WaitForEndOfFrame();
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
        Vector3 initHookPos = hook.transform.position;

        Vector3 hookReturnPos = initHookPos;

        float returnTime = 0;

        while (Vector3.Distance(transform.position, hookReturnPos) > 0.5)
        {
            returnTime += Time.deltaTime;

            hookReturnPos = Vector3.MoveTowards(hookReturnPos, transform.position, hookReturnSpeed * (returnTime * 10) * Time.deltaTime);
            hookLineRenderer.SetPosition(0, transform.position);
            hookLineRenderer.SetPosition(1, hookReturnPos);

            yield return new WaitForEndOfFrame();
        }

        hookLineRenderer.enabled = false;

        yield break;
    }
}
