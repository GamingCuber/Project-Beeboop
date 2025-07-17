using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class Crusher : MonoBehaviour
{
    int increaseamount = 1;
    Vector3 currentposy;
    Vector3 goalpos;
    float increment = 10;
    public float upTime = 5;
    public float downTime = 5;
   public bool isDown = false;
     public   bool isStopped = false;
    float stoptime = 1;
    void Start()
    {
        currentposy = this.gameObject.transform.position;
        goalpos = currentposy - new Vector3(0f, -increment, 0f);
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
                float newy = Mathf.Lerp(currentposy.y, goalpos.y, timer / downTime);

                Vector3 newPosition = this.gameObject.transform.position;
                newPosition.y = newy;

                transform.position = newPosition;

                if (timer > downTime)
                {
                    timer = 0;
                    isDown = true;
                    yield return new WaitForSecondsRealtime(stoptime);



                }

                else if(isDown && isStopped == false)
                {

                    float originy = Mathf.Lerp(goalpos.y, currentposy.y, timer / upTime);
                    Vector3 rar = this.gameObject.transform.position;
                    rar.y = originy;

                    transform.position = rar;

                    if (timer > upTime)
                    {
                        isDown = false;
                        timer = 0;
                        isStopped = true;
                       

                    }

                    if (isStopped)
                    {
                        if (timer > stoptime)
                        {
                            isStopped = false;
                            timer = 0;


                         }


                    }

                }
            }





            yield return new WaitForEndOfFrame();
        }

    }
}

   

    // Update is called once per frame
  