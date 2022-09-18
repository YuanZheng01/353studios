using System;

using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Numerics;
using System.Text;
using UnityEngine;
using InputSamples.Drawing.Test;
using InputSamples.Demo.Drawing.Test;

using UnityEngine.EventSystems;
using InputSamples.Controls;
using InputSamples.Drawing;
using Mono.CompilerServices.SymbolWriter;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

/**
 * script for touch player controls; is attached to Player gameObject in scene and relies on InputManager also being in the scene that has the TestPointerInputManager class attached to it
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.07.23  Script Created/Submitted - Joseph Roberts
 *                         2022.07.23  added checks for trail existence in MoveDashTrial() to prevent null error in console- Joseph Roberts
 *                         2022.08.07  added new MovePlayer() IEnumerator and other code for moving the player smoothly when dash is cooling down (i.e. not available)...
 *                                          ... to prevent failed collisions bug (see https://github.com/Unexpected-Rogues/353Studios_ProjectRepo/issues/13)  - Joseph Roberts
 *                         2022.08.25  added new _paused boolean to check for game being paused, and a check to see if a button is being pressed in OnPressed() - Joseph Roberts
 *                         2022.09.02  added code for sound clips - Joseph Roberts
 *                         2022.09.06  changed player MovePlayer() to use SmoothDamp instead of LERP to improve performance alleviate control clunkiness on builds  - Joseph Roberts

 */

public class PlayerController : MonoBehaviour
{
    #region Attributes
    // Reference to input manager.
    [SerializeField]
    private TestPointerInputManager inputManager; // creates a variable with the TestPointerInputManager class to handle input actions - Joseph Roberts
    
    // Camera with which our screen projections are performed.
    
    [SerializeField] private Camera renderCamera; // reference to the camera gameObject that is rendering the game scene to the player (likely the Main Camera...
                                                  // ... in the Hierarchy); used to determine pointer position - Joseph Roberts
    
    public GameObject player;  // reference to the player gameObject - Joseph Roberts

    private Transform _playerTransform;  // variable to hold the transform of the player gameObject - Joseph Roberts
    
    private Vector3 _lastKnownPlayerPosition;  // variable to record the last known position of the player gameObject - Joseph Roberts

    public GameObject dashTrail;  // reference to the prefab to instantiate (i.e. create a clone of) the dash trail when the player dashes - Joseph Roberts
    
    public GameObject dashReadyEffect;  // reference to the prefab to instantiate (i.e. create a clone of) when the dash is ready - Joseph Roberts

    private GameObject _dashCharged;

    public AudioClip dashReadySound;
    public AudioClip dashPerformedSound;

    public AudioSource sceneAudioSource; // to play sounds from the scene - Joseph Roberts

    public Image dashMeter; // reference to the visual meter to show the player when the dash is ready - Joseph Roberts

    [SerializeField] public float dashCooldown = 5f;  // the time in seconds it for the dash to cooldown (i.e. time it takes before the player can dash again) - Joseph Roberts

    [SerializeField] public float playerMoveSpeed = .1f; // the move speed of the player; should not be less than .1f to...
                                                         // ... prevent bug (see https://github.com/Unexpected-Rogues/353Studios_ProjectRepo/issues/13) - Joseph Roberts
    private Coroutine _movePlayer, _dashTrailMove;

    private Vector3 _velocity = Vector3.zero;

    private float _time = 0f; // time counter to compare against dashCooldown variable to determine when the dash is ready (see Update() function) - Joseph Roberts
    
    private float _uiBufferTimer = 0f;  // time counter to delay actions to allow for UI interactions - Joseph Roberts

    private bool _dashReady; // flag/boolean variable to indicate the status of the dash - Joseph Roberts

    private bool _pressed; // flag/boolean variable to record that a press was made - Joseph Roberts

    private bool _paused; // for storing the paused state of the game - Joseph Roberts
    
    #endregion


    protected virtual void OnEnable() // When the object this script is on is Enabled...
    {
        inputManager.Pressed += OnPressed;   //... set the inputManager variable's Pressed action to call the OnPressed() function in this script,... - Joseph Roberts
        inputManager.Dragged += OnDragged;   //... set the inputManager variable's Dragged action to call the OnDragged() function in this script,... - Joseph Roberts
        inputManager.Released += OnReleased; //... set the inputManager variable's Release action to call the OnReleased() function in this script,... - Joseph Roberts
    }

    protected virtual void OnDisable() // When the object this script is on is Disabled....
    {
        inputManager.Pressed -= OnPressed;   //... unset the inputManager variable's Pressed action to NOT call the OnPressed() function in this script,... - Joseph Roberts
        inputManager.Dragged -= OnDragged;   //... unset the inputManager variable's Dragged action to NOT call the OnDragged() function in this script,... - Joseph Roberts
        inputManager.Released -= OnReleased; //... unset the inputManager variable's Release action to NOT call the OnReleased() function in this script,... - Joseph Roberts
    }

