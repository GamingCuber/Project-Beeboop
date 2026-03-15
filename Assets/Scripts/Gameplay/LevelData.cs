using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData", order = 2)]


public class LevelData : ScriptableObject
{
    [Serializable]
    public struct Scene
    {
        public string sceneName;
        public string sceneSong;

        public float sceneTime;
    }

    public string levelName;

    public Sprite levelCover;

    public float levelTotalTime;

    public Scene[] scenes;

    [Tooltip("Values in the list 0-6 means times needed for V-S-A-B-C-D")]
    public float[] secondsPerRank = new float[6];

    [Tooltip("First is for +, Second is for blank, Third is assumed to be anything above")]
    public int[] deathsPerSign = new int[2];
}
