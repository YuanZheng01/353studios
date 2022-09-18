using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * script for simple smooth LERP animation/gameObject moving
 * 
 * Contributors     Name            Github UserName
 *                  Joseph Roberts  Techj70/jrobertsSCAD
 * 
 * Update History   Date        Important Changes
 *                  2022.08.05  Script Created - Joseph Roberts
 *                  2022.09.04  moved _moving coroutine creation to Awake() function - Joseph Roberts 
 */

public class SmoothMove : MonoBehaviour
{
    # region #Attributes
    
    [Tooltip("time it will take to complete the move in seconds")] 
    [SerializeField] private float timeToMoveIn;

    //[Tooltip("object or subject to be moved")] [SerializeField]
    private GameObject objectToMove;

    [Tooltip("object to be used for end position reference")] 
    [SerializeField] private GameObject objectRefForEndPosition;

    private Coroutine _moving;
    
    #endregion

    #region Unity_Functions

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        objectToMove = gameObject;
        _moving = StartCoroutine(Move(timeToMoveIn, objectToMove, objectToMove.transform.position, objectRefForEndPosition.transform.position));
    }
    
    // Update is called once per frame
    void Update()
    {
    }
    
    #endregion

    #region Moving_Functions

    IEnumerator Move(float duration, GameObject objectBeingMoved, Vector3 startPosition, Vector3 endPosition) // when called, takes a float number/value for the duration...
                                                                                            // ... of move, the gameObject to move, the position where the gameObject move...
                                                                                            // ... is starting from, and the position to move the gameObject to - Joseph Roberts
    {
        while (gameObject.activeSelf == true)
        {
            float
                time = 0; // sets the time variable used to compare how much time has passed to the duration variable to 0 - Joseph Roberts

            while (time < duration) // while the time variable is less than the duration variable... - Joseph Roberts
            {
                objectBeingMoved.transform.position = Vector3.Lerp(startPosition, endPosition, time / duration); // LERP move the objectToMove gameObject from...
                                                                                        // ... its starting position to the its new position by the ratio of the time...
                                                                                        // ... variable divided by the duration variable - Joseph Roberts
                time += Time.deltaTime; // update the time variable by adding the amount of real time that has passed sense the last frame - Joseph Roberts
                yield return null; // complete the coroutine and return nothing back - Joseph Roberts
            }

            objectBeingMoved.transform.position = endPosition; // makes sure the position of objectBeingMoved gameObject is equal to the intended end position...
                                                               // ... of the move - Joseph Roberts
        }
    }
    
    #endregion
}
