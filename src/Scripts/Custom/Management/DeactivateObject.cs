using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * script for deactivating an object
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.08.05  Script Created - Joseph Roberts
 *                  
 */

public class DeactivateObject : MonoBehaviour
{
    public void SetThisObjectToInactive()
    {
        this.gameObject.SetActive(false);
    }
}
