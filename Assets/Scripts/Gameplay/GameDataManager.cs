using UnityEngine;
using System.Collections.Generic;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    private Dictionary<string, float> data = new Dictionary<string, float>();

    public float totalTime;

    public LevelData[] levels;

    public LevelData curLevel;

    private int levelNum = 0;

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

        data["Time"] = totalTime;
        data["Deaths"] = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            nextLevel();
        }
    }

    //param is just level name of the scriptableobject
    public void setLevelData(string sceneName)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            LevelData curData = levels[i];

            if (curData.levelName == sceneName)
            {
                curLevel = curData;
            }
        }

        setTotalTime();
        resetData();
    }

    public float getTimeLeft()
    {
        return data["Time"];
    }

    public float getTotalTime()
    {
        return totalTime;
    }

    public void setTotalTime()
    {
        totalTime = curLevel.levelTotalTime;
    }

    public void updateTime(float time)
    {
        data["Time"] = time;
    }

    //this is called from the leveltransitionmanager, itll auto go to the next level in the sequence
    public void nextLevel()
    {
        levelNum++;

        if (levelNum < curLevel.scenes.Length)
        {
            LevelTransition.Instance.doTransition(curLevel.scenes[levelNum].sceneName);
            MusicManager.Instance.transitionSong(curLevel.scenes[levelNum].sceneSong);
        }
        else
        {
            LevelTransition.Instance.resetDeath();
            LevelTransition.Instance.doTransition("WinMenu");
            MusicManager.Instance.transitionSong("Win");
        }
    }
    public void resetData()
    {
        data["Time"] = totalTime;
        data["Deaths"] = 0;
        PlayerStateManager.Instance.getState().totalTime = 0f;
        PlayerStateManager.Instance.getState().deathNumber = 0;
        levelNum = 0;
    }
}
