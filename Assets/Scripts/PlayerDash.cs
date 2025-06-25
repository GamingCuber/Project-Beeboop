using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public int CoolDown = 1;
    public Rigidbody2D rb;
    
    public bool debounce = false;
    public bool DashingVar = PlayerDataManager.Instance.getData().isDashing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(PlayerInputs.Instance.dash) && debounce == false)
        {
            StartCoroutine(DashCoroutine());

        }



    }


    public IEnumerator DashCoroutine()
    {




        debounce = true;
        if (PlayerDataManager.Instance.getData().playerdirection == "right")
        {
            DashingVar = true;
            //rb.linearVelocityX = 0;
            rb.AddForceX(PlayerDataManager.Instance.getData().dashStr , ForceMode2D.Impulse);
        }
        else
        {
            DashingVar = true;
            //rb.linearVelocityX = 0;
            rb.AddForceX(-1 * PlayerDataManager.Instance.getData().dashStr, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(CoolDown);
        DashingVar = false;
        debounce = false;




    }
}

