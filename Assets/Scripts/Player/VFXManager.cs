using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;

    public int poolAmt;

    public GameObject VFXPre;
    private GameObject[] VFXObjects;

    public AnimationClip dj;
    public AnimationClip turn;
    private Dictionary<string, AnimationClip> clips = new Dictionary<string, AnimationClip>();

    private GameObject player;

    private Coroutine moveVFXCo;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        VFXObjects = new GameObject[poolAmt];

        for (int i = 0; i < poolAmt; i++)
        {
            GameObject newVFX = Instantiate(VFXPre, this.transform);
            VFXObjects[i] = newVFX;
            newVFX.SetActive(false);
        }

        clips.Add("DoubleJump", dj);
        clips.Add("Turn", turn);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void playVFX(string type)
    {
        StartCoroutine(playVFXCo(type));
    }

    public GameObject getVFX()
    {
        GameObject returnVFX = null;
        for (int i = 0; i < poolAmt; i++)
        {
            if (!VFXObjects[i].activeInHierarchy)
            {
                returnVFX = VFXObjects[i];
            }
        }
        return returnVFX;
    }

    private IEnumerator playVFXCo(string type)
    {
        GameObject vfxGO = getVFX();
        Animator anim = vfxGO.GetComponent<Animator>();

        if (PlayerDataManager.Instance.getData().playerDirection == "left")
        {
            vfxGO.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        }
        else
        {
            vfxGO.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }

        vfxGO.transform.position = player.transform.position + Vector3.down * 4/5;

        vfxGO.SetActive(true);
        anim.SetTrigger(type);

        yield return new WaitForSecondsRealtime(clips[type].length);

        vfxGO.SetActive(false);
    }
}