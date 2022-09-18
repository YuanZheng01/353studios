using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * script for opening player profiles on the Character Selection scene; attached to Characters gameObject
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.08.02  Script Created - Joseph Roberts
 *                         2022.08.18  added code to close/turn off profiles as others open - Joseph Roberts
 *                  
 */

public class CharacterProfilesOpen : MonoBehaviour
{
    #region Attributes
    [Tooltip("references to the character profiles used to open")]
    [SerializeField] List<GameObject> characterProfiles;
    /// <summary>
    /// holder for character profile gameObjects -Joseph Roberts
    /// </summary>
    public List<GameObject> CharProfileObjects
    {
        get => characterProfiles;
        set => characterProfiles = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    private int _lastOpenProfileIndex;
    #endregion

    #region Unity_Functions
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    public void OpenCharacterProfile(int profileIndexNum)
    {
        characterProfiles[_lastOpenProfileIndex].SetActive(false); // turns off last active profile gameObject
        characterProfiles[profileIndexNum].SetActive(true); // turn on choose profile gameObject
        _lastOpenProfileIndex = profileIndexNum; // sets last active profile to the profile gameObject that was just turned on
        Debug.Log("character profile " + characterProfiles[profileIndexNum] + " attempted to become active");
    }
}
