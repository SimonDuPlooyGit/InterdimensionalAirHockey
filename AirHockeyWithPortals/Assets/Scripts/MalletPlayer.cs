using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MalletPlayer : MonoBehaviour
{
    public Rigidbody2D body;
    public GameObject manager; //Variables to access the mallet's rigidbody and the game manager

    private Vector2 target; //Position where the mallet will need to move to
    private Vector2 size; //Vector 2 of the x and y size of the mallet's sprite

    private bool moving = false; //Flag to see if the mallet is currently moving
    public bool clicked = false; //Flag to see if the mallet has been clicked
    public bool hitThePuck = false; //Flag to see if the mallet has hit the puck already
    public bool colliding = false; //Flag to see if the mallet is currently colliding with the puck

    private void Start()
    {
        size = gameObject.GetComponent<SpriteRenderer>().bounds.extents; //Getting the sprite's size values and storing it in size
    }

    void FixedUpdate()
    {
        if (manager.gameObject.GetComponent<GameManager>().puc.transform.position.x > 0)
        {
            hitThePuck = false; //Reset the double puck hit flag once the puck has gone to the AI's half
        }
    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0)) //If left click is pressed
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Getting the world position of the mouse cursor

            if ((mouse.x >= transform.position.x && mouse.x < transform.position.x + size.x ||
                mouse.x <= transform.position.x && mouse.x > transform.position.x - size.x) &&
                (mouse.y >= transform.position.y && mouse.y < transform.position.y + size.y ||
                mouse.y <= transform.position.y && mouse.y > transform.position.y - size.y)) //If the mouse cursor's position is withing the bounds of the sprite's size, i.e. you have clicked on the mallet
            {
                clicked = true;
                moving = true; //The mallet has been clicked and is moving
            }

        }
        else if (Input.GetMouseButtonUp(0)) //When left click is released
        {
            moving = false;
            clicked = false; //The mallet has no longer been clicked and is not moving
        }

        if (moving == true && clicked == true) //If the mallet has been clicked and is moving
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Make the target position equal to the mouse cursor's world position
            body.MovePosition(target); //Move the mallet's rigidbody with the MovePosition function to the target position
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) //When the mallet's collider is entered
    {
        if (colliding == false && collision.gameObject.name == "Puck" && hitThePuck == false)
        { //If the mallet is not currently colliding with the puck, the colliding collider is from the puck and the puck was not hit prior
            colliding = true;
            hitThePuck = true; //The mallet will be colliding with the puck and the mallet will have hit the puck

        } else if (colliding == false && collision.gameObject.name == "Puck" && hitThePuck == true) //If the mallet hits the puck a second time or more
        {
            colliding = true;
            manager.gameObject.GetComponent<GameManager>().pscoreINT = (manager.gameObject.GetComponent<GameManager>().pscoreINT - 1); //The score must decrement

            if (manager.gameObject.GetComponent<GameManager>().pscoreINT < 0)
            {
                manager.gameObject.GetComponent<GameManager>().pscoreINT = 0; //But not go below 0
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (colliding == true && collision.gameObject.name == "Puck")
        {
            colliding = false; //The currently colliding flag needs to be returned to false once they are no longer colliding.
            //The colliding check is neccessary to avoid repeat colliding detection and repeat score decrements, it insures that is in only detected once and decremented once at the start of a collision
        }
    }
}
