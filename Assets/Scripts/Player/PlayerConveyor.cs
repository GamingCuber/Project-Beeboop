using UnityEngine;
using System.Collections;

public class PlayerConveyor : MonoBehaviour
{
    private bool onConveyor = false;

    private ConveyorPlatform curConvey;

    private Coroutine conveyCo;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Conveyor"))
        {
            curConvey = collision.gameObject.GetComponent<ConveyorPlatform>();
            attachPlayer();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (onConveyor)
        {
            detachPlayer();
        }
    }

    private void attachPlayer()
    {
        onConveyor = true;
        StartCoroutine(movePlayer());
    }

    private void detachPlayer()
    {
        onConveyor = false;
        curConvey = null;

        if (conveyCo != null)
        {
            StopCoroutine(conveyCo);
        }

        conveyCo = null;
    }

    private IEnumerator movePlayer()//GOTTA FIX
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float speed = curConvey.speed;
        float dir = curConvey.dir;

        while (onConveyor)
        {
            Vector3 targetPos = transform.position + Vector3.right * dir * speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, targetPos, 1);

            yield return wait;
        }
    }
}
