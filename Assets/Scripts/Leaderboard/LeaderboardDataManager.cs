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
        public List<Score> scores = new List<Score>();
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
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

        curWrap.scores.Add(new Score(name, score));

        updateJSON(curWrap);
    }

    private scoreWrapper readJSON()
    {
        if (!File.Exists(Application.persistentDataPath + "/scoredata.json")) //make a new json file if one dont exist
        {
            Debug.Log("making new json");
            File.WriteAllText(Application.persistentDataPath + "/scoredata.json", "{}");
        }

        var json = File.ReadAllText(Application.persistentDataPath + "/scoredata.json");

        return (JsonUtility.FromJson<scoreWrapper>(json));
    }

    private void updateJSON(scoreWrapper newWrap)
    {
        var json = JsonUtility.ToJson(newWrap);
        File.WriteAllText(Application.persistentDataPath + "/scoredata.json", json);
    }
}