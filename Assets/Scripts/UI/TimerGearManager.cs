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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            gears[0].GetComponent<TimerGear>().vibrateGear();
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
