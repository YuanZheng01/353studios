using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * script for keep track of score in a scene; attached to Characters gameObject
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.08.02  Script Created - Joseph Roberts
 *                  
 */

public class HealthIcons: MonoBehaviour
{
    #region Attributes
    [NonSerialized] public int currentIntHealth; // player health variable -Joseph Roberts

    [NonSerialized] public int maxIntHealth; // max player health variable -Joseph Roberts

    // public TMP_Text healthText; // old reference used for simple number display of remaining HP -Joseph Roberts
    
    [Tooltip("references to the icons used to represent HP; MAX of 5 allowed")]
    [SerializeField] List<GameObject> hpIcons;
    /// <summary>
    /// holder for HP icons 
    /// </summary>
    public List<GameObject> HpIconObjects
    {
        get => hpIcons;
        set => hpIcons = value ?? throw new ArgumentNullException(nameof(value));
    }
    #endregion

    #region Unity_Functions
    private void Awake()
    {
        if (hpIcons.Count > 5) // checks to see if the hpIcons list has more than five items (i.e. gameObjects) -Joseph Roberts
        {
            Debug.LogError("ERROR; size limit for Hp Icons list on the HealthIcons.cs component on the " + gameObject.name + "gameObject is 5");
            Debug.Break();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentIntHealth = maxIntHealth;   // sets player health equal to max health variable -Joseph Roberts
        Debug.Log("maxIntHealth on HealthIcon.cs component on " + gameObject.name + "gameObject is equal to " + maxIntHealth);
        if (maxIntHealth != hpIcons.Count) // checks to if the max health variable is equal to how many hpIcons are available to use -Joseph Roberts
        {
            Debug.LogError("ERROR; max health on HealthIcons.cs component of " + gameObject.name + " gameObject is not equal to the number of HP icons available on the Hp Icons list; change max health on PlayerHealthManager.cs and/or the number of HP icons available in the list");
            Debug.Break();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Setting-Changing_Health_Functions
    private void CheckPlayerHealth(int caseValue)
    {
        foreach (var hpIcon in hpIcons) // resets all health icons in the hpIcons list to OFF (i.e. isActive(false)) -Joseph Roberts
        {
            hpIcon.SetActive(false);
        }
        
        switch (caseValue) // checks the received health value and performs the corresponding case below to turn ON the correct hpIcons -Joseph Roberts
            {
                case 0:
                {
                    Debug.Log("all icons/HP lost on " + gameObject.name + " on the HealthIcons.cs component");
                    break;
                }
                case 1:
                {
                    hpIcons[0].SetActive(true);
                    break;
                }
                
                case 2:
                {
                    hpIcons[0].SetActive(true);                    
                    hpIcons[1].SetActive(true);
                    break;
                }
                
                case 3:
                {
                    hpIcons[0].SetActive(true);
                    hpIcons[1].SetActive(true);
                    hpIcons[2].SetActive(true);
                    break;
                }
                
                case 4:
                {
                    hpIcons[0].SetActive(true);
                    hpIcons[1].SetActive(true);
                    hpIcons[2].SetActive(true);                    
                    hpIcons[3].SetActive(true);
                    break;
                }
                
                case 5:
                {
                    hpIcons[0].SetActive(true);
                    hpIcons[1].SetActive(true);
                    hpIcons[2].SetActive(true);
                    hpIcons[3].SetActive(true);
                    hpIcons[4].SetActive(true);
                    break;
                }
            }
    }
    
    private void SetIconHealth (int healthChangeInt) // sets slider value of health bar -Joseph Roberts
    {
        currentIntHealth += healthChangeInt;            // sets current health variable equal to received variable + the received healthChangeFloat variable -Joseph Roberts
        if (currentIntHealth < 0) currentIntHealth = 0; // resets current health variable to 0 if has dropped below 0 -Joseph Roberts
        if (currentIntHealth > maxIntHealth) currentIntHealth = maxIntHealth; // resets current health variable to max health variable if it has gone higher than the max health variable -Joseph Roberts
        Debug.Log("SetHealth called on HealthIcons.cs component on the " + gameObject.name + " gameObject; new current health is " + currentIntHealth);
        CheckPlayerHealth(currentIntHealth); // calls the CheckPlayerHealth method and passes the current health variable -Joseph Roberts
    }
    
    public void DecreasePlayerIntHp(int amountToDecreaseBy) // receives is amountToDecreaseBy from PlayerHealthManager.cs -Joseph Roberts
    {
        SetIconHealth(-(amountToDecreaseBy)); // calls the SetHealth method and passes the received AND NEGATED amountToDecreaseBy variable -Joseph Roberts
    }
    
    public void IncreasePlayerIntHp(int amountToIncreaseBy) // receives is amountToIncreaseBy from PlayerHealthManager.cs -Joseph Roberts
    {
        SetIconHealth(amountToIncreaseBy); // calls the SetHealth method and passes the received amountToIncreaseBy variable -Joseph Roberts
    }
    #endregion
}