using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class CheckPointScript : MonoBehaviour
{
    private Dictionary<GameObject, bool> checkpointsHit = new Dictionary<GameObject, bool>();

    public Sprite on;

    public Sprite off;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            if (!checkpointsHit.ContainsKey(collision.gameObject))
            {
                checkpointsHit[collision.gameObject] = true;
                SoundManager.Instance.playSoundFX("checkpoint", collision.transform.position, 0, 15, 0.15f, false);
            }

            PlayerDataManager.Instance.getData().currentCheckpoint = collision.gameObject;

            SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();

            sr.sprite = on;

            collision.transform.GetChild(0).GetComponent<Light2D>().color = new Color32((byte)60, (byte)99, (byte)0, (byte)150);
        }
    }
}
