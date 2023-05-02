using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public bool scored = false; //Flag to check if a goal has been scored
    private void OnTriggerEnter2D(Collider2D other)
    {
        scored = true; //If the trigger area of the goals has been entered by another collider a goal has been scored
        //Only the puck can make it out at the goal openings
    }
}
