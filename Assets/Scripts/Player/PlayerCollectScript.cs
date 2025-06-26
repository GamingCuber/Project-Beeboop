using UnityEngine;

public class PlayerCollectScript : MonoBehaviour
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
        if (collision.gameObject.CompareTag("Collectable"))
        {
            Debug.Log("Is Collided");
            PlayerDataManager.Instance.getData().upgradescollected += 1;
            Destroy(collision.gameObject);
         }



    }
}
