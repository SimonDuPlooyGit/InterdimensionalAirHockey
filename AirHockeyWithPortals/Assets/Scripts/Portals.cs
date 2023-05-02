using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portals : MonoBehaviour
{
    public float speedRot; //Speed at which the portal will spin
    public float speedMove; //Speed at which the portals will "Ping pong" between point 1 and 2
    public GameObject otherPortal; //Variable to link this portal with the other portal
    public GameObject manager; //Variable to have access to the Game Manager
    public Transform otherPortalT; //The transform vector of the other portal
    public bool teleporting = false; //Flag to check whether the portals are currently teleporting the puck

    public Vector2 pos1; //First position for the portals to start at
    public Vector2 pos2; //Last position for the portals to move to

    void FixedUpdate()
    {
        transform.Rotate(Vector3.back , speedRot * 60 * Time.deltaTime); //Spinning the portals for an aesthethic effect
        transform.position = Vector3.Lerp(pos1, pos2, Mathf.PingPong(Time.time * speedMove, 1.0f));
        //PingPong gives an incrementing or decrementing value between 0 and 1 since the length was specified as 1f
        //Time.time is a self incrementing value which the function requires, and it oscillates between 0 and 1 on a factor of the current time * by the movement speed
        //The position to move to is then determined by a linear interpolation (lerp) between pos1 and pos2 according to the weighting of the pingpong value
        //At Y (the ping pong value) = 0 then lerp will return pos1, and at Y = 1 it will return pos2, at any point  0 < Y < 1 it will be at a position according to the equation:
        //position = pos1 + (pos2 - pos1) * Y
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Puck") && !teleporting) //If the puck has collided with the portal and its teleporting flag is false
        { //It is neccessary to have a flag for teleporting and for it to be true for both portals once teleporting, otherwise the puck will infinitely teleport between the two portals
            teleporting = true;
            otherPortal.GetComponent<Portals>().teleporting = true;
            other.transform.position = otherPortalT.position; //Teleport the puck
            manager.gameObject.GetComponent<GameManager>().player.GetComponent<MalletPlayer>().hitThePuck = false;
            manager.gameObject.GetComponent<GameManager>().ai.GetComponent<CPUbehaviour>().hitThePuckAI = false; //Reset the multiple puck hitting check
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        teleporting = false; //Current teleport ends when the puck exits the portal colliders, portal 1's will be false once the puck teleports away and the 2nd's will be false once the puck leaves it
    }
}
