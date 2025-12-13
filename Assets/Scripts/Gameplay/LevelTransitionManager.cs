using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionManager : MonoBehaviour
{
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
            GameDataManager.Instance.nextLevel();
        }
    }
}
