using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreenManager : MonoBehaviour
{
    public void backButton()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