    #region Unity_Functions

    // Start is called before the first frame update
    void Start()
    {
        dashMeter.fillAmount = 1; // sets dashMeter to ready/full

        if (playerMoveSpeed <= .1) // the move speed of the player should not be less than .1f to prevent bug (see https://github.com/Unexpected-Rogues/353Studios_ProjectRepo/issues/13) - Joseph Roberts
        {
            Debug.LogError("player move speed cannot be less than or equal to 0 on PlayerController component on " + gameObject.name + "; setting to default of .1f");
                                    // used for debug; checks to make sure player speed is not less than .1f - Joseph Roberts
                                    
            playerMoveSpeed = .1f;  // having the player speed any less than .1f can lead to OnTrigger events involving the player object to fail - Joseph Roberts
        }
        
        if (player == null) // checks to see if the player gameObject reference is empty on this script/component in the Inspector - Joseph Roberts 
        {
            Debug.LogError("no player object defined on the PlayerController component on " + gameObject.name + "; setting " + gameObject.name + " as the player gameObject");
            gameObject.GetComponent<GameObject>(); // set player gameObject to reference the gameObject his script is attached to - Joseph Roberts
        }

        if (dashTrail == null) // checks to see if the dash trail gameObject reference is empty on this script/component in the Inspector - Joseph Roberts
        {
            Debug.LogError("no dash trial object assigned/referenced on the PlayerController component on " + gameObject.name); // used for debug; log that the reference to...
                                                                                                                                        // ... dash trial is empty - Joseph Roberts
        }
        _pressed = true; // set default value of _pressed flag/boolean variable to ON (i.e TRUE) - Joseph Roberts

        _lastKnownPlayerPosition = player.transform.position; // set _lastKnownPlayerPosition variable equal to the current position of the referenced player gameObject - Joseph Roberts
    }

    // Update is called once per frame
    void Update()
    {
        if (_dashReady == false) // if the dash is not the dash is not ready yet, add time to the _time variable then check to see if it equals the dashCooldown variable - Joseph Roberts
        {
            _time += Time.deltaTime; // add the real time passed since the last frame to the _time variable - Joseph Roberts
            dashMeter.fillAmount = (_time/dashCooldown); // fill the dashMeter equal to the quotient of _time divided by the dashCooldown - Joseph Roberts
            if (_time > dashCooldown) // if _time is more than the dashCooldown then the cooldown is complete and teh dash is ready - Joseph Roberts
            {
                _dashReady = true; // set dash status to ready (i.e. TRUE)
                sceneAudioSource.PlayOneShot(dashReadySound,.5f);
                if (_dashCharged != null) Destroy(_dashCharged);
                _dashCharged = Instantiate(dashReadyEffect, player.transform.position,player.transform.rotation);
                _time = 0f; // reset the _time variable 
            }
        }

        _paused = GameManager.paused; // checks the GameManager object to see if the game has been paused then sets the local boolean to true or false - Joseph Roberts
    }
    #endregion

    #region Input_Getting_Functions
    private Vector3 GetWorldPosFromInput(PointerInput input, float z) // function that gets and returns the position of the pointer based on its location on the screen presented by...
                                                                      // ... the rendering camera (i.e. the camera showing the game scene to the player) - Joseph Roberts
    {
        return renderCamera.ScreenToWorldPoint(new Vector3(input.Position.x, input.Position.y, z));
    }
    
