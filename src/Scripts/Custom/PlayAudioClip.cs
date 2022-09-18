using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * script for playing audio clips
 * 
 * Contributors            Name             Github UserName
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date        Important Changes
 *                         2022.08.27  Script Created - Joseph Roberts
 *                  
 */

public class PlayAudioClip : MonoBehaviour
{
   #region Attributes
   
   private AudioSource _sceneAudio;

   #endregion

   #region Unity Functions

   private void Start()
   {
      _sceneAudio = GameObject.Find("Background").GetComponent<AudioSource>();
   }

   #endregion

   #region Custiom Functions

   public  void PlayOneShot(AudioClip clip)
   {
      _sceneAudio.PlayOneShot(clip);
   }

   #endregion
   
}
