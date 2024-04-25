using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class MainUI : MonoBehaviour
{
    public AudioSource btnClick;
    public void fightClick()
    {
        btnClick.Play();
        SceneManager.LoadScene(1);
    }
    public void quitClick()
    {
        btnClick.Play();
        Application.Quit();
    }
    public void menuClick()
    {
        btnClick.Play();
        SceneManager.LoadScene(0);
    }

}