    private void OnPressed(PointerInput input, double time) // when a Pressed action is detected from the inputManager... - Joseph Roberts
    {
        // Debug.Log("OnPressed() called from PlayerController.cs on " + gameObject.name + "."); // used for debug; confirms that the PlayerController.cs detected a Pressed action...
                                                                                                      // ... and started its OnPressed() function - Joseph Roberts
        
        // Debug.Log("player position equal to " +  player.transform.position + "; last known position was " + _lastKnownPlayerPosition); // used for debug; confirms the current position...
                                                                                                                        // ... of the player gameObject and its last known position - Joseph Roberts
                                                                                                                        
        //while (_uiBufferTimer < .1f)
        //{
        //    Debug.Log("UI buffer active");
        //    _uiBufferTimer += Time.deltaTime;
        //}
        
        if (_paused == false && !EventSystem.current.IsPointerOverGameObject(input.InputId)) // checks to make sure the game is not paused and that a gameObject is not being pressed - Joseph Roberts
        {
            _uiBufferTimer = 0f;

            if (_pressed == false && _lastKnownPlayerPosition != GetWorldPosFromInput(input, 4f) && _dashReady == true) // if the current position of the pointer input is different from...
                                                                                                                        // ... the recorded player gameObject position and the dash is ready,..
                                                                                                                        // ... then player is "dashing" - Joseph Roberts
            {
                _lastKnownPlayerPosition = player.transform.position; // set _lastKnownPlayerPosition variable equal to the current position of the referenced player gameObject - Joseph Roberts
                Dash(input); // performs a dash - Joseph Roberts
            }

            else if (_pressed == false && _lastKnownPlayerPosition != GetWorldPosFromInput(input, 4f) && _dashReady == false) // if the current position of the pointer input is different from...
                                                                                                                            // ... the recorded player gameObject position and the dash is NOT ready,..
                                                                                                                            // ... then player is "moving" - Joseph Roberts
            {
                Move(input); // moves the player - Joseph Roberts
            }

            _pressed = true; // set value of _pressed flag/boolean variable to ON (i.e TRUE) to indicate there has been a press; this used in OnRelease() to make sure it only performs its action AFTER...
                             // ... a Pressed action was detected - Joseph Roberts
        }
    }
    
    private void OnDragged(PointerInput input, double time) // called when a Dragged action is detected from the inputManager - Joseph Roberts
    {
        //Debug.Log("OnDragged called from PlayerController.cs on " + gameObject.name + "."); // used for debug; confirms that the PlayerController.cs detected a Dragged action and started...
                                                                                                    // ... its OnDragged() function - Joseph Roberts
        Move(input); // move the player - Joseph Roberts
    }
    
    private void OnReleased(PointerInput input, double time) // called when a Released action is detected from the inputManager - Joseph Roberts
    {
        _lastKnownPlayerPosition = player.transform.position;  // sets the _lastKnownPlayerPosition variable to the current position of the player gameObject storing its value to help determine...
                                                               // ... when a dash occurs in the OnPressed() function - Joseph Roberts

        if (_pressed == false!) // checks to see if the _pressed flag/boolean variable is not ON (i.e set to FALSE) - Joseph Roberts
        {
            // Debug.Log("OnRelease called from PlayerController.cs on " + gameObject.name + ", but _pressed was FALSE"); // used for debug; confirms that the PlayerController.cs detected...
                                                                                                            // ... a Released action, but that there was NOT a Pressed action before it - Joseph Roberts
            // Probably caught by UI, or the input was otherwise lost 
            return; // returns from this function and does not execute any more of it - Joseph Roberts
        }
        
        // Debug.Log("OnRelease called from PlayerController.cs on " + gameObject.name + ", and _pressed was TRUE"); // used for debug; confirms that the PlayerController.cs detected a Released...
                                                                                                                  // ... action AFTER a Pressed action and started its OnReleased() function - Joseph Roberts
                                                                                                                  
        // Debug.Log("last known player position  is equal to " + _lastKnownPlayerPosition);  // used for debug; confirms what the PlayerController.cs stored the last known player position...
                                                                                                  // ... in the _lastKnownPlayerPosition variable - Joseph Roberts
                                                                                                                  
        _pressed = false;  // set value of _pressed flag/boolean variable to OFF (i.e FALSE) to reset it and indicate the pressed action has been stopped - Joseph Roberts
    }
    #endregion

    #region Moving_&_Dashing_Functions
    private void Dash(PointerInput pointerInput) // function for performing the dash action - Joseph Roberts
    {
        if (_movePlayer != null) StopCoroutine(_movePlayer); // if the _movePlayer coroutine is already started/populated this stops it to prepare to start it again with new information - Joseph Roberts
        
        // player.transform.position = GetWorldPosFromInput(pointerInput, 4f); // sets the player gameObject position to the same position of the pointer on the screen - Joseph Roberts
        
        player.transform.position = Vector3.SmoothDamp(player.transform.position, GetWorldPosFromInput(pointerInput, 4f), ref _velocity, 0f); // uses SmoothDamp() to 
        sceneAudioSource.PlayOneShot(dashPerformedSound,.5f);
        
        // Debug.Log("player is dashing"); // used for debug; confirms that the PlayerController.cs is performing a dash action - Joseph Roberts
                                                
        var newDashTrail = Instantiate(dashTrail, _lastKnownPlayerPosition, Quaternion.identity); // creates a new dashTrail instance/clone at the _lastKnownPlayerPosition using the referenced...
                                                                                                            // ... prefab in the Inspector - Joseph Roberts
                                                                                                            
        // Debug.Log("dash trail started at position " + _lastKnownPlayerPosition); // used for debug; confirms where teh dashTrail was crated at - Joseph Roberts
        
        //if (_dashTrailMove != null) StopCoroutine(_dashTrailMove); /* if the _dashTrailMove coroutine is already started/populated this stops it to prepare to start it again with new information - Joseph Roberts*/
        _dashTrailMove = StartCoroutine(MoveDashTrail(.1f, newDashTrail, _lastKnownPlayerPosition, player.transform.position)); // uses the MoveDashTrail coroutine to smoothly move...
                                                                                                            // ... (i.e. LERP) the new dash trail from its original location to the player gameObject's position...
                                                                                                            // ... (the float variable in this call is duration of the move in seconds [i.e. how long does it...
                                                                                                            // ... take to complete the move]) - Joseph Roberts

                                                                                                            _dashReady = false; // sets the _dashReady variable to FALSE so the dash cooldown can start on the Update() function - Joseph Roberts
        dashMeter.fillAmount = 0; // empties dash meter; increases as remaining cooldown decreases - Joseph Roberts
    }
    
