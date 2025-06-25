using System.Numerics;
using Mono.Cecil.Cil;
using UnityEngine;

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
        if (Input.GetKeyDown(PlayerInputs.Instance.hook))
        {
            if (!PlayerDataManager.Instance.getData().isHooked)
            {
                rb.linearVelocity = UnityEngine.Vector2.zero;
                rb.angularVelocity = 0f;
            }

            PlayerDataManager.Instance.getData().isHooked = true;
            GameObject hook = getClosestHook();
            hookLineRenderer.enabled = true;

            // Draws line between Player and Hook Point
            hookLineRenderer.SetPosition(0, transform.position);
            hookLineRenderer.SetPosition(1, hook.transform.position);

            // Calculates Vector between player and HookPoint for applying Force
            UnityEngine.Vector2 hookVector = (hook.transform.position - transform.position) * Time.fixedDeltaTime;
            rb.AddForce(hookVector * hookSpeed, ForceMode2D.Impulse);
        }
        if (Input.GetKeyUp(PlayerInputs.Instance.hook))
        {
            PlayerDataManager.Instance.getData().isHooked = false;
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
                var currentHookDistanceFromPlayer = Mathf.Abs(UnityEngine.Vector2.Distance(transform.position, hookObject.transform.position));
                var closestHookDistanceFromPlayer = Mathf.Abs(UnityEngine.Vector2.Distance(transform.position, closestHook.transform.position));

                if (currentHookDistanceFromPlayer <= closestHookDistanceFromPlayer)
                {
                    closestHook = hookObject;
                }

            }
        }

        return closestHook;
    }
}
