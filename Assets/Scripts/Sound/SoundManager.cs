using System;
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

    [Serializable]
    public struct SoundClip
    {
        public string name;
        public AudioClip clip;
    }

    public SoundClip[] sounds;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        for (int i = 0; i < sounds.Length; i++)
        {
            sfx_dictionary[sounds[i].name] = sounds[i].clip;
        }

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

    public void playSoundFX(string key, Vector3 soundPos, float minDist, float maxDist, float volume, bool mono)
    {
        GameObject curSound = getAvailSound();
        curSound.SetActive(true);
        AudioSource source = curSound.GetComponent<AudioSource>();
        source.clip = sfx_dictionary[key];
        curSound.transform.position = soundPos;
        source.minDistance = minDist;
        source.maxDistance = maxDist;
        source.volume = volume;

        if (mono)
        {
            source.spatialBlend = 0;
        }
        else
        {
            source.spatialBlend = 1;
        }

        source.Play();
        StartCoroutine(turnOffSound(curSound, source.clip.length));
    }

    public void playLoopedSound(string key, Vector3 soundPos, float minDist, float maxDist, float volume, bool mono, out int index)
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

        if (mono)
        {
            source.spatialBlend = 0;
        }
        else
        {
            source.spatialBlend = 1;
        }

        source.Play();
    }

    public void stopLoopSound(int i)
    {
        GameObject sound = audioSources[i];
        AudioSource source = sound.GetComponent<AudioSource>();

        source.loop = false;
        sound.SetActive(false);
    }

    public void setLoopedVolume(int i, float volume)
    {
        Debug.Log(volume);
        audioSources[i].GetComponent<AudioSource>().volume = volume;
    }

    private IEnumerator turnOffSound(GameObject obj, float time)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;

        float startTime = Time.realtimeSinceStartup;

        while (timer < time)
        {
            timer = Time.realtimeSinceStartup - startTime;
            yield return wait;
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
