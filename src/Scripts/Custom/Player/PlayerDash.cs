using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * script for player dash; is attached to DashTrial prefab
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.08.01  Script Created - Joseph Roberts
 *                         2022.08.04  Fit with changes in Enemy Prefab - Zheng Yuan
 *                         2022.08.25  moved the Destroy(this.gameObject) for the enemy to the EnemyCollision.cs OnTriggerEnter2D() from the PlayerDash.cs;...
 *                                     ... added the “Dash Trail (clone)” gameObject name to the check for collisions - Joseph Roberts
 */

[RequireComponent(typeof(Collider2D))]
public class PlayerDash : MonoBehaviour
{
    #region Unity_Functions
    // Start is called before the first frame update -Joseph Roberts
    void Start()
    {
        
    }

    // Update is called once per frame -Joseph Roberts
    void Update()
    {
        if (GameManager.levelOver == true) Destroy(gameObject); // deletes the dash trail if it's still around when the level ends -Joseph Roberts
    }
    #endregion

    public void OnTriggerEnter2D(Collider2D other) // is called when the a collision with another collider is detected on this gameObject's collider (which must have "is trigger" checked) -Joseph Roberts
    {
        Debug.Log("OnTriggerEnter started on " + gameObject.name + " on its PlayerDash.cs component");
        if (other.gameObject.GetComponent<EnemyCollision>() == true) // checks to see if the object that collided with the trigger had the Enemy.cs component attached to it -Joseph Roberts
        {
            Debug.Log("collision occured with enemy game object " + other.gameObject.name);
            ScoreKeeper.IncreaseScore(other.GetComponent<Enemy>().pointValue);
        }
    }
}
