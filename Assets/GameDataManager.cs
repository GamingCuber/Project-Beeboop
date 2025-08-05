using UnityEngine;
using System.Collections.Generic;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    private Dictionary<string, float> data = new Dictionary<string, float>();

    public float totalTime;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        data["Time"] = totalTime;
        data["Deaths"] = 0;
    }

    public float getTime()
    {
        return data["Time"];
    }

    public void updateTime(float time)
    {
        data["Time"] = time;
        Debug.Log(data["Time"]);
    }
}
