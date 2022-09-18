using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * script for closing the game
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.08.05  Script Created - Joseph Roberts
 *                  
 */

public class CloseGame : MonoBehaviour
{
    public void Close()
    {
        Application.Quit();
        Debug.Break();
    }
}
