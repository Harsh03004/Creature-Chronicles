using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class MainUI : MonoBehaviour
{
    public void fightClick()
    {
        Debug.Log("Click");
        SceneManager.LoadScene(1);
    }
    public void quitClick()
    {
        Application.Quit();
    }

}