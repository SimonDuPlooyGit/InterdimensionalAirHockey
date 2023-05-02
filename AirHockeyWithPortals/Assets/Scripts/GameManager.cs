using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject ai;
    public GameObject portO;
    public GameObject portB;
    public GameObject puc;
    public GameObject goals;
    public Canvas pauseScreen;
    public Canvas scoreUI;
    public Canvas end;
    public Canvas diff;
    public TMP_Text count;
    public TMP_Text pscore;
    public TMP_Text ascore;
    public TMP_Text endText; //Variables to access all of the games menu's, ui elements and game objects

    public int pscoreINT = 0;
    public int ascoreINT = 0; //Score variables

    Vector2 playerStart = new Vector2 (-0.701f, -0.344f);
    Vector2 botStart = new Vector2 (0.701f, 0.361f);
    Vector2 puckStart = new Vector2(0,0); //Starting positions for the game elements

    public bool paused; //Flag to check if the game has been paused
    void Start()
    {
        Time.timeScale = 0f; //The scale at which time passes, 0 essentially pauses the game/everything that works on a time basis
        count.gameObject.SetActive(false);
        pauseScreen.gameObject.SetActive(false);
        scoreUI.gameObject.SetActive(false);
        end.gameObject.SetActive(false);
        diff.gameObject.SetActive(true); //Activate and deactivate all the various menus and ui elements as neccessary
        paused = false; //Initialize the starting paused state
    }
    void Update()
    {

        pscore.text = pscoreINT.ToString();
        ascore.text = ascoreINT.ToString(); //UI score elements equal to the scoring variables

        if (goals.GetComponent<GoalTrigger>().scored == true) //If a goal was scored
        {
            if (puc.transform.position.x > 0) //And the puck is on the Ai's side 
            {
                puckStart.x = 0.38f; //Then spawn the puck on the AI's side, since they got scored against
                pscoreINT++; //Player gains a point
                player.gameObject.GetComponent<MalletPlayer>().hitThePuck = false;
                ai.gameObject.GetComponent<CPUbehaviour>().hitThePuckAI = false; //Reset the multiple puck hit flags
                StartCoroutine(countDown()); //Start the countdown

            } else
            {
                puckStart.x = -0.38f; //Else repeat the process but the puck starts on the player's side since the player was scored against
                ascoreINT++;
                player.gameObject.GetComponent<MalletPlayer>().hitThePuck = false;
                ai.gameObject.GetComponent<CPUbehaviour>().hitThePuckAI = false;
                StartCoroutine(countDown());
            }
            goals.GetComponent<GoalTrigger>().scored = false; //Reset the goal flag
            player.GetComponent<MalletPlayer>().clicked = false; //Reset the flag to know if the player's mallet has been clicked

            player.transform.position = playerStart;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            ai.transform.position = botStart;
            ai.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            puc.transform.position = puckStart;
            puc.GetComponent<Rigidbody2D>().velocity = Vector2.zero; //Reset the positions of the mallets and puck and make their velocities equal to zero

        }

        if (pscoreINT == 5 || ascoreINT == 5) //If a score of 5 is achieved
        {
            if (pscoreINT == 5)
            {
                endText.text = "Player Won"; //The player won if their score is equal to 5
            } else
            {
                endText.text = "Cpu won"; //Else the AI won
            }
            Time.timeScale = 0f; //Pause the game
            scoreUI.gameObject.SetActive(false);
            end.gameObject.SetActive(true); //Display the ending information
        }

        if (Input.GetKeyDown(KeyCode.Escape)) //If escape is pressed
        {
            if (paused == false) //And the game is not currently paused
            {
                Time.timeScale = 0f; //Pause the game
                pauseScreen.gameObject.SetActive(true); //Show the pause menu
                scoreUI.gameObject.SetActive(false); 
                paused = true; //Flag that the game is paused

            } else if (paused == true) //Else if the game is currently paused
            {
                Time.timeScale = 1f; //Unpause
                pauseScreen.gameObject.SetActive(false);
                scoreUI.gameObject.SetActive(true); //Show the game UI elements again
                paused = false; //Flag that the game is no longer paused
            }
        }
    }

    public void resume() //Functionality for the resume button
    {
        Time.timeScale = 1f;
        pauseScreen.gameObject.SetActive(false);
        scoreUI.gameObject.SetActive(true);
        paused = false;
    }

    public void menu() //Functionality for the menu button
    {
        SceneManager.LoadScene(0); //Load the menu scene
    }

    public void replay() //Functionality for the replay button
    {
        SceneManager.LoadScene(1); //Reload the game scene
    }

    public void easy() //Functionality for the easy button
    {
        portB.gameObject.GetComponent<Portals>().speedMove = 0.25f;
        portO.gameObject.GetComponent<Portals>().speedMove = 0.25f;
        ai.gameObject.GetComponent<CPUbehaviour>().speed = 1f; //Adjust the AI and portals movement speeds
        scoreUI.gameObject.SetActive(true);
        diff.gameObject.SetActive(false);
        StartCoroutine(countDown());
        Time.timeScale = 1f;
    }

    public void medium() //Funtionality for the medium button
    {
        portB.gameObject.GetComponent<Portals>().speedMove = 0.5f;
        portO.gameObject.GetComponent<Portals>().speedMove = 0.5f;
        ai.gameObject.GetComponent<CPUbehaviour>().speed = 1.5f; //Adjust the AI and portals movement speeds
        scoreUI.gameObject.SetActive(true);
        diff.gameObject.SetActive(false);
        StartCoroutine(countDown());
        Time.timeScale = 1f;
    }

    public void hard() //Functionality for the hard button
    {
        portB.gameObject.GetComponent<Portals>().speedMove = 1f;
        portO.gameObject.GetComponent<Portals>().speedMove = 1f;
        ai.gameObject.GetComponent<CPUbehaviour>().speed = 2f; //Adjust the AI and portals movement speeds
        scoreUI.gameObject.SetActive(true);
        diff.gameObject.SetActive(false);
        StartCoroutine(countDown());
        Time.timeScale = 1f;
    }

    IEnumerator countDown() //Countdown function
    {
        player.gameObject.GetComponent<MalletPlayer>().enabled = false;
        ai.gameObject.GetComponent<CPUbehaviour>().enabled = false;
        portO.gameObject.GetComponent<Portals>().enabled = false;
        portB.gameObject.GetComponent<Portals>().enabled = false; //Disable all of the movement scripts to "pause" the game
        count.gameObject.SetActive(true); //Show the countdown
        count.text = "3";
        yield return new WaitForSeconds(1);
        count.text = "2";
        yield return new WaitForSeconds(1);
        count.text = "1";
        yield return new WaitForSeconds(1); //Show and wait for the countdown
        count.gameObject.SetActive(false); //Stop showing the countdown
        player.gameObject.GetComponent<MalletPlayer>().enabled = true;
        ai.gameObject.GetComponent<CPUbehaviour>().enabled = true;
        portO.gameObject.GetComponent<Portals>().enabled = true;
        portB.gameObject.GetComponent<Portals>().enabled = true; //Reactivate the scripts
    }
}
