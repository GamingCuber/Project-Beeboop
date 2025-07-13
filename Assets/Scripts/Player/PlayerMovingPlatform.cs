using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerMovingPlatform : MonoBehaviour
{
    private bool isAttached = false;

    private Coroutine movingPlatCo;

    private MovingPlatform curPlat;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform") && !isAttached)
        {
            Debug.Log("moving");
            curPlat = collision.gameObject.GetComponent<MovingPlatform>();
            attachPlayer();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isAttached)
        {
            Debug.Log("gone");
            detachPlayer();
        }
    }

    private void attachPlayer()
    {
        isAttached = true;
        StartCoroutine(movePlayer());
    }

    private void detachPlayer()
    {
        isAttached = false;
        curPlat = null;

        if (movingPlatCo != null)
        {
            StopCoroutine(movingPlatCo);
        }

        movingPlatCo = null;
    }

    private IEnumerator movePlayer()//GOTTA FIX
    {
        GameObject platGO = curPlat.gameObject;

        Vector3 distFromPlat = transform.position - platGO.transform.position;

        while (isAttached)
        {
            if (!PlayerStateManager.Instance.getState().isMoving && !PlayerStateManager.Instance.getState().isJumping)
            {
                transform.position = platGO.transform.position + distFromPlat;
            }
            else
            {
                distFromPlat = transform.position - platGO.transform.position;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}