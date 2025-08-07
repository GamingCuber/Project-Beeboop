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
        if (scene.Equals("MainScene"))
        {
            // Resets some Player States so game functions properly
            PlayerStateManager.Instance.getState().deathNumber = 0;
            PlayerStateManager.Instance.getState().totalTime = 0;
            PlayerStateManager.Instance.getState().canDash = false;
            PlayerStateManager.Instance.getState().canHook = false;
            PlayerStateManager.Instance.getState().canDoubleJump = false;
        }
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

        yield return new WaitForSecondsRealtime(anim.clip.length);

        transitionObj.SetActive(false);

        yield break;
    }
}
