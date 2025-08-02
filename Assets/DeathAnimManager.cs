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

        float timer = 0;

        while (timer < pieceLifetime)
        {
            timer += Time.deltaTime;

            rb.gravityScale = Mathf.Lerp(1, maxGravScale, timer / pieceLifetime);

            yield return new WaitForEndOfFrame();
        }

        g.SetActive(false);
        yield break;
    }
}
