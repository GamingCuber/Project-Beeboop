using UnityEngine;
using System.Collections.Generic;

public class TimerGearManager : MonoBehaviour
{
    public static TimerGearManager Instance;

    public List<GameObject> gears;

    public float timeWhenStop;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void setGearSpeed(float curTime, float totalTime)
    {
        float percentTime = (totalTime - curTime - timeWhenStop) / (totalTime - timeWhenStop);

        foreach (GameObject g in gears)
        {
            TimerGear tg = g.GetComponent<TimerGear>();

            tg.setSpeed(Mathf.Lerp(tg.maxSpeed, tg.minSpeed, percentTime));
        }
    }
}
