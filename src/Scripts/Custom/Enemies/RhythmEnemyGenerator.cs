using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Script for Enemy Generator Specific for Rhythm Setup.
 * The Enemy generator will release enemy when reach the specific timing.
 * 
 * 
 * Contributors            Name             Github UserName
 *                         Zheng Yuan       YuanZheng01
 *                         Conor Peterson   conorpeterson97
 *                         Joseph Roberts   Techj70/jrobertsSCAD
 * 
 * Update History          Date             Important Changes 
 *                         2022.08.04       Script Created - Zheng Yuan
 *                         2022.08.14       Updating script - Conor Peterson
 *                         2022.08.23       Added to WaveModeUpdate() - Joseph Roberts
 */
public class RhythmEnemyGenerator : MonoBehaviour
{
    #region Attributes
    // Prefab enemy waves.
    [Tooltip("The Enemy Objects to Generate in order. Alive based on corresponding Wave Duration")] public List<GameObject> EnemyWaves;
    // Timing that corresponding enemy will appear.
    [Tooltip("Time offset that enemy will appear.")] public List<float> WaveTiming;
    // When reach the end of list, restart from start of list.
    public bool IsLooping;
    // Helpful for Rhythm timing enemy generate.
    public float BPM;
    public float timer;
    private int currentIndex;
    #endregion

    #region Unity Funtions
    // Start is called before the first frame update
    void Start()
    {
        RhythmModeInit();

        // Only needs for once.
        BPM = BPM / 60f;
    }

    // Update is called once per frame
    void Update()
    {
        WaveModeUpdate();
        timer += Time.deltaTime;
    }
    #endregion

    #region Functions
    public void RhythmModeInit()
    {
        if (EnemyWaves.Count != WaveTiming.Count)
        {
            Debug.Log("Some Enemy Wave Don't have Duration! Please Set Duration time correspondly");
        }
        //timer = 0;
        currentIndex = 0;
    }

    // Class for Normal Wave Mode.
    public void WaveModeUpdate()
    {
        // Pre-generate next wave enemy that hit the beat.
        // The timing with 2 decimal are equal to timing list.

        // You can calculate the Time needs for enemy to reach a precise position, and pre initialize the enemy.
        if ((WaveTiming[currentIndex] - Mathf.Round(timer * 100f) / 100f) <= 0) // 08-23-22 changed the == to <= to account for... 
                                                                    // ... numbers equal to and below 0; this was to ensure expected...
                                                                    // ... behavior on wave generation - Joseph Roberts
        {
            InitNextWave();
            timer = 0;                                              // resets the timer so the next wave can be timed - Joseph Roberts
        }
    }

    public void DismissCurrentWave()
    {
        // Might be useful to extend for some visual effects.(Like dis appear in some beats).
        // If not needs this feature, remove this.
    }

    public void InitNextWave()
    {
        // If all the list are covered.
        if (currentIndex >= EnemyWaves.Count)
        {
            if (IsLooping)
            {
                // Restart the Enemy List.
                //RhythmModeInit();
            }
        }
        else
        {
            // Set the prefab in preset position.
            Instantiate(EnemyWaves[currentIndex], new Vector3(transform.position.x, transform.position.y, transform.position.y), Quaternion.identity);
            Debug.Log(transform.position);
        }

        currentIndex++;
    }
    #endregion
}
