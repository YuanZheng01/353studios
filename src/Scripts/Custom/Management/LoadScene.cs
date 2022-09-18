using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * script for loading scenes; is attached to buttons or other input getting gameObjects to call its functions to change scenes
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.08.02  Script Created - Joseph Roberts
 *                  
 */

public class LoadScene : MonoBehaviour
{
    #region Functions_Using_Build#
    public void ReturnToTitle()
    {
        SceneManager.LoadScene(0);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(1);
    }
    
    public void ReturnToCharacterSelection()
    {
        SceneManager.LoadScene(2);
    }
    
    public void ReturnToStageSelection()
    {
        SceneManager.LoadScene(3);
    }
    
    //public void Eula()
    //{
    //    SceneManager.LoadScene(#);
    //}
    #endregion

    public void ChangeSceneByName(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
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
}
