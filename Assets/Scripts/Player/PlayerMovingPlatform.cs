using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerMovingPlatform : MonoBehaviour
{
    private bool isAttached = false;
    private MovingPlatform curPlat;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform") && !isAttached)
        {
            curPlat = collision.gameObject.GetComponent<MovingPlatform>();
            attachPlayer();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform") && isAttached)
        {
            detachPlayer();
        }
    }

    private void attachPlayer()
    {
        isAttached = true;
    }

    private void detachPlayer()
    {
        isAttached = false;
        curPlat = null;
    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    private void movePlayer()
    {
        if (isAttached)
        {
            PlayerStateManager.Instance.getState().isFalling = false;
            gameObject.GetComponent<Rigidbody2D>().linearVelocityY = curPlat.platformVeloY;
        }
    }
}
