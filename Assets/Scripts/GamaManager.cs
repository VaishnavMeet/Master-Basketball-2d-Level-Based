using UnityEngine;
using UnityEngine.SceneManagement;

public class GamaManager : MonoBehaviour
{

    public GameObject settingsBox;
    public GameObject WinMenu;
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
        
    }
    public void displayWinMenu()
    {
        WinMenu.SetActive(true);
    }

}
