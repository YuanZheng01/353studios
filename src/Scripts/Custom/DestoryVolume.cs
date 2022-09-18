using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * script for creating destroy volume to remove gameObjects
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.09.02  Script Created - Joseph Roberts
 *                  
 */

public class DestoryVolume : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("object entered DestroyVolume " + gameObject.name);
        if (other.gameObject.GetComponent<EnemyCollision>())
        {
            Destroy(other.gameObject);
        }
    }
}
