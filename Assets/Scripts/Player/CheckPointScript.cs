using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    public Sprite on;

    public Sprite off;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            PlayerDataManager.Instance.getData().currentCheckpoint = collision.gameObject;

            SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();

            sr.sprite = on;
        }
    }
}
