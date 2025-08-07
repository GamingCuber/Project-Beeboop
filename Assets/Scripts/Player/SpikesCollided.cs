using UnityEngine;

public class SpikesCollided : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            Debug.Log("Respawn");
            PlayerStateManager.Instance.getState().isDead = true;
            PlayerJump.Instance.cancelJump(false);
            DashAfterimage.Instance.cancelAfterImage();
            DeathAnimManager.Instance.doAnimation();
            gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
            Invoke(nameof(death), 1f);
        }  
    }

    void death()
    {
        RespawnScript.Instance.respawntocheckpoint();
        Invoke(nameof(giveInputs), 0.5f);
    }

    void giveInputs()
    {
        PlayerStateManager.Instance.getState().isDead = false;
        TimerGearManager.Instance.vibrateGears();
    }
}
