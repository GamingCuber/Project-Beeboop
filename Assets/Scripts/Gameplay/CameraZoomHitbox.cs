using UnityEngine;

public class CameraZoomHitbox : MonoBehaviour
{
    public float zoomSize;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CameraZoomManager.Instance.doZoom(zoomSize);
        }
    }
}
