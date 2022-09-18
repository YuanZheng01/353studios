using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * script for pointing a 2D object towards the Player gameObject
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.08.25  Script Created - Joseph Roberts
 *                  
 */

public class PointAtPlayer : MonoBehaviour
{
    #region Attributes
    
    private Transform _transform;
    private Vector3 _rotation;
    private GameObject _player;
 
    #endregion

    #region Unity Functions
    
    // Start is called before the first frame update
    void Start()
    {
        _transform = gameObject.transform; // get gameObject Transform and store it as a reference - Joseph Roberts
        _player = GameObject.Find("Player"); // find the gameObject named Player and store it as a reference - Joseph Roberts
    }

    // Update is called once per frame
    void Update()
    {
        _rotation = _transform.position - _player.transform.position; // get the rotation to apply to the object by subtracting the player's position from the gameObject's position Vector3 - Joseph Roberts
        _transform.rotation = Quaternion.AngleAxis(((Mathf.Atan2(-_rotation.x, _rotation.y)*Mathf.Rad2Deg)), Vector3.forward); // change the transform.rotation of this gameObject by using the _rotation...
        // ... and set the forward reference to Vector3.forward - Joseph Roberts
    }
    
    #endregion
    
}
