using System;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
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

    private void Start()
    {
        foreach (Song s in musicList)
        {
            musicDictionary.Add(s.name, s.audioClip);
        }
        playMusic("LevelMusic");
    }


    public void playMusic(string songName)
    {
        currentMusic = musicDictionary[songName];
        musicPlayer.PlayOneShot(currentMusic);
    }

}
