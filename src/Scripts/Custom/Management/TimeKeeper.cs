using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

/**
 * script for keeping track of time in a scene; attached to TimeKeepr gameObject
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.08.02  Script Created - Joseph Roberts
 *                  
 */

[RequireComponent(typeof(TextMeshProUGUI))]

public class TimeKeeper : MonoBehaviour
{
    #region Attributes
    private TMP_Text timeText;
    
    [NonSerialized] public static int timeLimit;
    
    public float time;
    private int timeSeconds;
    private int timeMinutes;
    
    
    private int timeRemaining;
    private int timeRemainingDisplay;
    

    [NonSerialized] public static bool timeUp; // checked by other scripts like GameManager to see if the time is up -Joseph Roberts
    #endregion
    
    public static void TimeUpTriggered()
    {
        timeUp = true;
    }

    public void ResetTime()
    {
        time = 0f;
        timeRemaining = timeLimit;
        timeUp = false;
    }

    #region Unity_Functions
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("timeLimit on TimeKeeper.cs of " + gameObject.name + " set to " + timeLimit + "; was received from GameManager.cs");
        timeLimit = FindObjectOfType<GameManager>().levelTimeLimit; // gets level time limit from GameManager; should only be one in scene  -Joseph Roberts
        timeText = GetComponent<TextMeshProUGUI>(); // gets the TextMeshPro component of the gameObject this script is attached to -Joseph Roberts
        time = 0f;
        timeRemaining = timeLimit;
        timeUp = false;
    }
    void Update()
    {
        if (timeUp == true) return;
        time += Time.deltaTime; // increment time by one second each call -Joseph Roberts
        timeSeconds = Mathf.RoundToInt(time);
        if (timeSeconds % 60 == 0)         // check to see if more than a minute has passed -Joseph Roberts
        {
            timeMinutes = timeSeconds/60; // divide timeSeconds by 60 to get timeMinutes and round the result -Joseph Roberts
        }

        timeRemaining = timeLimit - Mathf.RoundToInt(time);                                                    // determine total timeRemaining value as a whole number... -Joseph Roberts
        // Debug.Log("timePassed on TimeKeeper.cs of " + gameObject.name + " set to " + timePassed);
        timeRemainingDisplay = ((timeRemaining/60)*100)+(timeRemaining%60);
        timeText.text = timeRemainingDisplay.ToString("00:00");                 // ... then uses that number to display the time remaining on the text UI component -Joseph Roberts

        if (timeRemaining <= 0)
        {
            Debug.Log("time up triggered on TimeKeeeper.cs on " + gameObject.name);
            TimeUpTriggered();
        }
    }
    #endregion
}