using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour
{
    public static LevelTransition Instance;

    public GameObject transitionObj;

    public GameObject dyingObj;

    private Coroutine dyingCo = null;

    public Animation anim;

    public AnimationClip fadeInAnim;

    public AnimationClip fadeOutAnim;

    public float transitionTime = 1f;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void doTransition(string scene)
    {
        StartCoroutine(doTransitionCo(scene));
    }

    private IEnumerator doTransitionCo(string scene)
    {
        transitionObj.SetActive(true);

        anim.clip = fadeInAnim;
        anim.Play();

        yield return new WaitForSecondsRealtime(anim.clip.length);

        SceneManager.LoadScene(scene);

        if (dyingObj.activeInHierarchy)
        {
            dyingObj.SetActive(false);
        }

        yield return new WaitForSecondsRealtime(transitionTime);

        anim.clip = fadeOutAnim;
        anim.Play();

        if (scene.Equals("MainScene"))
        {
            resetStats();
            StartCoroutine(waitForTimer());
        }
        else if (scene.Equals("StartMenu"))
        {
            resetStats();
        }

        yield return new WaitForSecondsRealtime(anim.clip.length);

        transitionObj.SetActive(false);

        yield break;
    }

    public void resetStats()
    {
        PlayerStateManager.Instance.getState().canDash = false;
        PlayerStateManager.Instance.getState().canHook = false;
        PlayerStateManager.Instance.getState().canDoubleJump = false;
        PlayerStateManager.Instance.getState().deathNumber = 0;
        PlayerStateManager.Instance.getState().totalTime = 0;
        GameDataManager.Instance.resetData();
    }

    public void startDyingEffect()
    {
        dyingObj.SetActive(true);
        dyingCo = StartCoroutine(dieCo());
    }

    public void stopDying()
    {
        if (dyingCo != null)
        {
            StopCoroutine(dyingCo);
        }
        dyingCo = null;

        StartCoroutine(aliveCo());
    }

    private IEnumerator dieCo()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;

        float waitTime = 3f;

        Image img = dyingObj.GetComponent<Image>();
        Color32 color = img.color;

        while (timer < waitTime)
        {
            timer += Time.deltaTime;

            float a = Mathf.Lerp(0, 255, timer / waitTime);
            color.a = (byte)a;
            img.color = color;

            if (timer > waitTime - 0.25f)
            {
                GameTimer.Instance.gameLost();
                yield break;
            }

            yield return wait;
        }
    }

    public void resetDeath()
    {
        Color32 c = Color.black;
        c.a = 0;
        dyingObj.GetComponent<Image>().color = c;
        stopDying();
    }

    //to fade the thing back to clear if player picks up battery during the time theyre bouta die
    private IEnumerator aliveCo()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;

        float waitTime = 3f;

        Image img = dyingObj.GetComponent<Image>();
        Color32 initColor = img.color;
        Color32 color = img.color;

        if (initColor.a == 0)
        {
            yield break;
        }

        while (timer < waitTime)
        {
            timer += Time.deltaTime;

            float a = Mathf.Lerp(initColor.a, 0, timer / waitTime);
            color.a = (byte)a;
            img.color = color;

            yield return wait;
        }

        dyingObj.SetActive(false);
        yield break;
    }

    private IEnumerator waitForTimer()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while (GameTimer.Instance == null)
        {
            yield return wait;
        }

        GameTimer.Instance.timeLeft = GameDataManager.Instance.totalTime;
    }
}
