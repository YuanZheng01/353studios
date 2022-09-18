using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * script for healing pickups for the player; it mostly just holds the heal value while the PlayerHealthManagement script handles making the changes to the player health
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.07.28  Script Created - Joseph Roberts
 *                         2022.08.30  script updated with collision effects and functions - Joseph Roberts
 *                         
 */

[RequireComponent(typeof(Collider2D))] // makes this script require a gameObject to have to also have a Collider2D component before it can be added to that gameObject -Joseph Roberts
[RequireComponent(typeof(AudioSource))]

public class HP : MonoBehaviour
{
    #region Attributes
    public int intPlayerHeal = 1;
    public float floatPlayerHeal = 1f;

    private Transform _objectPosition;
    
    [SerializeField] [Tooltip("object collision effect")] public GameObject objectCollisionEffect;
    private GameObject _objectCollided;
    
    [SerializeField] [Tooltip("audio clip for object destruction")] public AudioClip objectDestroyed;
    
    [SerializeField] [Tooltip("time for the object to live before being destoryed")] public float lifetime;

    private GameObject _parentObject;
    
    private AudioSource _sceneAudio;

    private Coroutine _timeToLive;

    #endregion

    #region Unity_Functions
    // Start is called before the first frame update
    void Start()
    {
        
        gameObject.GetComponent<Collider2D>().isTrigger = true;

        _parentObject = this.gameObject;

        _sceneAudio = gameObject.GetComponent<AudioSource>();
        
        if (intPlayerHeal == 0)
        {
            Debug.LogError("HP.cs intHeal less than or equal to 0 on " + gameObject.name + " gameObject; set to 1 by default");
            intPlayerHeal = 1; 
        }
        
        if (floatPlayerHeal == 0f)
        {
            Debug.LogError("HP.cs floatHeal less than or equal to 0 on " + gameObject.name + " gameObject; set to 1f by default");
            floatPlayerHeal = 1f; 
        }

        _timeToLive = StartCoroutine(TimeToLive(lifetime, _parentObject));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        // The checker in Player Script Might Have Issue when Multiple Collision happens.
        // If needed, could refactor Player get hit to this Script.
        if (collider.name == "Player")
        {
            _objectPosition = gameObject.transform;               // records teh gameObject transform
            if (_objectCollided != null) Destroy(_objectCollided); // destroys the _enemyCollided gameObject if it already exists - Joseph Roberts 
            _objectCollided = Instantiate(objectCollisionEffect, _objectPosition.position, Quaternion.identity); // created collision effect at recorded gameObject transform.position - Joseph Roberts
            _sceneAudio.PlayOneShot(objectDestroyed, .5f);
            Destroy(this.gameObject); // enemy is destroyed after touching the player - Joseph Roberts
        }
    }
    
    IEnumerator TimeToLive(float timeToLive, GameObject hpObject)
        // destroys the enemy gameObject after its lifetime expires - Joseph Roberts
    {
        float time = 0; // sets the time variable  to 0; used to compare against the timeToLive variable for how much time has passed - Joseph Roberts

        while (time < timeToLive) // while the time variable is less than the timeToLive variable... - Joseph Roberts
        {
            time += Time.deltaTime;  // increase the time variable by amount of real-time pasted since last check - Joseph Roberts
            yield return null;       // complete the coroutine and return nothing back - Joseph Roberts
        }
        
        Destroy(hpObject); // destroy the stored enemyObject - Joseph Roberts
    }
    
    #endregion

}
