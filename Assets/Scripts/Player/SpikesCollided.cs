using UnityEngine;

public class SpikesCollided : MonoBehaviour
{
    private bool alreadyTriggered = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn") && !alreadyTriggered)
        {
            alreadyTriggered = true;
            PlayerStateManager.Instance.getState().isDead = true;
            PlayerStateManager.Instance.getState().deathNumber++;
            PlayerJump.Instance.cancelJump(false);
            DashAfterimage.Instance.cancelAfterImage();
            DeathAnimManager.Instance.doAnimation();
            SoundManager.Instance.playPlayerSound("death");
            gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
            Invoke(nameof(onDeath), 1f);
        }
    }

    void onDeath()
    {
        CameraZoomManager.Instance.resetZoom();
        RespawnScript.Instance.respawntocheckpoint();
        Invoke(nameof(giveInputs), 0.5f);
    }

    void giveInputs()
    {
        PlayerStateManager.Instance.getState().isDead = false;
        TimerGearManager.Instance.vibrateGears();
        alreadyTriggered = false;
    }
}
