using UnityEngine;
using System.Collections.Generic;

public class TimerGearManager : MonoBehaviour
{
    public static TimerGearManager Instance;

    public List<GameObject> gears;

    public float timeWhenStop; //in seconds

    private float speedMult = 1;

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

    public void setMult(float m)
    {
        speedMult = m;
    }

    public void resetMult()
    {
        speedMult = 1f;
    }

    public void reverseGears()
    {
        if (speedMult > 0)
        {
            speedMult *= -1;
        }
        else
        {
            Mathf.Abs(speedMult);
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
                    tg.setSpeed(Mathf.Lerp(tg.maxSpeed, tg.minSpeed, percentTime) * speedMult);
                }
            }
        }
    }
}
