using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Menu sections
    public GameObject mainMenu;
    public GameObject loadingScreen;
    public GameObject quizForbbidenScreen;
    public GameObject gameForbbidenScreen;
    public GameObject quizScreen;

    private void Start()
    {
        if(StateManager.remote)
        {
            try
            {
                GameObject.Find("Toggle").GetComponent<Toggle>().isOn = true;
            } 
            catch(NullReferenceException e)
            {
                Debug.Log("Testing Mode.");
            }
        }
    }

    public void startBtn()
    {
        mainMenu.SetActive(false);

        if (StateManager.lectionRead && StateManager.rulesRead)
        {
            loadingScreen.SetActive(true);
            SceneManager.LoadScene("Soba");
        }
        else
        {
            gameForbbidenScreen.SetActive(true);
        }
    }

    public void quizBtn()
    {
        if(StateManager.gameOver)
        {
            quizScreen.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(false);
            quizForbbidenScreen.SetActive(true);
        }
    }

    public void menuBtn()
    {
        SceneManager.LoadScene("Lekcija");
    }

    public void setRemoteSelect()
    {   
        if (StateManager.remote)
        {
            StateManager.remote = false;
        }
        else
        {
            StateManager.remote = true;
        }
    }

    public void readRules()
    {
        StateManager.rulesRead = true;
    }

    public void readLection()
    {
        StateManager.lectionRead = true;
    }

}
