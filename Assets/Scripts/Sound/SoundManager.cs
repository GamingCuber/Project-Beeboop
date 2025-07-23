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
    public AudioClip crtOn;
    public AudioClip crtAmbience;
    public AudioClip crtOff;
    public AudioClip crtClick;

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
        sfx_dictionary.Add("crtOn", crtOn);
        sfx_dictionary.Add("crtAmbience", crtAmbience);
        sfx_dictionary.Add("crtOff", crtOff);
        sfx_dictionary.Add("crtClick", crtClick);

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

    public void playLoopedSound(string key, Vector3 soundPos, float minDist, float maxDist, float volume, out int index)
    {
        GameObject curSound = getAvailSound(out int i);
        index = i;
        curSound.SetActive(true);
        AudioSource source = curSound.GetComponent<AudioSource>();
        source.clip = sfx_dictionary[key];
        curSound.transform.position = soundPos;
        source.minDistance = minDist;
        source.maxDistance = maxDist;
        source.volume = volume;
        source.loop = true;
        source.Play();
    }

    public void stopLoopSound(int i)
    {
        GameObject sound = audioSources[i];
        AudioSource source = sound.GetComponent<AudioSource>();

        source.loop = false;
        sound.SetActive(false);
    }

    private IEnumerator turnOffSound(GameObject obj, float time)
    {
        float timer = 0;

        float startTime = Time.realtimeSinceStartup;

        while (timer < time)
        {
            Debug.Log(timer);
            timer = Time.realtimeSinceStartup - startTime;
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

    private GameObject getAvailSound(out int index)
    {
        int i = 0;
        foreach (GameObject g in audioSources)
        {
            if (!g.activeInHierarchy)
            {
                index = i;
                return g;
            }
            i++;
        }
        index = -1;
        return null;
    }
}