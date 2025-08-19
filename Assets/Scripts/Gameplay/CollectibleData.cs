using UnityEngine;

public class CollectibleData : MonoBehaviour
{
    public enum UpgradeOptions
    {
        None,
        Dash,
        Hook,
        DoubleJump,
        Time
    }
    public UpgradeOptions upgrade;

    [Tooltip("ONLY APPLICABLE IF YOU CHOOSE TIME UPGRADE")]
    public float time;

}
