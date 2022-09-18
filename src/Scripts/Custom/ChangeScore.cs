using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * script for adding points
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.09.02  Script Created - Joseph Roberts
 */

public class ChangeScore : MonoBehaviour
{
    [SerializeField] [Tooltip("how many points to added to score")] public int pointValue;

    [SerializeField] [Tooltip("add or substract points")] public Boolean onForAdd_OffForSubtract;

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

    #region Point_Functions
    
    public void AddPoints()
    {
        ScoreKeeper.IncreaseScore(pointValue);
    }
    
    public void SubstractPoints()
    {
        ScoreKeeper.DecreaseScore(pointValue);
    }
    
    #endregion

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && onForAdd_OffForSubtract == true)
        {
            AddPoints();
        }

        else if (other.gameObject.name == "Player" && onForAdd_OffForSubtract == false)
        {
            SubstractPoints();
        }
    }
}
