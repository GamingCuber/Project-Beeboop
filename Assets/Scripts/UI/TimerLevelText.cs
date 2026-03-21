using UnityEngine;

using TMPro;

public class TimerLevelText : MonoBehaviour
{
    private LevelData data;

    private TMP_Text levelNameText;

    private TMP_Text deltaTimeText;

    private TMP_Text levelTimeText;

    public void Start()
    {
        //hard coding it to be based off children bc im lazy

        levelNameText = this.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        levelTimeText = this.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        deltaTimeText = this.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>();
    }

    public void setData(LevelData data)
    {
        this.data = data;
    }

    
}
