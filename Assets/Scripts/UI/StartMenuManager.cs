using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("Movement Playtest");
    }
}
