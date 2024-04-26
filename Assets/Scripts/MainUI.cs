using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class MainUI : MonoBehaviour
{
    public void fightClick()
    {
        
        SceneManager.LoadScene("MainGame");
    }
    public void quitClick()
    {
        
        Application.Quit();
    }
    public void menuClick()
    {
       
        SceneManager.LoadScene("MainMenu");
    }

}