    private void Move(PointerInput pointerInput) // function for moving the player gameObject - Joseph Roberts
    {
        // Debug.Log("player is moving"); // used for debug; confirms that the PlayerController.cs is moving the player gameObject - Joseph Roberts

        if (_movePlayer != null) StopCoroutine(_movePlayer); // if the _movePlayer coroutine is already started/populated this stops it to prepare to start it again with new information - Joseph Roberts

        //while (player.transform.position != GetWorldPosFromInput(pointerInput, 4f))
        //{
        //    player.transform.position = Vector3.SmoothDamp(player.transform.position, GetWorldPosFromInput(pointerInput, 4f), ref _velocity, playerMoveSpeed);
        //}

        _movePlayer = StartCoroutine(MovePlayer(pointerInput,playerMoveSpeed, player, player.transform.position, GetWorldPosFromInput(pointerInput, 4f))); // ... uses the MovePlayer coroutine to...
        // ... smoothly move (i.e. SmoothDamp) the player gameObject from its original location to the pointer input's...
        // ... position (the float variable in this call is the speed of the move in seconds [i.e. how long does...
        // ... it take to complete the move]) - Joseph Roberts
    }

    IEnumerator MoveDashTrail(float duration, GameObject trail, Vector3 lastKnownPlayerPosition, Vector3 playerPosition) // when called, takes a float number/value for the duration of move, the gameObject to move,...
                                                                                                                         // ... the position where the gameObject move is starting from, and the position to move...
                                                                                                                         // ... the gameObject to - Joseph Roberts
    {
        float time = 0; // sets the time variable used to compare how much time has passed to the duration variable to 0 - Joseph Roberts

        while (time < duration) // while the time variable is less than the duration variable... - Joseph Roberts
        {
            if (trail != null)  // LERP move the trail gameObject from its starting position to the its new position by the ratio...
            {                   // ... of the time variable divided by the duration variable - Joseph Roberts
                trail.transform.LookAt(playerPosition, Vector3.up);
                trail.transform.position = Vector3.Lerp(lastKnownPlayerPosition, playerPosition, time / duration);
            }                                                                                                                      
                                                                                                                                  
            time += Time.deltaTime;  // update the time variable by adding the amount of real time that has passed sense the last frame - Joseph Roberts
            yield return null;       // complete the coroutine and return nothing back - Joseph Roberts
        }

        if (trail != null) trail.transform.position = playerPosition; // makes sure the position of trail gameObject is equal to the intended end position of the move - Joseph Roberts
    }
    
    IEnumerator MovePlayer(PointerInput pointerInput, float duration, GameObject playerObject, Vector3 playerPosition, Vector3 newPosition) // when called, takes a float number/value for the duration of move, the gameObject to move,...
                                                                                                                 // ... the position where the gameObject move is starting from, and the position to move...
                                                                                                                 // ... the gameObject to - Joseph Roberts
    {
        //  float time = 0; // sets the time variable used to compare how much time has passed to the duration variable to 0 - Joseph Roberts

        //  while (time < duration) // while the time variable is less than the duration (i.e. player move speed) variable... - Joseph Roberts
        while (playerPosition != GetWorldPosFromInput(pointerInput, 4f))
        {
        //  playerObject.transform.position = Vector3.Lerp(playerPosition, newPosition, time / duration); 
        //  time += Time.deltaTime;  // update the time variable by adding the amount of real time that has passed sense the last frame - Joseph Roberts
            
            player.transform.position = Vector3.SmoothDamp(player.transform.position, newPosition, ref _velocity, playerMoveSpeed);
            yield return null;       // complete the coroutine and return nothing back - Joseph Roberts
        }

        player.transform.position = newPosition; // makes sure the position of player gameObject is equal to the intended end position of the move - Joseph Roberts
    }
    #endregion
}
