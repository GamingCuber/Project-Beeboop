using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    private Vector2 startingPos;

    public Vector2 curStart;

    private Vector2 pos1; //initial spot

    private Vector2 pos2; //target spot;

    [Tooltip("how much the platform moves on the x y axis")]
    public Vector2 moveAmt;

    public float moveTime;

    public float stopTime;

    private bool isStopped = false;


    private Coroutine moveCo;

    public float platformVeloX; //used in moving player's position alongside platform
    public float platformVeloY;

    void Start()
    {
        startingPos = this.gameObject.transform.position;
        startMove();
    }

    void startMove() //resets platform and starts move coroutine
    {
        resetPos();
        moveCo = StartCoroutine(movePositionCo());
    }


    IEnumerator movePositionCo()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;

        pos1 = startingPos;

        pos2 = startingPos + moveAmt;

        curStart = pos1;

        Vector2 previousPos = startingPos;

        while (timer < moveTime)
        {
            timer += Time.deltaTime;

            if (!isStopped) //if platform isnt stopped, move
            {
                float xPos = Mathf.Lerp(pos1.x, pos2.x, Mathf.Sin(Mathf.PI / 2 * (timer / moveTime)));
                float yPos = Mathf.Lerp(pos1.y, pos2.y, Mathf.Sin(Mathf.PI / 2 * (timer / moveTime)));

                gameObject.transform.position = new Vector2(xPos, yPos);

                if (timer >= moveTime)
                {
                    timer = 0;
                    isStopped = true;

                    Vector3 tempForStart = pos1; //swap position to lerp btwn
                    pos1 = pos2;
                    pos2 = tempForStart;
                    curStart = pos1;
                }
            }
            else //otherwise wait
            {
                if (timer >= stopTime)
                {
                    timer = 0;
                    isStopped = false;
                }
            }

            platformVeloX = (gameObject.transform.position.x - previousPos.x) / Time.fixedDeltaTime;
            platformVeloY = (gameObject.transform.position.y - previousPos.y) / Time.fixedDeltaTime;
            previousPos = gameObject.transform.position;

            yield return wait;
        }
    }

    void resetPos()
    {
        gameObject.transform.position = startingPos;

        if (moveCo != null)
        {
            StopCoroutine(moveCo);
        }

        moveCo = null;
    }
}
