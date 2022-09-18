using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/**
 * script for keep track of player health; relies on HealthBar and HealthIcons scripts
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.07.27  Script Created - Joseph Roberts
 *                         2022.08.04  Edits to resolve automatic losing issue when game build is played - Joseph Roberts
 *                         2022.08.05  Enabled the CurrentIntHealth() function to correct...
 *                                          ... bug (see https://github.com/Unexpected-Rogues/353Studios_ProjectRepo/issues/16); called by GameManager in Update() - Joseph Roberts
 *                         2022.09.02  added code for player damaged sound and damage invulnerability (see PlayerDamaged() and OnTriggerEnter2D()) - Joseph Roberts
 *                  
 */

[RequireComponent(typeof(Rigidbody2D))] // makes this script require a gameObject to have to also have a RigidBody component before it can be added to that gameObject -Joseph Roberts
[RequireComponent(typeof(Collider2D))] // makes this script require a gameObject to have to also have a Collider2D component before it can be added to that gameObject -Joseph Roberts

public class PlayerHealthManager : MonoBehaviour // used to manage the adding and subtracting of player health based on collision with a collider trigger on the same gameObjects as it -Joseph Roberts
{
    #region Attributes
    [SerializeField] [Tooltip("reference for health icons object")] public GameObject healthIconsObject;
    [SerializeField] [Tooltip("reference for health bar object")] public GameObject healthBarObject;

    
    [SerializeField] [Tooltip("max health for whole number based health tracking")] public int integerMaxHealth;
    [SerializeField] [Tooltip("max health for decimal based health tracking")] public float floatMaxHealth;
    
    [SerializeField] [Tooltip("player damaged sound effect")] public Boolean useIcons = true;
    [SerializeField] [Tooltip("Use a health bar for health tracking?")] public Boolean useBar;
    
    [SerializeField] [Tooltip("player damaged sound effect")] public AudioClip playerDamagedClip;
    [SerializeField] [Tooltip("reference to player image")] public Image playerImage;

    public AudioSource sceneAudioSource;

    private float _invunerableTime = 2f;

    private Coroutine _playerBlinking; 

    public bool vulnerable = true; 

    [NonSerialized] public int currentIntHealth;
    [NonSerialized] public float currentFloatHealth;
    
    #endregion

    #region Unity_Functions
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = true;
        
        if (useIcons == true && healthIconsObject != null) currentIntHealth = healthIconsObject.GetComponent<HealthIcons>().currentIntHealth;
        if (useBar == true && healthBarObject != null) currentFloatHealth = healthIconsObject.GetComponent<HealthBar>().currentFloatHealth;
        
        if (playerImage == null) // checks to see if the player has chosen at least 1 method for visually tracking the player's health - icons or bar -Joseph Roberts
        {
            Debug.LogError("no player reference image set on " + gameObject.name);
        }
        
        if (useIcons == false && useBar == false) // checks to see if the player has chosen at least 1 method for visually tracking the player's health - icons or bar -Joseph Roberts
        {
            Debug.LogError("no health tracking method chosen on " + gameObject.name);
        }

        if (healthIconsObject == null && healthBarObject == null) // checks to see if the the referenced to the objects to visual track the players health have been assigned -Joseph Roberts
        {
            Debug.LogError("no health tracking objects referenced on " + gameObject.name);
        }
        
        if (healthIconsObject != null) healthIconsObject.GetComponent<HealthIcons>().maxIntHealth = integerMaxHealth; // sets the max health variable on the icons object to the integerMaxHealth...
                                                                                                                      // ... defined by the user in the inspector -Joseph Roberts
        if (healthBarObject != null) healthBarObject.GetComponent<HealthBar>().maxFloatHealth = floatMaxHealth; // sets the max health variable on the bar object to the floatMaxHealth defined...
                                                                                                                // ... by the user in the inspector -Joseph Roberts

        if (integerMaxHealth <= 0)
        {
            healthIconsObject.GetComponent<HealthIcons>().maxIntHealth = 3;  // sets the max int health to a default of 3 if the the max int health is set to 0 or below in the component for this...
                                                                             // ... script in the inspector -Joseph Roberts
            Debug.LogError("max int health on the PlayerHealthManager.cs on " + gameObject.name + " was set to 0 or below; reset to default of 3");
        }
        
