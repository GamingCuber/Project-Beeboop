using UnityEngine;

public class PlayerGravManager : MonoBehaviour //central script to mess with player gravity in case we need to change gravity in scripts outside of jump
{
    public static PlayerGravManager Instance;

    public Rigidbody2D rb;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (PlayerStateManager.Instance.getState().isFalling && !PlayerStateManager.Instance.getState().isGrounded)
        {
            setGrav(PlayerDataManager.Instance.getData().jumpFallGrav);
        }
        else if (PlayerStateManager.Instance.getState().isGrounded)
        {
            resetGrav();
        }
    }

    public void setGrav(float g)
    {
        rb.gravityScale = g;
    }

    public void resetGrav()
    {
        rb.gravityScale = 1;
    }
}