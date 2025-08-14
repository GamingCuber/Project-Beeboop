using UnityEngine;
using System.Collections;

public class FallingPlatformManager : MonoBehaviour
{
    public BoxCollider2D platformBox;

    public GameObject spriteGO;
    [SerializeField]
    private float secondsUntilDissolve;
    [SerializeField]
    private float secondsUntilReappearance;

    private bool triggered = false;

    public Sprite upSprite;

    public Sprite downSprite;

    private Coroutine wiggleCo;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.transform.position.y > transform.position.y)
                if (collision.gameObject.transform.position.y > transform.position.y && !triggered)
                {
                    triggered = true;
                    wiggleCo = StartCoroutine(wigglePlatform());
                    Invoke(nameof(dissolvePlatform), secondsUntilDissolve);
                }
        }
    }

    private void dissolvePlatform()
    {
        StopCoroutine(wiggleCo);
        spriteGO.GetComponent<SpriteRenderer>().sprite = downSprite;
        StartCoroutine(breakPlatform());
        platformBox.enabled = false;
        Invoke(nameof(reenablePlatform), secondsUntilReappearance);
        StartCoroutine(waitForReapperance());
    }
    private void reenablePlatform()
    {
        platformBox.enabled = true;
    }

    private IEnumerator waitToDissolve()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        float timer = 0;

        while (timer < secondsUntilDissolve)
        {
            timer += Time.deltaTime;
            yield return wait;
        }

        dissolvePlatform();
    }

    private IEnumerator waitForReapperance()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        float timer = 0;

        while (timer < secondsUntilReappearance)
        {
            timer += Time.deltaTime;
            yield return wait;
        }

        StartCoroutine(fixPlatform());
        reenablePlatform();
        triggered = false;
    }

    private IEnumerator wigglePlatform()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float maxWiggle = 3;

        float timer = 0;

        int dir = 1;

        while (timer <= secondsUntilDissolve)
        {
            timer += Time.deltaTime;

            float rand = Random.Range(0, maxWiggle);

            float rotation = rand * dir;

            spriteGO.transform.localEulerAngles = new Vector3(0f, spriteGO.transform.rotation.y, rotation);

            dir *= -1;

            yield return wait;
        }
    }

    private IEnumerator breakPlatform()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;

        float fallTime = 0.05f;

        while (timer < fallTime)
        {
            timer += Time.deltaTime;

            float rotation = Mathf.Lerp(0, -90, timer / fallTime);

            spriteGO.transform.localEulerAngles = new Vector3(0f, spriteGO.transform.rotation.y, rotation);

            yield return wait;
        }
    }

    private IEnumerator fixPlatform()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;

        float upTime = 0.25f;

        while (timer < upTime)
        {
            timer += Time.deltaTime;

            float rotation = Mathf.Lerp(-90, 0, timer / upTime);

            spriteGO.transform.localEulerAngles = new Vector3(0f, spriteGO.transform.rotation.y, rotation);

            yield return wait;
        }

        spriteGO.GetComponent<SpriteRenderer>().sprite = upSprite;
    }
}