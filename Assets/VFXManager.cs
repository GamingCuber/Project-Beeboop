using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;
    public GameObject VFXPre;
    private GameObject[] VFXObjects = new GameObject[5];

    public AnimationClip dJ;
    public AnimationClip turn;


    private void Start()
    {
        if (Instance == null) {
            Instance = this;
        }

        VFXObjects = new GameObject[5];

        for (int i = 0; i < 5; i++)
        {
            GameObject newVFX = Instantiate(VFXPre, this.transform);
            VFXObjects[i] = newVFX;
            newVFX.SetActive(false);
        }
    }

    //publiuc function returns void, turns first available on, sets the clip to dj, plays, turn pff

    public void playVFX (string type)
    {
        GameObject geeop = getVFX();
        geeop.SetActive(true);
    }

    public GameObject getVFX()
    {
        GameObject returnVFX = null;
        for (int i = 0; i < 5; i++)
        {
            if (!VFXObjects[i].activeInHierarchy)
            {
                returnVFX = VFXObjects[i];
            }
        }
        return returnVFX;
    }

    //public GameObject getVFX()
    //{
    //    foreach(GameObject g in VFXObjects)
    //    {
    //        if (!g.activeInHierarchy)
    //        {
    //            return g;
    //        }
    //    }
    //    return null;
    //}

}