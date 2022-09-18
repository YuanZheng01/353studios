using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
#if UNITY_EDITOR
#endif
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

/**
 * script for game management; would be attached to GameManager empty gameObject in a scene
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.08.01  Script Created - Joseph Roberts
 *                         2022.08.04  Edits to resolve automatic losing issue when game build is played - Joseph Roberts
 *                         2022.08.05  Enabled the CurrentIntHealth() function on PlayerHealthManger.cs and used it on in Update() to correct...
 *                                          ... bug (see https://github.com/Unexpected-Rogues/353Studios_ProjectRepo/issues/16); called by GameManager in Update() - Joseph Roberts
 *                         2022.08.25  added GameObject reference for pauseMenu and the Pause() function to activate it and stop the the game - Joseph Roberts
 */

public class GameManager : MonoBehaviour
{
    #region Attributes
    
    [Header("Level Variables")]
    
    [Tooltip("level time limit in second to pass to TimeKeeper.cs")]
    [SerializeField] public int levelTimeLimit;
    
    // values for generating/activating tier messages -Joseph Roberts
    [SerializeField] public int levelScoreLow; // use if you what the player to have to reach a minimum score to win; must also remove the /**/ comment flags in the first if statement in CheckTier() -Joseph Roberts
    [SerializeField] public int levelScoreMed;
    [SerializeField] public int levelScoreHigh;

    //public GameObject winWindowPrefab; // used for testing purposes -Joseph Roberts
    private GameObject winWindowInstance;

    [Header("Time Reference")]
    [NonSerialized] public TMP_Text timeValue;
    
    [NonSerialized] public TMP_Text finalLevelScore;

    [Header("Win Message Prefab References")]
    [Tooltip("reference variables for tier win message prefabs; set in the Inspector")]
    [SerializeField] public GameObject lowTierMessage;
    [SerializeField] public GameObject medTierMessage;
    [SerializeField] public GameObject highTierMessage;
    [SerializeField] public GameObject loseMessage;
    [SerializeField] public GameObject pauseMenu;
    
    public GameObject player;

    public AudioSource musicAudioSource;
    public AudioSource gameManagmentAudioSource;
    public AudioClip levelOverChime;

    [NonSerialized] public static bool paused; 
    
    [HideInInspector] public static bool levelOver;
    #endregion



    #region Unity_Functions
    private void Awake()
    {
        levelOver = false;
        
        if (Time.timeScale <= 0f)
        {
            Time.timeScale = 1f;
        }
        
    }


    // Start is called before the first frame update -Joseph Roberts
    void Start()
    {
        levelOver = false;
        lowTierMessage.SetActive(false);
        medTierMessage.SetActive(false);
        highTierMessage.SetActive(false);
        
        TimeKeeper.timeLimit = levelTimeLimit;

        if (levelScoreLow >= levelScoreMed || levelScoreLow >= levelScoreHigh)
        {
            Debug.LogError("score tiers do not have ascending values on " + gameObject.name + "; check the values of the levelScore_ variables ");
        }
        
        if (levelScoreMed <= levelScoreLow || levelScoreMed >= levelScoreHigh)
        {
            Debug.LogError("score tiers do not have ascending values on " + gameObject.name + "; check the values of the levelScore_ variables ");
        }
        
        if (levelScoreHigh <= levelScoreMed || levelScoreHigh <= levelScoreLow)
        {
            Debug.LogError("score tiers do not have ascending values on " + gameObject.name + "; check the values of the levelScore_ variables ");
        }

        ScoreKeeper.LevelScore = 0;
    }
    

