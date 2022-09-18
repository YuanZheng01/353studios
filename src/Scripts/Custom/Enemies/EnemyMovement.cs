using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * This is the Script For Enemy Movement.
 * 
 * This allows the gameobject to move with specific pattern.
 * 
 *                  Name        Github UserName
 * Author           Zheng Yuan  YuanZheng01
 * 
 *                  Date        Important Changes
 * Update History   2022.07.29  Script Created
 *                  2022.07.30  Impelemented four basic movement:
 *                              1.Toward a GameObject.
 *                              2.Toward a Vector3 Position.
 *                              3.Toward a Vector3 Direction.
 *                              4.Follow a Given path.
 *                  
 */

// This Class Haven't Finish Yet!
// Will Update and Improve In Future Sprint When Detailed Functionality Decided.

// TODO: Remove Enemy That out of bound. Make sure new generated enemy won't get removed.
public class EnemyMovement : MonoBehaviour
{
    #region Attributes

    // The Target Gameobject to Reach.
    public GameObject targetObject;

    // The Current Target Position to Reach.
    public Vector3 targetPosition;

    // The Directly to Reach.
    public Vector3 targetDirection;

    // The GameObject with Player Tag.
    private GameObject _playerObject;

    // The Input movement speed of enemy.
    public float speed;

    // For SmoothDamp.
    [SerializeField] Vector3 velocity = Vector3.zero;

    // Stores the Input from Unity Interface.
    public List<Transform> path;

    [SerializeField] int currentPathIndex;

    // True to move looping the path.

    public bool _loopingPath;

    // If the Object chase Player.
    public bool _isChasePlayer;
    
    #endregion

    #region Unity Functions
    void Start()
    {
        // Initialize the speed to make sure it's valid.
        Speed = speed;
        currentPathIndex = 0;
        _playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // To Test and see the movement, run the movement one per time.
        if (_isChasePlayer)
        {
            MoveToPlayer();
        } else if (path.Count > 0)
        {
            MoveFollowPath();
        } else if (targetObject != null)
        {
            MoveToTargetObject();
        } else if (targetDirection != Vector3.zero)
        {
            MoveToTargetDirection();
        } else
        {
            MoveToTargetPosition();
        }
    }
    #endregion

    #region Functions


    #region Movement Functions
    // Use Movement Method Separately.
    public void MoveToTargetObject()
    {
        if (targetObject != null)
        {
            // Currently using MoveToward in Vector3. Could Change to SmoothDamp and Vector2 in future sprint if needed.
            transform.position = Vector3.MoveTowards(transform.position, targetObject.transform.position, speed * Time.deltaTime);
        }
    }

    public void MoveToTargetPosition()
    {
        if (targetPosition != null)
        {
            // Currently using MoveToward in Vector3. Could Change to SmoothDamp and Vector2 in future sprint if needed.
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    public void MoveToTargetDirection()
    {
        if (targetDirection != null)
        {
            // Currently using MoveToward in Vector3. Could Change to SmoothDamp and Vector2 in future sprint if needed.
            transform.position = Vector3.MoveTowards(transform.position, transform.position + targetDirection, speed * Time.deltaTime);
        }
    }

    public void MoveFollowPath()
    {
        // When the index out of bound.
        if (currentPathIndex >= path.Count)
        {
            // When reach the end of path, go to start of path.
            if (_loopingPath)
            {
                // Reset to first path index.
                currentPathIndex = 0;
            }
            return;
        }
        else {
            // Currently using MoveToward in Vector3. Could Change to SmoothDamp and Vector2 in future sprint if needed.
            transform.position = Vector3.MoveTowards(transform.position, path[currentPathIndex].position, speed * Time.deltaTime);
        }

        // When reach the current position, go for the next possible position.
        if (transform.position == path[currentPathIndex].position)
        {
            // Track to next Position.
            currentPathIndex++;
        }
    }

    public void MoveToPlayer()
    {
        if (_playerObject != null)
        {
            // Currently using MoveToward in Vector3. Could Change to SmoothDamp and Vector2 in future sprint if needed.
            transform.position = Vector3.MoveTowards(transform.position, _playerObject.transform.position, speed * Time.deltaTime);
        }
    }
    #endregion

    // Add the new position ot the end of path.
    // Could implement for a specific index.
    public void AddToPath(Transform transform)
    {
        path.Add(transform);
    }



    #endregion

    #region Getter && Setter
    // Getter && Setter
    public float Speed
    {
        get { return speed; }
        set { speed = Mathf.Abs(value); }
    }
    public Vector3 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }
    #endregion
}
