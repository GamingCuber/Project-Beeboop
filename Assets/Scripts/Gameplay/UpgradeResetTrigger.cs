using UnityEngine;

public class UpgradeResetTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.resetState();
            this.gameObject.SetActive(false);
        }
    }
}
