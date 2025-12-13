using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

public class LeaderboardDataManager : MonoBehaviour
{
    public static LeaderboardDataManager Instance;

    [Serializable]
    public struct Score
    {
        public string name;
        public float time;

        public Score(string n, float t)
        {
            name = n;
            time = t;
        }
    }

    [Serializable]
    public class scoreWrapper
    {
        public List<levelScoreData> levelData = new List<levelScoreData>();
    }

    [Serializable]
    public class oldScoreWrapper
    {
        public List<Score> scores = new List<Score>();
    }

    [Serializable]
    public class levelScoreData
    {
        public string levelName;
        public List<Score> scores = new List<Score>();
    }

    public levelScoreData curLevelData; 

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        scoreWrapper curWrap = getJSON();

        for (int i = 0; i < curWrap.levelData.Count; i++)
        {
            if (curWrap.levelData[i].levelName == GameDataManager.Instance.curLevel.levelName)
            {
                curLevelData = curWrap.levelData[i];
                break;
            }
        }

        //means its a new level data entry (doesnt have a level data in our json file alrdy)
        if (curLevelData.scores.Count == 0)
        {
            var jsonFile = File.ReadAllText(Application.persistentDataPath + "/scoredata.json");

            //this bit is just so we can maintain the scores we had before I changed how the json works
            if (jsonFile.Contains("scores") && !jsonFile.Contains("levelData"))
            {
                var oldSavedScores = JsonUtility.FromJson<oldScoreWrapper>(jsonFile);
                curLevelData.levelName = GameDataManager.Instance.curLevel.name;
                curLevelData.scores = oldSavedScores.scores;
            }
            else
            {
                curLevelData.levelName = GameDataManager.Instance.curLevel.levelName;
                curLevelData.scores = new List<Score>();
            }
        }
    }

    public scoreWrapper getJSON()
    {
        var json = readJSON();
        if (json == null)
        {
            json = new scoreWrapper();
        }
        return json;
    }

    public void addScoreToJSON(string name, float score)
    {
        scoreWrapper curWrap = readJSON();

        //if the json file is empty, itll error out bc curwrap is null, so we just make a new one
        if (curWrap == null)
        {
            curWrap = new scoreWrapper();
        }

        curLevelData.scores.Add(new Score(name, score));

        for (int i = 0; i < curWrap.levelData.Count; ++i)
        {
            if (curWrap.levelData[i].levelName == GameDataManager.Instance.curLevel.levelName)
            {
                curWrap.levelData[i] = curLevelData;
                updateJSON(curWrap);
            }
            else if (i == curWrap.levelData.Count - 1)
            {
                curWrap.levelData.Add(curLevelData);
                updateJSON(curWrap);
            }
        }
    }

    private scoreWrapper readJSON()
    {
        if (!File.Exists(Application.persistentDataPath + "/scoredata.json")) //make a new json file if one dont exist
        {
            File.WriteAllText(Application.persistentDataPath + "/scoredata.json", "{}");
        }

        var json = File.ReadAllText(Application.persistentDataPath + "/scoredata.json");

        return (JsonUtility.FromJson<scoreWrapper>(json));
    }

    private void updateJSON(scoreWrapper newWrap)
    {
        var json = JsonUtility.ToJson(newWrap, true);
        File.WriteAllText(Application.persistentDataPath + "/scoredata.json", json);
    }

    public levelScoreData getCurData()
    {
        return curLevelData;
    }
}