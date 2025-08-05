using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class Crusher : MonoBehaviour
{
    public GameObject crusherPart;
    Vector3 currentposy;
    Vector3 goalpos;
    public float upTime = 5;
    public float downTime = 5;
   public bool isDown = false;
     public   bool isStopped = false;
    public float stoptime = 1;
    void Start()
    {
        currentposy = crusherPart.transform.localPosition;
        goalpos = currentposy + new Vector3(0f, 0.75f, 0f);
        StartCoroutine(moveCrusher());
    }

    // Start is 
    // called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator moveCrusher()
    {
        float timer = 0;
        

        while (true)
        {
            timer += Time.deltaTime;

            if (!isDown && isStopped == false)
            {
                float newy = Mathf.Lerp(currentposy.y, goalpos.y, timer / upTime);

                Vector3 newPosition = crusherPart.transform.localPosition;
                newPosition.y = newy;

                crusherPart.transform.localPosition = newPosition;

                if (timer > upTime)
                {
                    timer = 0;
                    isDown = true;
                    isStopped = true;   
                }

            }
            else if (isDown && isStopped == false)
            {

                float originy = Mathf.Lerp(goalpos.y, currentposy.y, timer / downTime);
                Vector3 rar = crusherPart.transform.localPosition;
                rar.y = originy;

                crusherPart.transform.localPosition = rar;

                if (timer > downTime)
                {
                    isDown = false;
                    timer = 0;
                    isStopped = true;
                }
            }
            if (isStopped)
            {
                if (timer > stoptime)
                {
                    isStopped = false;
                    timer = 0;
                }
            }




            yield return new WaitForEndOfFrame();
        }

    }
}

   

    // Update is called once per frame
  