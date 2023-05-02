using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour
{
    public Canvas HTP;
    public Canvas MM;

    public void Start()
    {
        HTP.gameObject.SetActive(false); //How to play menu should not be visible
        MM.gameObject.SetActive(true); //Main menu made to be visible
    }
    public void controls()
    {
        HTP.gameObject.SetActive(true); //How to play visible
        MM.gameObject.SetActive(false); //Main menu not visible
    }

    public void back()
    {
        HTP.gameObject.SetActive(false); //How to play not visible 
        MM.gameObject.SetActive(true); //Main menu visible
    }
    public void Exit()
    {
        Application.Quit(); //Quit the game and close it
    }

    public void startGame()
    {
        SceneManager.LoadScene(1); //Start the game scene to play the game
    }
}
