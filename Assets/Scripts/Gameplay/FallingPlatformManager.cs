
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
        StartCoroutine(waitForReapperance());
    }
    private void reenablePlatform()
    {
        platformBox.enabled = true;
    }

    private IEnumerator waitToDissolve()
    {
        float timer = 0;

        while (timer < secondsUntilDissolve)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        dissolvePlatform();
    }

    private IEnumerator waitForReapperance()
    {
        float timer = 0;

        while (timer < secondsUntilReappearance)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(fixPlatform());
        reenablePlatform();
        triggered = false;
    }

    private IEnumerator wigglePlatform()
    {
        float maxWiggle = 3;

        float timer = 0;

        int dir = 1;

        while (timer <= secondsUntilDissolve)
        {
            Debug.Log(spriteGO.transform.eulerAngles);
            timer += Time.deltaTime;

            float rand = Random.Range(0, maxWiggle);

            float rotation = rand * dir;

            spriteGO.transform.localEulerAngles = new Vector3(0f, 0f, rotation);

            dir *= -1;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator breakPlatform()
    {
        float timer = 0;

        float fallTime = 0.05f;

        while (timer < fallTime)
        {
            timer += Time.deltaTime;

            float rotation = Mathf.Lerp(0, -90, timer / fallTime);

            spriteGO.transform.localEulerAngles = new Vector3(0f, 0f, rotation);

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator fixPlatform()
    {
        float timer = 0;

        float upTime = 0.25f;

        while (timer < upTime)
        {
            timer += Time.deltaTime;
            
            float rotation = Mathf.Lerp(-90, 0, timer / upTime);

            spriteGO.transform.localEulerAngles = new Vector3(0f, 0f, rotation);
            
            yield return new WaitForEndOfFrame();
        }

        spriteGO.GetComponent<SpriteRenderer>().sprite = upSprite;
    }
}
