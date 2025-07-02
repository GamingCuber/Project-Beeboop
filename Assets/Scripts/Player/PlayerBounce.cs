using UnityEngine;

public class PlayerBounce : MonoBehaviour
{
    public Rigidbody2D rb;
    private LayerMask platformLayer;

    void Start()
    {
        platformLayer = LayerMask.NameToLayer("Platform");
    }
    void Update()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, 1.01f, platformLayer))
        {
            Debug.Log("bouning");

            GameObject bottomPlatform = Physics2D.Raycast(transform.position, Vector2.down, 1.01f, platformLayer).collider.gameObject;

            if (bottomPlatform != null && bottomPlatform.CompareTag("BouncePlatform"))
            {
                rb.AddForce(Vector2.up * PlayerDataManager.Instance.getData().bounceForce);
            }
        }
    }
}
