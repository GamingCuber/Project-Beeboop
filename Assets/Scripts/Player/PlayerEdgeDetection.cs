using System.Collections;
using UnityEngine;

public class PlayerEdgeDetection : MonoBehaviour
{
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        // Double checks to see if it's actually the tilemap
        if (collider.gameObject.layer == LayerMask.NameToLayer("Platform") && collider.gameObject.CompareTag("PlatformJump"))
        {
            if (rb.linearVelocityY < 0)
            {
                giveYLeeway(collider);
            }
            if (rb.linearVelocityX < 0)
            {
                giveXLeeway(collider);
            }
        }

    }
    // Thanks Tarodev for providing the code in this video
    // https://youtu.be/3sWTzMsmdx8?si=MH1R1v4LE9WQyvp5
    private IEnumerator giveXLeeway(Collision2D collider)
    {
        rb.linearVelocityX = 0;

        for (var i = 0; i < 25; i++)
        {
            var dir = transform.position - collider.transform.position;
            transform.position += dir.normalized * 5;
        }
        yield break;
    }
    private IEnumerator giveYLeeway(Collision2D collider)
    {
        rb.linearVelocityY = 0;

        for (var i = 0; i < 25; i++)
        {
            var dir = transform.position - collider.transform.position;
            transform.position += dir.normalized * 5;
        }
        yield break;
    }
}
