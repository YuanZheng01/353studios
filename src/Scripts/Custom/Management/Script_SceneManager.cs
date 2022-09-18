using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * script for handling scene management; can be used for a pause menu or in other places; not currently used
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.08.01  Script Created - Joseph Roberts
 *                         2022.09.03  Script updated with Async and Reload function; changed name of some functions...
 *                                          ... to fit standard naming convention - Joseph Roberts
 * 
 */

public class Script_SceneManager : MonoBehaviour
{
    #region Attributes
    //An inspector-viewable field for the Pause Menu game object -Joseph Roberts
    //Will need to restrict its activation to the Main Menu -Joseph Roberts
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject pauseButton;
    #endregion

    #region Scene_Managment_Functions
    //A custom function that will tell the Scene Manager to load the next scene listed -Joseph Roberts
    //in the Build Index (Starting at 0 [Scene_Title], Loading to 1 [SampleScene]) -Joseph Roberts
    public void PlayGame()
    {
        //Tell Scene Manager to load scene at build index 4; check Build Settings for scene index numbers -Joseph Roberts
        //Single Load mode closes all other active scenes before loading this one -Joseph Roberts
        SceneManager.LoadSceneAsync(4, LoadSceneMode.Single);
    }

    public void PauseGame()
    {
        //When button this function is assigned to is pressed, the pause menu scene -Joseph Roberts
        //will become active
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);

        //Set the timescale of the game to 0 to "pause" the game -Joseph Roberts
        //No game objects should move in this timeScale -Joseph Roberts
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        //When button this function is assigned to is pressed, the pause menu scene -Joseph Roberts
        //will become inactive -Joseph Roberts
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);

        //Set the timescale of the game to 1 to "unpause" the game -Joseph Roberts
        //Game objects should move in this timeScale -Joseph Roberts
        Time.timeScale = 1f;
    }


    public void ReturnToMainMenu() 
    {
        //Set the timescale of the game to 1 to "unpause" the game -Joseph Roberts
        //Game objects should move in this timeScale -Joseph Roberts
        Time.timeScale = 1f;

        //Tell Scene Manager to load scene at build index 1; check Build Settings for scene index numbers -Joseph Roberts
        //Single Load mode closes all other active scenes before loading this one  -Joseph Roberts
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }
    
    public void ChangeSceneByName(string sceneName) 
    {
        //Set the timescale of the game to 1 to "unpause" the game -Joseph Roberts
        //Game objects should move in this timeScale -Joseph Roberts
        Time.timeScale = 1f;

        //Tell Scene Manager to load scene at build index 1; check Build Settings for scene index numbers -Joseph Roberts
        //Single Load mode closes all other active scenes before loading this one  -Joseph Roberts
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
    
    public void ReloadCurrent()
    {
        if (Time.timeScale <= 0f)
        {
            Time.timeScale = 1f;
        }
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadSceneAsync(currentScene);
    }
    
    #endregion
}