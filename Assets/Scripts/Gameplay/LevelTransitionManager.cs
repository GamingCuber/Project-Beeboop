using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionManager : MonoBehaviour
{
    public string nextSceneName;

    public string nextSongName;

    private Collider2D coll;

    private void Start()
    {
        coll = this.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            GameDataManager.Instance.updateTime(GameTimer.Instance.getTimeLeft());
            LevelTransition.Instance.doTransition(nextSceneName);
            MusicManager.Instance.transitionSong(nextSongName);
        }
    }
}
