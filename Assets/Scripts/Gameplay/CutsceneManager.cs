using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public PlayableDirector dir;

    public GameObject skipText;

    public GameObject[] comicObjs;

    void Start()
    {
        Invoke(nameof(startCutscene), 1f);
    }

    void startCutscene()
    {
        foreach(GameObject g in comicObjs)
        {
            g.SetActive(true);  
        }

        dir.Play();
        StartCoroutine(allowSkip());
        Invoke(nameof(endCutscene), (float)(dir.playableAsset.duration));
    }

    private IEnumerator allowSkip()
    {
        skipText.SetActive(true);

        bool clicked = false;

        while (true)
        {
            if (PlayerInputs.Instance.playerController.Player.Jump.WasPressedThisFrame() && !clicked)
            {
                Debug.Log("Hello");
                clicked = true;
                endCutscene();
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void endCutscene()
    {
        LevelTransition.Instance.doTransition("MainScene");
        MusicManager.Instance.transitionSong("DoubleJump");
    }
}
