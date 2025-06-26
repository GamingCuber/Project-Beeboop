using System.Numerics;
using Mono.Cecil.Cil;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerHook : MonoBehaviour
{
    public Rigidbody2D rb;
    public LineRenderer hookLineRenderer;
    [SerializeField]
    private float hookDistanceLimit;
    [SerializeField]
    private float hookSpeed;


    void Update()
    {
        GameObject hook = getClosestHook();
        if (Input.GetKeyDown(PlayerInputs.Instance.hook))
        {


            if (hook != null)
            {
                if (Vector2.Distance(hook.transform.position, transform.position) < hookDistanceLimit)
                {
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
                    rb.AddForce(hookVector * hookSpeed, ForceMode2D.Impulse);
                }

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
            rb.inertia = 5;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            PlayerStateManager.Instance.getState().isHooked = false;
            hookLineRenderer.enabled = false;
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
}
