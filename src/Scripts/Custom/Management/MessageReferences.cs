using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * script for managing TextMeshProReferences for messages activated by the GameManager script for "game over"/"complete"; attached to the parent gameObject of messages
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.08.02  Script Created - Joseph Roberts
 *                  
 */

public class MessageReferences : MonoBehaviour
{
    #region Attributes
    [Header("References")]
    [SerializeField] public TextMeshProUGUI title; // reference the title text of the message -Joseph Roberts
    [SerializeField] public TextMeshProUGUI finalScoreText; // reference for the finalScoreText of the message; GameManger will use it to display level score -Joseph Roberts
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
}
