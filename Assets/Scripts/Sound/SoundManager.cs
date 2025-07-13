using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager: MonoBehaviour
{
    Dictionary<string, AudioClip> sfx_dictionary = new Dictionary<string, AudioClip>();
    public static SoundManager Instance;

    public AudioSource ourAudioSource;

    private GameObject[] audioSources;

    public GameObject sourcePre;

    public int soundPoolAmt;

    public AudioClip dash;
    public AudioClip fall;
    public AudioClip jump;
    public AudioClip Walk;
    public AudioClip test;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        sfx_dictionary.Add("dash", dash);
        sfx_dictionary.Add("fall", fall);
        sfx_dictionary.Add("Jump", jump);
        sfx_dictionary.Add("Test", test);

        audioSources = new GameObject[soundPoolAmt];

        for (int i = 0; i < soundPoolAmt; i++)
        {
            GameObject newSource = Instantiate(sourcePre, this.transform);
            newSource.SetActive(false);
            audioSources[i] = newSource;
        }
    }
   
    public void playPlayerSound(string key)
    {
        ourAudioSource.PlayOneShot(sfx_dictionary[key]);
    }

    public void playSoundFX(string key, Vector3 soundPos, float minDist, float maxDist, float volume)
    {
        GameObject curSound = getAvailSound();
        curSound.SetActive(true);
        AudioSource source = curSound.GetComponent<AudioSource>();
        source.clip = sfx_dictionary[key];
        curSound.transform.position = soundPos;
        source.minDistance = minDist;
        source.maxDistance = maxDist;
        source.volume = volume;
        source.Play();
        StartCoroutine(turnOffSound(curSound, source.clip.length));
    }

    private IEnumerator turnOffSound(GameObject obj, float time)
    {
        float timer = 0;

        while (timer < time)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        obj.SetActive(false);
        yield break;
    }

    private GameObject getAvailSound()
    {
        foreach(GameObject g in audioSources)
        {
            if (!g.activeInHierarchy)
            {
                return g;
            }
        }
        return null;
    }
}