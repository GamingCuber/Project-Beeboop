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
    public bool isStopped = false;
    public float stoptime = 1;
    public float offsetTime;

    private GameObject[] hitboxes;
    private bool isKilling = false;

    void Start()
    {
        currentposy = crusherPart.transform.localPosition;
        goalpos = currentposy + new Vector3(0f, 0.75f, 0f);
        Invoke(nameof(startCrusher), offsetTime);

        hitboxes = new GameObject[crusherPart.transform.childCount];
        for(int i = 0; i < hitboxes.Length; i++)
        {
            hitboxes[i] = crusherPart.transform.GetChild(i).gameObject;
        }
    }


    private void startCrusher()
    {
        StartCoroutine(moveCrusher());
    }
    IEnumerator moveCrusher()
    {
        float timer = 0;

        bool isPlayingUp = false;
        int loopInt = -1;

        while (true)
        {
            timer += Time.deltaTime;

            if (!isDown && isStopped == false)
            {
                if (!isPlayingUp)
                {
                    isPlayingUp = true;
                    SoundManager.Instance.playLoopedSound("crusherUp", transform.position, 0, 25, 1, false, out int i);
                    loopInt = i;
                }

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
                if (!isKilling)
                {
                    isPlayingUp = false;
                    setKill(true);
                    SoundManager.Instance.playSoundFX("crusherDown", hitboxes[1].transform.position, 0, 30, 1, false);
                }

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
                if (isKilling)
                {
                    setKill(false);
                }

                if (loopInt != -1)
                {
                    SoundManager.Instance.stopLoopSound(loopInt);
                    loopInt = -1;
                }

                if (timer > stoptime)
                {
                    isStopped = false;
                    timer = 0;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
    
    private void setKill(bool kill) //true to set tags to kill player, false to not
    {
        if (kill)
        {
            isKilling = true;
        }
        else
        {
            isKilling = false;
        }
    
        for (int i = 0; i < hitboxes.Length; i++)
        {
            hitboxes[i].GetComponent<BoxCollider2D>().enabled = kill;
        }
    }
}