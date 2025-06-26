using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager: MonoBehaviour
{
    Dictionary<string, AudioClip> sfx_dictionary = new Dictionary<string, AudioClip>();
    public static SoundManager Instance;
    public AudioSource ourAudioSource;
    public AudioClip dash;
    public AudioClip fall;
    public AudioClip jump;
    public AudioClip Walk;



    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }


        sfx_dictionary.Add("dash", dash);
        sfx_dictionary.Add("fall", fall);
        sfx_dictionary.Add("Jump", jump);
    
    }

   
    public void playsound(string key){

        ourAudioSource.PlayOneShot(sfx_dictionary[key]);

    }
}