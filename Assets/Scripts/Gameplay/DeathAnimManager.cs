using UnityEngine;
using System.Collections;

public class DeathAnimManager : MonoBehaviour
{
    public static DeathAnimManager Instance;

    public GameObject piecePre;

    private GameObject[] pieces;

    public int pieceAmt;

    public float pieceLifetime;

    public float maxXForce;

    public float maxYForce;

    public float maxGravScale;

    public GameObject transitionScreen;

    private GameObject player;
    
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        pieces = new GameObject[pieceAmt];

        for (int i = 0; i < pieceAmt; i++)
        {
            GameObject newPiece = Instantiate(piecePre, transform);
            pieces[i] = newPiece;
            newPiece.SetActive(false);
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void doAnimation()
    {
        StartCoroutine(doTransition());
        StartCoroutine(flingShrapnel());
    }

    private IEnumerator flingShrapnel()
    {
        foreach(GameObject g in pieces)
        {
            g.SetActive(true);

            g.transform.position = player.transform.position;

            Rigidbody2D rb = g.GetComponent<Rigidbody2D>();

            rb.gravityScale = 1;

            float Xvelo = Random.Range(-maxXForce, maxXForce);

            float Yvelo = Random.Range(1, maxYForce);

            rb.AddForce(new Vector2(Xvelo, Yvelo), ForceMode2D.Impulse);

            StartCoroutine(sharpnelLifetime(g));
        }

        yield break;
    }

    private IEnumerator sharpnelLifetime(GameObject g)
    {
        Rigidbody2D rb = g.GetComponent<Rigidbody2D>();

        SpriteRenderer sr = g.GetComponent<SpriteRenderer>();

        Color32 c = sr.color;

        sr.color = new Color32(255, 255, 255, 255);

        g.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        float timer = 0;

        while (timer < pieceLifetime)
        {
            timer += Time.deltaTime;

            rb.gravityScale = Mathf.Lerp(1, maxGravScale, timer / pieceLifetime);

            if (timer > pieceLifetime/2)
            {
                Debug.Log((timer - (pieceLifetime / 2)) / (pieceLifetime / 2));
                //color fade to black
                byte a = (byte)Mathf.Lerp(255, 0, (timer - (pieceLifetime / 2)) / (pieceLifetime / 2));
                sr.color = new Color32(a,a,a,255);

                //scale to be teeny
                float scale = Mathf.Lerp(0.2f, 0f, (timer - (pieceLifetime / 2)) / (pieceLifetime / 2));
                g.transform.localScale = new Vector3(scale, scale, scale);
            }

            yield return new WaitForEndOfFrame();
        }

        g.SetActive(false);
        yield break;
    }

    private IEnumerator doTransition()
    {
        Animator anim = transitionScreen.GetComponent<Animator>();


        yield return new WaitForSecondsRealtime(0.25f);

        anim.SetTrigger("Close");

        yield return new WaitForSecondsRealtime(1f);

        anim.SetTrigger("Back");
        anim.SetTrigger("Open");

        yield return new WaitForSecondsRealtime(1f);

        anim.SetTrigger("Back");

        yield break;
    }
}
