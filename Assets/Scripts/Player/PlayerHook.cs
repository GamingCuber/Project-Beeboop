using System.Numerics;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.U2D;
using Vector2 = UnityEngine.Vector2;

public class PlayerHook : MonoBehaviour
{
    public Rigidbody2D rb;
    public LineRenderer hookLineRenderer;
    void Update()
    {

        if (PlayerStateManager.Instance.getState().canHook)
        {
            GameObject hook = getClosestHook();
            hookReaction(hook, Vector2.Distance(hook.transform.position, transform.position));
            if (Input.GetKeyDown(PlayerInputs.Instance.hook))
            {
                if (hook != null)
                {
                    if (Vector2.Distance(hook.transform.position, transform.position) < PlayerDataManager.Instance.getData().hookDistanceLimit)
                    {
                        rb.linearDamping = 0;
                        if (!PlayerStateManager.Instance.getState().isHooked)
                        {
                            // Resets player force for a frame, then continues movement in the next line
                            rb.constraints = RigidbodyConstraints2D.FreezeAll;
                        }
                        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                        PlayerStateManager.Instance.getState().isHooked = true;

                        hookLineRenderer.enabled = true;

                        // Calculates Vector between player and HookPoint for applying Force
                        Vector2 hookVector = (hook.transform.position - transform.position) * Time.fixedDeltaTime;
                        rb.AddForce(hookVector * PlayerDataManager.Instance.getData().hookSpeed, ForceMode2D.Impulse);
                    }
                    PlayerStateManager.Instance.getState().keepMomentum = true;
                }
            }

            if (Input.GetKey(PlayerInputs.Instance.hook) && PlayerStateManager.Instance.getState().isHooked)
            {
                // Draws line between Player and Hook Point
                hookLineRenderer.SetPosition(0, transform.position);
                hookLineRenderer.SetPosition(1, hook.transform.position);
            }

            if (Input.GetKeyUp(PlayerInputs.Instance.hook))
            {
                // Makes you slower when you let go of the hook
                rb.linearDamping = PlayerDataManager.Instance.getData().dampeningPostHook;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;

                // Increases grav so you fall faster    
                PlayerStateManager.Instance.getState().isFalling = true;

                PlayerStateManager.Instance.getState().isHooked = false;
                hookLineRenderer.enabled = false;
                // Resets drag on the player
                if (PlayerStateManager.Instance.getState().isGrounded)
                {
                    rb.linearDamping = 0;
                }
            }
        }
    }


    private GameObject getClosestHook()
    {
        var hookObjects = GameObject.FindGameObjectsWithTag("Hook");
        GameObject closestHook = null;
        if (hookObjects != null)
        {
            closestHook = hookObjects[0];
            foreach (var hookObject in hookObjects)
            {
                var currentHookDistanceFromPlayer = Mathf.Abs(Vector2.Distance(transform.position, hookObject.transform.position));
                var closestHookDistanceFromPlayer = Mathf.Abs(Vector2.Distance(transform.position, closestHook.transform.position));

                if (currentHookDistanceFromPlayer <= closestHookDistanceFromPlayer)
                {
                    closestHook = hookObject;
                }

            }
        }

        return closestHook;
    }

    private void hookReaction(GameObject hook, float distanceBetweenHook)
    {
        if (distanceBetweenHook <= PlayerDataManager.Instance.getData().hookDistanceLimit)
        {
            hook.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            hook.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
