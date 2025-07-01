using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    public void backButton()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
