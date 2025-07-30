using UnityEngine;

public class SpikesCollided : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            Debug.Log("Respawn");
            RespawnScript.Instance.respawntocheckpoint();
            PlayerJump.Instance.cancelJump(false);
            DashAfterimage.Instance.cancelAfterImage();
        }  
    }
}
