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
    }

    public string levelName;

    public Sprite levelCover;

    public float levelTotalTime;

    public Scene[] scenes;
}
