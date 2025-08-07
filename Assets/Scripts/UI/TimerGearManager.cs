using UnityEngine;
using System.Collections.Generic;

public class TimerGearManager : MonoBehaviour
{
    public static TimerGearManager Instance;

    public List<GameObject> gears;

    public float timeWhenStop; //in seconds

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void vibrateGears()
    {
        for(int i = 0; i < gears.Count; i++)
        {
            gears[i].GetComponent<TimerGear>().vibrateGear();
        }
    }

    public void setGearSpeed(float curTime, float totalTime)
    {
        float offsetTime = totalTime - timeWhenStop;
        float offsetCur = curTime - timeWhenStop;


        float percentTime = (offsetTime - offsetCur) / (offsetTime);


        if (percentTime <= 1)
        {
            foreach (GameObject g in gears)
            {
                TimerGear tg = g.GetComponent<TimerGear>();
                
                if (!tg.paused)
                {
                    tg.setSpeed(Mathf.Lerp(tg.maxSpeed, tg.minSpeed, percentTime));
                }

            }
        }
    }
}