        if (floatMaxHealth <= 0) 
        {
            healthIconsObject.GetComponent<HealthBar>().maxFloatHealth = 3f;    // sets the max float health to a default of 3f if the the max float health is set to 0 or below in the component...
                                                                                // ... for this script in the inspector -Joseph Roberts
            Debug.LogError("max float health on the PlayerHealthManager.cs on " + gameObject.name + " was set to 0 or below; reset to default of 3f");
        }
    }

    private void Awake()
    {
        if (healthIconsObject != null) healthIconsObject.GetComponent<HealthIcons>().maxIntHealth = integerMaxHealth; // sets the max health variable on the icons object to the integerMaxHealth...
                                                                                                                      // ... defined by the user in the inspector -Joseph Roberts
        if (healthBarObject != null) healthBarObject.GetComponent<HealthBar>().maxFloatHealth = floatMaxHealth; // sets the max health variable on the bar object to the floatMaxHealth defined by...
                                                                                                                // ... the user in the inspector -Joseph Roberts
    }

    // Update is called once per frame
    void Update()
    {
        if (useIcons == true && healthIconsObject != null) currentIntHealth = healthIconsObject.GetComponent<HealthIcons>().currentIntHealth;
        if (useBar == true && healthBarObject != null) currentFloatHealth = healthIconsObject.GetComponent<HealthBar>().currentFloatHealth;
    }
    #endregion


    public int CurrentIntHealth() // meant to be called from other scripts to get current health; enabled to correct bug with GameManager.cs automatically calling "Lose" on builds of...
                                  // ... the game from Update (see https://github.com/Unexpected-Rogues/353Studios_ProjectRepo/issues/16) -Joseph Roberts
    {
        if (useIcons == true) // checks to see if the user has selected to use icons in the inspector
        {
          currentIntHealth = healthIconsObject.GetComponent<HealthIcons>().currentIntHealth; // calls IncreasePlayerIntHp method on the healthIconsObject and passes the amount of HP that...
                                                                                             // ... the HP.cs component has set for intPlayerHeal to increase the player health by that amount -Joseph Roberts
          // Debug.Log("CurrentIntHealth() called on " + gameObject.name + "; currentIntHealth equal to " + currentIntHealth);
        }
        return currentIntHealth;
    }
    
    
    /* 
    *public float CurrentFloatHealth() // meant to be called from other scripts to get current health; not currently used -Joseph Roberts
    *{
    *   if (useBar == true) // checks to see if the user has selected to use a bar in the inspector
    *    {
    *        // Debug.Log("" + useIcons + "" + gameObject.name);
    *        currentFloatHealth = healthIconsObject.GetComponent<HealthIcons>().currentIntHealth; // calls IncreasePlayerIntHp method on the healthIconsObject and passes the amount of HP that the HP.cs...
    *                                                                                             // ... component has set for intPlayerHeal to increase the player health by that... amount -Joseph Roberts
    *    }
    * 
    *    return currentFloatHealth;
    *}
    */
    
    public void OnTriggerEnter2D(Collider2D other) // is called when the a collision with another collider is detected on this gameObject's collider (which must have "is trigger" checked) -Joseph Roberts
    {
        Debug.Log("OnTriggerEnter started on " + gameObject.name + " on its PlayerHealthManager.cs component");
        if (useIcons == true) // checks to see if the user has selected to use icons in the inspector -Joseph Roberts
        {
            if (other.gameObject.GetComponent<EnemyCollision>() == true && vulnerable == true) // checks to see if the object that collided with the trigger had the Enemy.cs component attached to it -Joseph Roberts
            {
                vulnerable = false;
                Debug.Log("useIcons equal to " + useIcons + " and collision occured with enemy game object " + other.gameObject.name);
                healthIconsObject.GetComponent<HealthIcons>().DecreasePlayerIntHp(other.gameObject.GetComponent<EnemyCollision>().intPlayerDamage); // calls DecreasePlayerIntHp method on the healthIconsObject...
                                                                                                                                                    // ... and passes the amount of damage that the Enemy.cs...
                                                                                                                                                    // ... component has set for intPlayerDamage to decrease...
                                                                                                                                                    // ... the player health by that amount -Joseph Roberts
                _playerBlinking = StartCoroutine(PlayerDamaged());

            }
            else if (other.gameObject.GetComponent<HP>() == true) // checks to see if the object that collided with the trigger had the HP.cs component attached to it -Joseph Roberts
            {
                Debug.Log("useIcons equal to " + useIcons + " and collision occured with healing game object " + other.gameObject.name);
                healthIconsObject.GetComponent<HealthIcons>().IncreasePlayerIntHp(other.gameObject.GetComponent<HP>().intPlayerHeal);; // calls IncreasePlayerIntHp method on the healthIconsObject and passes...
                                                                                                                                       // ...the amount of HP that the HP.cs component has set for intPlayerHeal...
                                                                                                                                       // ... to increase the player health by that amount -Joseph Roberts
            }
                
        }

        if (useBar == true) // checks to see if the user has selected to use a bar in the inspector
        {
            if (other.gameObject.GetComponent<EnemyCollision>() == true) // checks to see if the object that collide with the trigger had the Enemy.cs component attached to it -Joseph Roberts
            { 
                vulnerable = false;
                Debug.Log("useBar equal to " + useBar + " and collision occured with enemy game object " + other.gameObject.name);
                healthBarObject.GetComponent<HealthBar>().DecreasePlayerFloatHp(other.gameObject.GetComponent<EnemyCollision>().floatPlayerDamage); // calls DecreasePlayerFloatHp method on the healthBarObject...
                                                                                                                                                    // ... and passes the amount of damage that the Enemy.cs...
                                                                                                                                                    // ... component has set for floatPlayerDamage to decrease...
                                                                                                                                                    // ... the player health by that amount -Joseph Roberts  
                _playerBlinking = StartCoroutine(PlayerDamaged());
            }
            else if (other.gameObject.GetComponent<HP>() == true) // checks to see if the object that collide with the trigger had the HP.cs component attached to it -Joseph Roberts
            {
                Debug.Log("useBar equal to " + useBar + " and collision occured with healing game object " + other.gameObject.name);
                healthBarObject.GetComponent<HealthBar>().IncreasePlayerFloatHp(other.gameObject.GetComponent<HP>().floatPlayerHeal);; // calls IncreasePlayerFloatHp method on the healthBarObject and passes...
                                                                                                                                       // ... the amount of HP that the HP.cs component has set for floatPlayerHeal...
                                                                                                                                       // ... to increase the player health by that amount -Joseph Roberts
            }
        }
    }

    IEnumerator PlayerDamaged()
    {
        float time = 0f;
        Color color = playerImage.color;
        
        sceneAudioSource.PlayOneShot(playerDamagedClip, .2f);

        while (time < _invunerableTime)
        {
            float blinkerAlpha = Mathf.PingPong(5f * Time.time, 1);
            color = new Color(color.r, color.g, color.b, blinkerAlpha);
            playerImage.color = color;
            time += Time.deltaTime;
            yield return null;
        }
        
        vulnerable = true;
        playerImage.color = new Color(color.r, color.g, color.b, 255f);
        yield return null;
    }
}
