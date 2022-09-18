using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is the Script For Enemy.
 * 
 * This add the basic attributes to attached gameobject.
 * 
 * Contributors          Name            Github UserName
 *                      Zheng Yuan      YuanZheng01
 *                      Joseph Roberts  Tech70/jrobertsSCAD
 * 
 * Update History       Date        Important Changes
 *                      2022.07.29  Script Created =Zheng Yuan
 *                      2022.08.02  added point value and some debug -Joseph Roberts
 *                      2022.09.02  added lifetime to enemy for removing them once they have exited the screen and
 *                                       improve optimization - Joseph Roberts
 */

// This Class Haven't Finish Yet!
// Will Update In Future Sprint When Detailed Functionality Decided.

public class Enemy : MonoBehaviour
{
    #region Attributes
    // Basic attributes for enemy.
    [Tooltip("the current and max possible health of the enemy")]
    [SerializeField] float health, maxHealth = 3f;
    
    [Tooltip("point value gained when enemy is destroyed")]
    [SerializeField] public int pointValue = 1;
    
    [Tooltip("lifetime of enemy object in seconds before self-destruct (default = 10 seconds)")]
    [SerializeField] public float lifetime = 10f;
    
    [Tooltip("reference to whole enemy gameObject")]
    [SerializeField] public GameObject enemyObject;

    private Coroutine _ttlCoroutine; 
    
    #endregion

    #region Unity Functions
    void Start()
    {
        if (maxHealth <= 0)
        {
            Debug.LogError("Enemy.cs maxHealth is less than or equal to 0 on " + gameObject.name + " gameObject; reset to 3f by default");
            maxHealth = 3f;
        }
        
        health = maxHealth;
        
        // past the lifetime of the enemy and the parent gameObject of the enemy component to the coroutine to start TimeToLive - Joseph Roberts
        _ttlCoroutine = StartCoroutine(TimeToLive(lifetime, enemyObject));
    }

    void Update()
    {

    }
    #endregion

    #region Functions
    // Enemy Initializer
    public void InitEnemy(float maxHealth)
    {
        Health = MaxHealth = maxHealth;
    }
    #endregion

    #region Getter && Setter
    // Getter && Setter
    public float Health
    {
        get { return health; }
        set { health = (value > MaxHealth) ? MaxHealth : (value > 0) ? value : 0 ; }
    }
    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = (value > 0) ?  value : 0; }
    }

    //  Move to Enemy Movement.
    //
    //public float Speed
    //{
    //    get { return speed; }
    //    set { speed = Mathf.Abs(value); }
    //}
    //public float Velocity
    //{
    //    get { return velocity; }
    //    set { velocity = value; }
    //}
    #endregion
    
    IEnumerator TimeToLive(float timeToLive, GameObject enemyObject)
        // destroys the enemy gameObject after its lifetime expires - Joseph Roberts
    {
        float time = 0; // sets the time variable  to 0; used to compare against the timeToLive variable for how much time has passed - Joseph Roberts

        while (time < timeToLive) // while the time variable is less than the timeToLive variable... - Joseph Roberts
        {
            time += Time.deltaTime;  // increase the time variable by amount of real-time pasted since last check - Joseph Roberts
            yield return null;       // complete the coroutine and return nothing back - Joseph Roberts
        }
        
        Destroy(enemyObject); // destroy the stored enemyObject - Joseph Roberts
    }
}