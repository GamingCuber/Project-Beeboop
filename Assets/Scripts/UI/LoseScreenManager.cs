using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoseScreenManager : MonoBehaviour
{
    public GameObject lightPanel;

    private Image img;

    public float minChange;

    public float maxChange;

    public float minChangeTime;

    public float maxChangeTime;

    private bool doing = false;

    private void Start()
    {
        img = lightPanel.GetComponent<Image>();

        StartCoroutine(doLights());
    }

    public void restart()
    {
        GameManager.Instance.resetState();
        GameDataManager.Instance.resetData();
        LevelTransition.Instance.doTransition("MainScene");
        MusicManager.Instance.transitionSong("DoubleJump");
    }

    public void quit()
    {
        Application.Quit();
    }

    private IEnumerator doLights()
    {
        while (true)
        {
            if (!doing)
            {
                StartCoroutine(flashLight());
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator flashLight()
    {
        doing = true;

        float timer = 0;

        float time = Random.Range(minChangeTime, maxChangeTime);

        float curA = img.color.a;

        Color32 color = img.color;

        float change = Random.Range(minChange, maxChange);

        if (Random.Range(0, 2) == 0)
        {
            change *= -1;
        }

        float target = img.color.a + change;

        if (target > 255)
        {
            target = 255;
        }
        else if (target < 0)
        {
            target = 0;
        }

        while (timer < time)
        {
            timer += Time.deltaTime;

            float a = Mathf.Lerp(curA, target, timer / time);

            color.a = (byte)a;

            img.color = color;

            yield return new WaitForEndOfFrame();
        }

        doing = false;

        yield break;
    }
}
