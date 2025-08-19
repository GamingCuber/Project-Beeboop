using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditScreenManager : MonoBehaviour
{
    public PlayableDirector dir;

    private void Start()
    {
        Invoke(nameof(startCutscene), 0.1f + 1f);
    }

    private void startCutscene()
    {
        dir.Play();
        StartCoroutine(waitForCutscene());
    }

    private IEnumerator waitForCutscene()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;

        float startTime = Time.realtimeSinceStartup;

        float totalTime = (float)dir.playableAsset.duration;

        bool clicked = false;

        while (timer < totalTime)
        {
            timer = Time.realtimeSinceStartup - startTime;

            if (PlayerInputs.Instance.playerController.Player.Jump.WasPerformedThisFrame() && !clicked)
            {
                clicked = true;
                endCutscene();
            }

            yield return wait;
        }

        endCutscene();
        yield break;
    }

    public void endCutscene()
    {
        LevelTransition.Instance.doTransition("StartMenu");
    }
}
