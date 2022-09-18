using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/**
 * script for keep track of player health; relies on HealthBar and HealthIcons scripts
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.07.27  Script Created - Joseph Roberts
 *                  
 */

public class HealthBar : MonoBehaviour
{
    #region Attributes
    [NonSerialized] public float currentFloatHealth; // player health variable -Joseph Roberts
    
    [NonSerialized] public float maxFloatHealth; // max player health variable -Joseph Roberts
    
    [Tooltip("reference to slider to change its fill to reflect health")]
    public Slider healthSlider;
    
    // [Tooltip("reference to image to change the opacity on to reflect health (example: set the image as a heart so it will get more transparent as health decreases)")] // this was to include a "health image" that could have its fill amount changed to reflect health; not currently used -Joseph Roberts
    // public Image healthImage;
    
    // public double healthFill;
    #endregion

    #region Unity_Functions
    private void Start()
    {
        healthSlider = this.gameObject.GetComponent<Slider>(); // gets the Slider component from the gameObject this script is attached to and the healthSlider gameObject reference to it -Joseph Roberts
        if (healthSlider == null /* && healthImage == null*/) // checks to see if the healthSlider gameObject reference is not empty -Joseph Roberts
        {
            Debug.LogError("health bar graphic not defined in HealthBar.cs on " + gameObject.name);
            healthSlider = gameObject.GetComponent<Slider>();
            if (healthSlider != null) // if healthSlider is not empty, this tells teh player what Slider object is being used and on what gameObject  -Joseph Roberts
            {
                Debug.Log("HealthBar.cs has set its Health Slider variable to " + gameObject.GetComponent<Slider>() + " on " + gameObject.name);
            }
        }

        if (maxFloatHealth > 100 || maxFloatHealth < 0) // checks to see if the max health value is between 0 and 100 -Joseph Roberts
        {
            Debug.LogError("maxFloathealth pn HealthBar.cs on " + gameObject.name + " should be no more than 100 and no less than 0");
        }
        
        currentFloatHealth = maxFloatHealth;        // sets player health equal to max health variable  -Joseph Roberts
        Debug.Log("maxFloatHealth on HealthBar.cs component on " + gameObject.name + "gameObject is equal to " + maxFloatHealth);
        healthSlider.maxValue = maxFloatHealth;     // sets healthSlider max value equal to max health variable -Joseph Roberts
        healthSlider.value = maxFloatHealth; // sets healthSlider current value equal to max health variable -Joseph Roberts
    }
    #endregion

    #region Setting-Changing_Health_Functions
    private void SetHealth (float healthChangeFloat) // sets slider value of health bar
    {
        currentFloatHealth += healthChangeFloat;            // sets current health variable equal to received variable + the received healthChangeFloat variable -Joseph Roberts
        if (currentFloatHealth < 0) currentFloatHealth = 0; // resets current health variable to 0 if has dropped below 0 -Joseph Roberts
        if (currentFloatHealth > maxFloatHealth) currentFloatHealth = maxFloatHealth; // resets current health variable to max health variable if it has gone higher than the max health variable -Joseph Roberts
        healthSlider.value = currentFloatHealth;           // sets slider value equal to current health variable -Joseph Roberts
        // if (healthImage != null) healthImage.fillAmount += currentFloatHealth;     // checks to see if a health image has been referenced; if one has, then it sets its fillAmount equal to the current health variable -Joseph Roberts
        Debug.Log("SetHealth called on HealthBar.cs component on the " + gameObject.name + " gameObject; new current float health is " + currentFloatHealth);
    }
    
    public void DecreasePlayerFloatHp(float amountToDecreaseBy) // receives is amountToDecreaseBy from PlayerHealthManager.cs -Joseph Roberts
    {
        SetHealth(-(amountToDecreaseBy)); // calls the SetHealth method and passes the received AND NEGATED amountToDecreaseBy variable -Joseph Roberts
    }

    public void IncreasePlayerFloatHp(float amountToIncreaseBy) // receives is amountToIncreaseBy from PlayerHealthManager.cs -Joseph Roberts
    {
        SetHealth(amountToIncreaseBy); // calls the SetHealth method and passes the received amountToIncreaseBy variable -Joseph Roberts
    }
    #endregion
}
