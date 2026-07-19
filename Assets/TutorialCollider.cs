using UnityEngine;

public class TutorialCollider : MonoBehaviour
{
    public static TutorialCollider Instance;

    private bool reachedPoint = false;

    public void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public bool getReachedPoint() => reachedPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) reachedPoint = true;
    }

    public void tutorialFinished() => this.gameObject.SetActive(false);
}
