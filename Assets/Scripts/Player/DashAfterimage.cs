using UnityEngine;
using System.Collections;

public class DashAfterimage : MonoBehaviour
{
    public static DashAfterimage Instance;

    public GameObject afterimagePre;

    private GameObject[] afterimages;

    public int imagePerDash;

    public float imageFadeTime;

    private GameObject player;

    private Coroutine co;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        afterimages = new GameObject[imagePerDash];

        for (int i = 0; i < afterimages.Length; i++)
        {
            GameObject newGO = Instantiate(afterimagePre, this.transform);
            newGO.SetActive(false);
            afterimages[i] = newGO;
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void cancelAfterImage()
    {
        StopCoroutine(co);
    }

    public void doAfterimage()
    {
        co = StartCoroutine(afterimageCo());
    }

    private IEnumerator afterimageCo() //to actually like 'spawn' the afterimages
    {
        int curAmt = 0; //how many afterimages have spawned

        float timer = 0;

        float dashTime = PlayerDataManager.Instance.getData().dashTime;

        float interval = dashTime / imagePerDash;

        while (curAmt < imagePerDash)
        {
            timer += Time.deltaTime;

            if (timer >= interval)
            {
                curAmt++;
                timer = 0;

                GameObject img = getAvailImg();

                img.SetActive(true);
                SpriteRenderer sr = img.GetComponent<SpriteRenderer>();
                sr.flipX = player.GetComponent<SpriteRenderer>().flipX;
                sr.sprite = player.GetComponent<SpriteRenderer>().sprite;
                img.transform.position = player.transform.position;
                StartCoroutine(fadeImage(img));
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator fadeImage(GameObject obj) //to fade each afterimage
    {
        float timer = 0;

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

        while (timer < imageFadeTime)
        {
            timer += Time.deltaTime;

            Color32 color = sr.color;
            float a = Mathf.Lerp(255, 0, timer / imageFadeTime);
            color.a = (byte)a;
            sr.color = color;
            yield return new WaitForEndOfFrame();
        }

        obj.SetActive(false);
        yield break;
    }

    private GameObject getAvailImg()
    {
        foreach(GameObject g in afterimages)
        {
            if (!g.activeInHierarchy)
            {
                return g;
            }
        }
        return null;
    }
}
