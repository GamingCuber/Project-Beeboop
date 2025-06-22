using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody2D rb;

    void Update()
    {
        if (Input.GetKey(PlayerInputs.Instance.right)){
            rb.AddForceX(10, ForceMode2D.Force);

        }
        
        else if (Input.GetKey(PlayerInputs.Instance.left))
        {
            rb.AddForceX(-10, ForceMode2D.Force);

        }
        else
        {
            Vector2 velo = rb.linearVelocity; // G
            velo.x = 0;
            rb.linearVelocity = velo;
        }
    }
}

