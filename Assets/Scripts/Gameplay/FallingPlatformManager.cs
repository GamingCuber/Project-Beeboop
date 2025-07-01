using UnityEngine;

public class FallingPlatformManager : MonoBehaviour
{
    public BoxCollider2D platformBox;
    [SerializeField]
    private float secondsUntilDissolve;
    [SerializeField]
    private float secondsUntilReappearance;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.transform.position.y > transform.position.y)
                Invoke(nameof(dissolvePlatform), secondsUntilDissolve);
        }
    }

    private void dissolvePlatform()
    {
        platformBox.enabled = false;
        Invoke(nameof(reenablePlatform), secondsUntilReappearance);
    }
    private void reenablePlatform()
    {
        platformBox.enabled = true;
    }

}