    // Update is called once per frame -Joseph Roberts
    void Update()
    {
        if (levelOver == false)
        {
            if (player.GetComponent<PlayerHealthManager>().useIcons == true && player.GetComponent<PlayerHealthManager>().CurrentIntHealth() <= 0) // uses CurrentIntHealth() function rather than calling the value directly...
                                                                                                                                                   // ... to correct bug (see https://github.com/Unexpected-Rogues/353Studios_ProjectRepo/issues/16) -Joseph Roberts
            {
                Debug.Log("GameManager.cs sees icons are in use on PlayerHealthManager.cs and player int health is less than or equal 0");
                Lose(ScoreKeeper.LevelScore); // ... call Lose() function because the player has died -Joseph Roberts
            }
            
            else if (TimeKeeper.timeUp == true) // else if the time is up and the player has NOT died... -Joseph Roberts
            { 
                Win(ScoreKeeper.LevelScore); // ... call the Win() function -Joseph Roberts
            }
        }

        //else if (PlayerHealthManager.useBar == true && PlayerHealthManager.currentFloatHealth <= 0) // currently unused
        //{
        //  
        //}
    }
    #endregion
    
    public void Pause()
    {
        paused = !paused;
        Debug.Log("paused boolean on GameManager.cs of " + gameObject.name + " set to " + paused);
        if (paused == true && levelOver != true)
        {
            player.GetComponent<PlayerController>().enabled = false;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else if (paused != true && levelOver != true)
        {
            player.GetComponent<PlayerController>().enabled = true;
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
    }

    #region Win-Lose_Functions
    public void Win(int score)
    {
        int scoreTier = CheckTier(score); // checks the for what tier the player has achieved based on their score -Joseph Roberts
        if (scoreTier == 0) // if the player did not reach the minimum required score... -Joseph Roberts
        {
            Lose(ScoreKeeper.LevelScore); //... call the Lose() function instead -Joseph Roberts
            return;
        }

        levelOver = true;
        
        Debug.Log("Win() called on GameManager.cs and final score is equal to " + score);
        
        if (levelOverChime != null) gameManagmentAudioSource.PlayOneShot(levelOverChime);
        Time.timeScale = 0f; // stops time -Joseph Roberts
        switch (scoreTier)
        {
            case 1:
                lowTierMessage.SetActive(true);
                //lowTierMessage.GetComponent<MessageReferences>().finalScoreText;
                //finalLevelScore.text = score.ToString("0000");
                break;
            case 2:
                medTierMessage.SetActive(true);
                //finalLevelScore = medTierMessage.GetComponent<MessageReferences>().finalScoreText;
                //finalLevelScore.text = score.ToString("0000");
                break;
            case 3:
                highTierMessage.SetActive(true);
                //finalLevelScore = highTierMessage.GetComponent<MessageReferences>().finalScoreText;
                //finalLevelScore.text = score.ToString("0000");
                break;
        }
    }

    public void Lose(int score) // called when player loses all health -Joseph Roberts
    {
        levelOver = true;
        
        Debug.Log("Lose() called on GameManager.cs and final score is equal to " + score);
        
        if (levelOverChime != null) gameManagmentAudioSource.PlayOneShot(levelOverChime);
        Time.timeScale = 0f; //stops time
        loseMessage.SetActive(true);
        //finalLevelScore = loseMessage.GetComponent<MessageReferences>().finalScoreText;
        //finalLevelScore.text = score.ToString("0000");
    }

    public int CheckTier(int score) // checks the for what tier the player has achieved based on their score -Joseph Roberts
    {
        int tier = 0;
        
        if (levelScoreMed > score /*&& score >= levelScoreLow*/) // if score is equal to or greater than levelScoreLow but less than levelScoreMed... -Joseph Roberts
        {
            tier = 1;
            Debug.Log("score tier set to 1");
        }
        
        else if (levelScoreHigh > score && score >= levelScoreMed) // if score is equal to or greater than levelScoreMed but less than levelScoreHigh... -Joseph Roberts
        {
            tier = 2;
            Debug.Log("score tier set to 2");
        }
        
        else if (score >= levelScoreHigh) // if score is equal to or greater than levelScoreHigh... -Joseph Roberts
        {
            tier = 3;
            Debug.Log("score tier set to 3");
        }

        return tier;
    }
    #endregion
}