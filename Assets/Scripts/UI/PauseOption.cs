using UnityEngine;

[CreateAssetMenu(fileName = "pauseOptions", menuName = "pauseOptions", order = 2)]

public class PauseOption : ScriptableObject
{
    public string optionName;
    public string description;
    public string channelNumber;
    public Sprite backgroundImg;
}
