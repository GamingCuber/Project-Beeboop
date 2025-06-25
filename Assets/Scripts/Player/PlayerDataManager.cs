using UnityEngine;

public class PlayerDataManager : MonoBehaviour //this is so theres one central playerdata that all the movement scripts can reference and change
{
    public static PlayerDataManager Instance;

    public PlayerData data; //public so we can easily just plop it in in the editor, probably shouldn't mess with it directly tho

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public PlayerData getData()
    {
        return data;
    }
}
