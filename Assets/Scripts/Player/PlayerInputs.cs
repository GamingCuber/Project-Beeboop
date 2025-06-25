using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs Instance;

    public KeyCode left { get; set; }
    public KeyCode right { get; set; }
    public KeyCode jump { get; set; }
    public KeyCode hook { get; set; }

    public KeyCode dash { get; set; }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("left", "A"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("right", "D"));
        jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jump", "Space"));
        dash = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("dash", "E"));
        hook = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("hook", "F"));
    }
}
