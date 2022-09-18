using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * script for level progression UI bar; is attached to DashTrial prefab
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.08.01  Script Created - Joseph Roberts
 *                         2022.08.03  added " + .5f" to currentValue in Update() to accomodate on level...
 *                                         ...end stopping time before the slider is full - Joseph Roberts
 */

[RequireComponent(typeof(Slider))]
public class LevelProgressionBar : MonoBehaviour
{
    #region Attributes
    private Slider progressionSlider;
    private float maxValue;
    private float currentValue;
    #endregion

    #region Unity_Functions
    // Start is called before the first frame update
    void Start()
    {
        progressionSlider = gameObject.GetComponent<Slider>();
        maxValue = FindObjectOfType<GameManager>().levelTimeLimit;
        progressionSlider.maxValue = maxValue;
        currentValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentValue = FindObjectOfType<TimeKeeper>().time;
        progressionSlider.value = currentValue + .5f; // adds .5f to ensure the bar is filled at end of time limit - Joseph Roberts
    }
    #endregion
}
