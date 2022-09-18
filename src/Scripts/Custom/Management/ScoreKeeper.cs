using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * script for keeping track of score in a scene; attached to Characters gameObject
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.08.02  Script Created - Joseph Roberts
 *                  
 */

public class ScoreKeeper : MonoBehaviour // attached to Score_Value child object of HUD  -Joseph Roberts
{
    #region Attributes
    public static int LevelScore = 0; // starting default score set to 0  -Joseph Roberts
    
    public TMP_Text scoreValue; // TMP component of Score_value reference set in the inspector  -Joseph Roberts
    #endregion

    #region Unity_Functions
    // Start is called before the first frame update
    void Start()
    {
        scoreValue = gameObject.GetComponent<TextMeshProUGUI>();
        
        if (scoreValue == null)
        {
            Debug.LogError("scoreValue TextMeshPro reference not defined on ScoreKeeper.cs");
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreValue.text = LevelScore.ToString("0000");
    }
    #endregion

    #region Score_Changing_Functions
    public static void IncreaseScore(int points)
    {
        LevelScore += points;
    }

    public static void DecreaseScore(int points)
    {
        LevelScore -= points;
    }
    #endregion
}
