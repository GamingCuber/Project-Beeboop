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

    private IEnumerator fadeOutCo()
    {
        float timer = 0;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;

            float volume = Mathf.Lerp(maxVolume, 0, timer / transitionTime);

            musicPlayer.volume = volume;

            yield return new WaitForEndOfFrame();
        }

        yield break;
    }

    private IEnumerator fadeInCo()
    {
        Debug.Log(maxVolume);

        float timer = 0;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;

            float volume = Mathf.Lerp(0, maxVolume, timer / transitionTime);

            musicPlayer.volume = volume;

            yield return new WaitForEndOfFrame();
        }

        yield break;
    }

    private void calculateMaxVolume()
    {
        maxVolume = 0.35f * (float)PlayerPrefs.GetInt("MasterVolume", 100)/100 * (float)PlayerPrefs.GetInt("MusicVolume", 100)/100;
    }
}
