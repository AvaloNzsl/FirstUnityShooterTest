using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    public Player playerHealth;       // Reference to the player's health.
    public float restartDelay = 5f;         // Time to wait before restarting the level


    Animator anim;                          // Reference to the animator component.
    float restartTimer;                     // Timer to count up to restarting the level


    //// Use this for initialization
    //void Start()
    //{

    //}
    void Awake()
    {
        // Set up the reference.
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        // If the player has run out of health...
        if (playerHealth.Health <= 0) 
        {
            // ... tell the animator the game is over.
            anim.SetTrigger("GameOver");

            Cursor.lockState = CursorLockMode.Locked;
            // .. increment a timer to count up to restarting.
            restartTimer += Time.deltaTime;

            // .. if it reaches the restart delay...
            if (restartTimer >= restartDelay)
            {
                // .. then reload the currently loaded level.
                //Application.LoadLevel(1);
                Application.LoadLevel(1);
            }
        }
    }
}
