using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;


/**
 * This is the Script For Enemy Collision.
 * 
 * This add the gameobject the collider and react when collide.
 * 
 * Contributors     Name            Github UserName
 *                  Zheng Yuan      YuanZheng01
 *                  Joseph Roberts  Techj70/jrobertsSCAD
 * 
 * Update History   Date        Important Changes
 *                  2022.07.30  Script Created - Zheng Yuan
 *                  2022.08.01  Moved code from Enemy.cs on player-health branch to this script - Joseph Roberts
 *                  2022.08.19  Added code to OnTriggerEnter2D so the enemy is destroyed after touching the player - Joseph Roberts
 *                  2022.08.25  Added code to OnTriggerEnter2D so the enemy is destroyed after touching the dash trail and to instantiate a destruction effect - Joseph Roberts
 *                  
 */

// This Class Haven't Finish Yet!
// Will Update and Improve In Future Sprint When Detailed Functionality Decided.

[RequireComponent(typeof(Collider2D))] // makes this script require a gameObject to have to also have a Collider2D component before it can be added to that gameObject - Joseph Roberts
[RequireComponent(typeof(AudioSource))]

public class EnemyCollision : MonoBehaviour
{
    #region Attributes

    [NonSerialized] public Collider2D collider; // collider variable; needed to detect collisions - Joseph Roberts
    
    [SerializeField] [Tooltip("enemy collision effect")] public GameObject enemyCollisionEffect;
    private GameObject _enemyCollided;

    private Transform _enemyPosition;
    
    [SerializeField] [Tooltip("damage in whole numbers (integer) that the enemy causes to the player when it collides with them")] public int intPlayerDamage;
    
    [SerializeField] [Tooltip("damage in decimal numbers (float) that the enemy causes to the player when it collides with them")] public float floatPlayerDamage;
    
    [SerializeField] [Tooltip("audio clip for enemy destruction")] public AudioClip enemyDestroyed;

    private AudioSource _sceneAudio;

    public GameObject player;

    private Component _playerHealthManager;

    #endregion
    
    // Start is called before the first frame update
    #region Unity Functions
    void Start()
    {
        collider = gameObject.GetComponent<Collider2D>(); // gets the collider that is attached to this gameObject that is required by [RequireComponent(typeof(Collider2D))] above on this script and assigns it to the collider variable - Joseph Roberts
        collider.isTrigger = true;                        // ensures the trigger check box is "on" for making the collider a trigger to generate trigger events - Joseph Roberts

        _sceneAudio = GameObject.Find("Background").GetComponent<AudioSource>();
        
        player = GameObject.Find("Player");
        //_playerHealthManager = player.GetComponent<PlayerHealthManager>();

        if (intPlayerDamage <= 0) // checks for intPlayerDamage to be set - Joseph Roberts
        {
            intPlayerDamage = 1;
            Debug.Log("no intPlayerDamage set on Enemy.cs of " + gameObject.name + "; intPlayerDamage set to a default of 1");
        }
        
        if (floatPlayerDamage <= 0f) // checks for floatPlayer Damage to be set - Joseph Roberts
        {
            floatPlayerDamage = 1f;
            Debug.Log("no floatPlayerDamage set on Enemy.cs of " + gameObject.name + "; floatPlayerDamage set to a default of 1f");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    void OnTriggerEnter2D(Collider2D collider)
    {
        // The checker in Player Script Might Have Issue when Multiple Collision happens.
        // If needed, could refactor Player get hit to this Script.
        
         if (collider.gameObject == player || collider.name == "Dash Trail(Clone)")
        {
            //if (player.PlayerHealthManager.vulnerable == false)
            //{
            //    return;
            //}
            //else
            //{
                _enemyPosition = gameObject.transform; // records teh gameObject transform
                if (_enemyCollided != null) Destroy(_enemyCollided); // destroys the _enemyCollided gameObject if it already exists - Joseph Roberts 
                _enemyCollided = Instantiate(enemyCollisionEffect, _enemyPosition.position, Quaternion.identity); // created collision effect...
                                                                                // ... at recorded gameObject transform.position - Joseph Roberts
                _sceneAudio.PlayOneShot(enemyDestroyed, .2f);
                Destroy(this.gameObject); // enemy is destroyed after touching the player - Joseph Roberts
            //}
        }
    }
}
