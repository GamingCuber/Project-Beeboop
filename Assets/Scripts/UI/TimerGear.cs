using UnityEngine;
using System.Collections;

public class TimerGear : MonoBehaviour
{
    public RectTransform rect;

    public float minSpeed;

    public float maxSpeed;

    public bool paused;

    private float speed;

    public float dir; //-1 or 1 please

    public float maxWiggle;

    public int wiggleAmt;

    void Update()
    {
        rect.Rotate(0, 0, speed * Time.deltaTime * dir);
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    public void vibrateGear()
    {
        StartCoroutine(vibrateCo());
    }

    private IEnumerator vibrateCo()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        paused = true;
        setSpeed(0);

        int timesMoved = 0;

        float timer = 0;

        float timeToMove = Random.Range(0.05f,0.15f);

        RectTransform rect = this.gameObject.GetComponent<RectTransform>();

        Vector3 ogPos = rect.localPosition;

        Vector3 curPos = ogPos;

        Vector3 newPos = randomizePos(ogPos);

        while (timesMoved < wiggleAmt)
        {
            timer += Time.deltaTime;

            float x = Mathf.Lerp(curPos.x, newPos.x, timer / timeToMove);
            float y = Mathf.Lerp(curPos.y, newPos.y, timer / timeToMove);

            rect.localPosition = new Vector3(x, y, 0);

            if (timer >= timeToMove)
            {
                timer = 0;
                timeToMove = Random.Range(0.03f, 0.15f);
                curPos = newPos;
                newPos = randomizePos(ogPos);
                timesMoved++;
            }

            yield return wait;
        }

        rect.localPosition = ogPos;
        paused = false;
        yield break;
    }

    private Vector3 randomizePos(Vector3 pos)
    {
        return pos + Vector3.up * Random.Range(-maxWiggle, maxWiggle) + Vector3.left * Random.Range(-maxWiggle, maxWiggle);
    }
}
