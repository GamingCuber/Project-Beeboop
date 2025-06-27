using UnityEngine;

public class CheckPointScript : MonoBehaviour
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
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            PlayerDataManager.Instance.getData().current_checkpoint = collision.gameObject;
            Debug.Log("Collided with a checkpoint");


        }
    }


    
}
