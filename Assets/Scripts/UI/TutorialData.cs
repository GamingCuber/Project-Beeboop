using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "TutorialData", menuName = "TutorialData")]
public class TutorialData : ScriptableObject
{
    public string topText;
    public string descriptionText;
    public VideoClip tutorialVideo;
    public Sprite inputIcon;
    public string inputName;
}
