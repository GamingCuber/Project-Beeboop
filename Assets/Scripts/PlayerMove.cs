using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody2D rb;
    public string Direction;
    void Update()
    {
        if (Input.GetKey(PlayerInputs.Instance.right)){ // Moving Right
            rb.AddForceX(PlayerDataManager.Instance.getData().playerAcc, ForceMode2D.Force);
            PlayerDataManager.Instance.getData().playerdirection = "right";
        }
        
        else if (Input.GetKey(PlayerInputs.Instance.left))
        {
            rb.AddForceX(-PlayerDataManager.Instance.getData().playerAcc, ForceMode2D.Force);
             PlayerDataManager.Instance.getData().playerdirection = "left";// Player is moving left
        }
        else
        {

            Vector2 velo = rb.linearVelocity; // G
            if (PlayerDataManager.Instance.getData().isDashing == false)
            {
                 velo.x = 0; // if player isn't dashing you are able to set the velocity to 0  
            }
          
            rb.linearVelocity = velo;
        }
    }
}

