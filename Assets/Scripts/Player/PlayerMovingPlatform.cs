using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class PlayerMovingPlatform : MonoBehaviour
{
    private bool isAttached = false;
    private GameObject curPlat;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform") && !isAttached)
        {
            isAttached = true;
            curPlat = collision.gameObject;
            transform.SetParent(curPlat.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform") && isAttached)
        {
            isAttached = false;
            transform.SetParent(null);

        }
    }
}
