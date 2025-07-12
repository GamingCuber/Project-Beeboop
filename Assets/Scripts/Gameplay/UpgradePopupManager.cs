using UnityEngine;
using System.Collections;
using TMPro;

public class UpgradePopupManager : MonoBehaviour
{
    public static UpgradePopupManager Instance;

    private GameObject[] popups;

    public GameObject popupPre;

    public float floatSpeed;

    public float popupTime;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        popups = new GameObject[3];

        for (int i = 0; i < popups.Length; i++)
        {
            GameObject newPU = Instantiate(popupPre, this.transform);
            newPU.SetActive(false);
            popups[i] = newPU;
        }
    }

    public void showPopup(string name, Collider2D collision)
    {
        GameObject PU = getAvailText();
        TMP_Text text = PU.transform.GetChild(0).GetComponent<TMP_Text>();
        text.text = name + " Unlocked!";
        PU.transform.position = collision.transform.position + Vector3.up * 2;
        PU.SetActive(true);
        StartCoroutine(animatePopup(PU));
    }

    private IEnumerator animatePopup(GameObject text) //fades and moves the text
    {
        float timer = 0;

        TMP_Text PUtext = text.transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text PUPlus = text.transform.GetChild(1).GetComponent<TMP_Text>();

        while (timer < popupTime)
        {
            timer += Time.deltaTime;

            Color32 color = new Color32(255, 255, 255, (byte)Mathf.Lerp(255,0,timer/popupTime));
            PUtext.color = color;
            PUPlus.color = color;

            text.transform.position += Vector3.up * floatSpeed * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        text.SetActive(false);
        yield break;
    }

    private GameObject getAvailText()
    {
        foreach(GameObject g in popups)
        {
            if (!g.activeInHierarchy)
            {
                return g;
            }
        }
        return null;
    }
}
