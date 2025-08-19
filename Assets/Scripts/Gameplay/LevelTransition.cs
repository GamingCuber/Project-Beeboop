using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public static LevelTransition Instance;

    public GameObject transitionObj;

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

        yield return new WaitForSecondsRealtime(transitionTime);

        anim.clip = fadeOutAnim;
        anim.Play();

        if (scene.Equals("MainScene"))
        {
            resetStats();
            StartCoroutine(waitForTimer());
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
