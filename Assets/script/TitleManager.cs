using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void StartGame()
    {
        Time.timeScale = 1f; // ”O‚Ì‚½‚ß’â~‰ğœ
        SceneManager.LoadScene("GameScene");
    }

    public void OpenSettings()
    {
        Debug.Log("İ’è‚ğŠJ‚­");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("ƒQ[ƒ€I—¹");
    }
}