using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUbehaviour : MonoBehaviour
{
    public Rigidbody2D bodyPuck;
    public Rigidbody2D body;
    public GameObject manager; //Variables to access the puck and mallet's rigidbodies and the game manager

    public float speed; //Speed to move at

    public Vector2 anchor; //Anchor point at the goal for midpoint calculations
    private Vector2 midpoint; //Midpoint between the puck and anchor point

    public bool hitThePuckAI = false;
    public bool collidingAI = false; //Same as the player mallet's flags for the multiple puck hit behaviour

    void FixedUpdate()
    {
        if (manager.gameObject.GetComponent<GameManager>().puc.transform.position.x < 0)
        {
            hitThePuckAI = false; //Same as player's mallet
        }

        if (bodyPuck.transform.position.x < 0) //State 1 of the AI behaviour
        {
            midpoint.x = (anchor.x + bodyPuck.transform.position.x) / 2f;
            midpoint.y = (anchor.y + bodyPuck.transform.position.y) / 2f;
            transform.position = Vector2.MoveTowards(transform.position, midpoint, speed * Time.deltaTime);
            //If the puck is on the player's side then the AI will try to position itself between the puck and the goal/anchor point

        } else if (bodyPuck.transform.position.x > 0 && hitThePuckAI == false) //State 2 of the AI behaviour
        {
            Vector2 direction = new Vector2(bodyPuck.transform.position.x - body.transform.position.x, bodyPuck.transform.position.y - body.transform.position.y);
            //Straight line direction that the puck should move in to go to the puck
            direction = direction.normalized; //Unit vector of the direction
            body.velocity = direction * speed; //Add a velocity to the body which is equal to the speed in the direction of the puck
            //If the puck is on the AI's side and it has not hit the puck already, it will move towards the puck to hit it

        } else if (body.transform.position.x > 0 && hitThePuckAI == true) //State 3 of the AI behaviour
        {
            midpoint.x = (anchor.x + bodyPuck.transform.position.x) / 2f;
            midpoint.y = (anchor.y + bodyPuck.transform.position.y) / 2f;
            transform.position = Vector2.MoveTowards(transform.position, midpoint, speed * Time.deltaTime);
            //If the puck is on the AI's side it repeats the behaviour of state 1, to avoid hitting the puck again but to block it if needs be
        }
        
    }
    //All the code below is a repeat of the player mallet's script
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collidingAI == false && collision.gameObject.name == "Puck" && hitThePuckAI == false)
        {
            collidingAI = true;
            hitThePuckAI = true;

        }
        else if (collidingAI == false && collision.gameObject.name == "Puck" && hitThePuckAI == true)
        {
            collidingAI = true;
            manager.gameObject.GetComponent<GameManager>().ascoreINT = (manager.gameObject.GetComponent<GameManager>().ascoreINT - 1);

            if (manager.gameObject.GetComponent<GameManager>().ascoreINT < 0)
            {
                manager.gameObject.GetComponent<GameManager>().ascoreINT = 0;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collidingAI == true && collision.gameObject.name == "Puck")
        {
            collidingAI = false;
        }
    }
}
