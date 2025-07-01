using UnityEngine;

public class Jumpable_Platform : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlatformJump"))
        {
            Debug.Log("Enter");
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;


        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlatformJump"))

        {
            Debug.Log("left");
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = true;


        }
    }
}
