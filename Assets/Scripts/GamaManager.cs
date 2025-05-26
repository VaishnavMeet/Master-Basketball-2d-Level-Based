using UnityEngine;
using UnityEngine.SceneManagement;

public class GamaManager : MonoBehaviour
{

    public GameObject settingsBox;
    public GameObject WinMenu;
    public GameObject LoseMenu;
    public LoadLevel levelLoader;
    public void onClickSettings()
    {
        Debug.Log("Clicked");
        settingsBox.SetActive(true);
    }
    public void onClickDone()
    {
        settingsBox.SetActive(false);
    }
    public void onClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void displayWinMenu()
    {
        WinMenu.SetActive(true);
        LoseMenu.SetActive(false);
    }
    public void onClickMenu()
    {
        LoseMenu.SetActive(false);
        WinMenu.SetActive(false);    
        SceneManager.LoadScene("Levels");

    }
    public void OnNextClick()
    {
        WinMenu.SetActive(false);
        LoseMenu.SetActive(false);

        if (levelLoader != null && levelLoader.levelData != null && levelLoader.levelData.nextLevel != null)
        {
            LevelManager.currentLevel = levelLoader.levelData.nextLevel;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    public void displayLoseMenu()
    {
        LoseMenu.SetActive(true);
    }

}
