using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Script for Basic Enemy Generator.
 * The Enemy generator will release enemy when all enemy eliminated or wave time up.
 * 
 * 
 * Contributors            Name             Github UserName
 *                         Zheng Yuan       YuanZheng01
 * 
 * Update History          Date             Important Changes
 *                         2022.08.04       Script Created
 */
public class EnemyGenerator : MonoBehaviour
{
    #region Attributes
    // Prefab enemy waves.
    [Tooltip("The Enemy Objects to Generate in order. Alive based on corresponding Wave Duration")] public List<GameObject> EnemyWaves;
    // Wave Duration, how long the wave will stay.
    // Less than 0 will have no limitation on duration.
    [Tooltip("How long GameObject alive. Less than 0 will have infinite life span.")] public List<float> WaveDurations;
    // When reach the end of list, restart from start of list.
    public bool IsLooping;

    private GameObject currentWave;
    private float counter;
    private int currentIndex;
    #endregion

    #region Unity Funtions
    // Start is called before the first frame update
    void Start()
    {
        WaveModeInit();
    }

    // Update is called once per frame
    void Update()
    {
        WaveModeUpdate();
    }
    #endregion

    #region Functions
    public void WaveModeInit()
    {
        // For now, needs to make sure each Enemy Wave has Corresponding Duration time.
        if (EnemyWaves.Count != WaveDurations.Count)
        {
            Debug.Log("Some Enemy Wave Don't have Duration! Please Set Duration time correspondly");
        }

        currentIndex = 0;
        counter = 0;

        // Initialize the First wave.
        currentWave = Instantiate(EnemyWaves[0], Vector3.zero, Quaternion.identity);
    }

    // Class for Normal Wave Mode.
    public void WaveModeUpdate()
    {
        // If there are remain enemy wave, count for duation.
        // Could make it not destory the last wave.
        if (EnemyWaves[currentIndex] != null)
        {
            counter += Time.deltaTime;
        }

        // Generate next wave enemy when:
        // 1. Duration of Current Wave are end.
        // 2. All the enemy in current wave are elimnated.
        if ((WaveDurations[currentIndex] > 0 && counter > WaveDurations[currentIndex]) ||
            currentWave.transform.childCount <= 0)
        {
            DismissCurrentWave();
            counter = 0;
            InitNextWave();
        }
    }

    public void DismissCurrentWave()
    {
        // For now it's simply distory it.
        Destroy(currentWave);
        // If in future we could add animation, or lead current wave move to outside of screen, then distory.
    }

    public void InitNextWave()
    {
        currentIndex++;

        // If all the list are covered.
        if (currentIndex >= EnemyWaves.Count)
        {
            if (IsLooping)
            {
                // Restart the Enemy List.
                WaveModeInit();
            }
        } else
        {
            currentWave = Instantiate(EnemyWaves[currentIndex], Vector3.zero, Quaternion.identity);
        }
    }
    #endregion
}
