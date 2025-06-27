using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public static RespawnScript Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null)
        {

            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void respawntocheckpoint()
    {
        transform.position = PlayerDataManager.Instance.getData().current_checkpoint.gameObject.transform.position;



    }
}
