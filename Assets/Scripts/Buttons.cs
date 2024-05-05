using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    public GameObject StartScreen;
   
    public void StartGame()
    {
        Debug.Log("StartGame() method called.");
        Time.timeScale = 1f;
        StartScreen.SetActive(false);

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
