using UnityEngine;
using UnityEngine.UI;

public class BSideOption : MonoBehaviour
{
    public LevelData optionData;

    public void startLevel()
    {
        StartMenuManager.Instance.startBSide(optionData.levelName);
    }

    public void setupButton()
    {
        transform.GetChild(1).GetComponent<Button>().onClick.AddListener(startLevel);
    }
}
