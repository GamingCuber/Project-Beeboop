using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public static RespawnScript Instance;
    void Start()
    {
        if (Instance == null)
        {

            Instance = this;
        }
    }

    public void respawntocheckpoint()
    {
        transform.position = PlayerDataManager.Instance.getData().currentCheckpoint.gameObject.transform.position;



    }
}
