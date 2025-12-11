using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField]
    private AudioSource musicPlayer;
    private Dictionary<string, AudioClip> musicDictionary = new Dictionary<string, AudioClip>();
    [Serializable]

    public struct Song
    {
        public string name;
        public AudioClip audioClip;
    }
    public Song[] musicList;

    private AudioClip currentMusic;

    public float transitionTime; //how long it takes to fade the song out/new one in

    private float maxVolume;

    private int factoryAmbIndex = -1; //for manipulating the background ambience

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        foreach (Song s in musicList)
        {
            musicDictionary.Add(s.name, s.audioClip);
        }

        calculateMaxVolume();
    }

    public void volumeUpdated()
    {
        calculateMaxVolume();
        musicPlayer.volume = maxVolume;

        if (factoryAmbIndex != -1)
        {
            SoundManager.Instance.setLoopedVolume(factoryAmbIndex, maxVolume * 2f);

        }
    }

    public void playMusic(string songName)
    {
        currentMusic = musicDictionary[songName];
        musicPlayer.clip = currentMusic;
        musicPlayer.Play();
    }

    public void fadeOut()
    {
        StartCoroutine(fadeOutCo());
    }

    public void fadeIn()
    {
        StartCoroutine(fadeInCo());
    }

    public void transitionSong(string song)
    {
        StartCoroutine(transitionSongCo(song));
    }

    private IEnumerator transitionSongCo(string song)
    {
        StartCoroutine(fadeOutCo());
        yield return new WaitForSecondsRealtime(transitionTime);
        playMusic(song);
        StartCoroutine(fadeInCo());
        yield break;
    }

    //for pause menu
    public void resumeSong()
    {
        musicPlayer.Play();
        playBackgroundAmbience();
    }

    public void pauseSong()
    {
        musicPlayer.Pause();
        stopBackgroundAmbience();
    }

    public void playBackgroundAmbience()
    {
        StartCoroutine(waitForSoundManager());
    }

    public void stopBackgroundAmbience()
    {
        if (factoryAmbIndex != -1)
        {
            SoundManager.Instance.stopLoopSound(factoryAmbIndex);
        }
        factoryAmbIndex = -1;
    }

    private IEnumerator fadeOutCo()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;

            float volume = Mathf.Lerp(maxVolume, 0, timer / transitionTime);

            musicPlayer.volume = volume;

            if (factoryAmbIndex != -1)
            {
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.setLoopedVolume(factoryAmbIndex, volume * 2f);   
                }
            }

            yield return wait;
        }

        yield break;
    }

    private IEnumerator fadeInCo()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        float timer = 0;

        if (SoundManager.Instance != null)
        {
            playBackgroundAmbience();
        }

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;

            float volume = Mathf.Lerp(0, maxVolume, timer / transitionTime);

            if (musicPlayer == null)
            {
                yield break;
            }

            musicPlayer.volume = volume;

            yield return wait;
        }

        yield break;
    }

    private IEnumerator waitForSoundManager()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while (SoundManager.Instance == null)
        {
            yield return wait;
        }

        //had an issue where volume was 0 b/c of the lerp in fadeout (i think), so just stall it out, unnoticeable anyway
        yield return new WaitForSecondsRealtime(0.1f);

        SoundManager.Instance.playLoopedSound("factoryAmbience", Vector3.zero, 0, 300, maxVolume * 2f, true, out int i);
        factoryAmbIndex = i;
    }

    private void calculateMaxVolume()
    {
        maxVolume = 0.35f * (float)PlayerPrefs.GetInt("MasterVolume", 100)/100 * (float)PlayerPrefs.GetInt("MusicVolume", 100)/100;
    }
}
