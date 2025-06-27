using UnityEngine;

public class Jumpable_Platform : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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
