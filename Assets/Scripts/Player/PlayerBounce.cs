using UnityEngine;

public class PlayerBounce : MonoBehaviour
{
    public Rigidbody2D rb;
    private LayerMask platformLayer;

    void Start()
    {
        platformLayer = LayerMask.GetMask("Platform");
    }
    // void FixedUpdate()
    // {
    //     if (Physics2D.Raycast(transform.position, Vector2.down, 1.3f, platformLayer))
    //     {
    //         GameObject bottomPlatform = Physics2D.Raycast(transform.position, Vector2.down, 1.3f, platformLayer).collider.gameObject;
    //         if (bottomPlatform != null && bottomPlatform.CompareTag("BouncePlatform"))
    //         {
    //             rb.position += new Vector2(0, 1.4f);
    //             rb.linearVelocity = Vector2.zero;
    //             rb.AddForce(Vector2.up * PlayerDataManager.Instance.getData().bounceForce, ForceMode2D.Impulse);
    //         }
    //     }
    // }
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collided = collision.gameObject;
        if (collided.CompareTag("BouncePlatform"))
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * PlayerDataManager.Instance.getData().bounceForce, ForceMode2D.Impulse);
        }
    }

}
