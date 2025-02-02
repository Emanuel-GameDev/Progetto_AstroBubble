using UnityEngine;

/// <summary>
/// Keeps player movement scripts and playerInput script separated.
/// On Join a playerControllerPrefab will be instantiated as child of the playerInput;
/// that way we can have different types of players 
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("MOVEMENT")]
    
    [SerializeField]
    private float moveSpeed = 5f;

    
    [Header("ROTATION")]
    
    [SerializeField]
    private GameObject sightObject;
    
    [SerializeField, Tooltip("Offset to the rotation value \n(Ex. 1 * var = distance from player)")]
    private float sightDistanceOffset = .5f;
    
    [SerializeField]
    private bool useAlternativeRotation = false;
    [SerializeField]
    private GameObject alternativeSightObject;
    [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField] private float rotationInput;
    
    
    private Vector2 _moveDirection;

    private void Update()
    {
        // Move player
        transform.Translate(new Vector2(_moveDirection.x, _moveDirection.y) * (moveSpeed * Time.deltaTime));
        
        //Alternative rotation
        if (useAlternativeRotation)
        {
            alternativeSightObject.transform.Rotate(0f, 0f, -rotationInput * rotationSpeed * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// Called from the playerInputHandler, receives movement input
    /// </summary>
    /// <param name="movement"></param>
    public void OnMove(Vector2 movement)
    {
        // Set movement var
        _moveDirection = new Vector2(movement.x, movement.y);
    }
    
    /// <summary>
    /// Called from the playerInputHandler, receives rotation input
    /// </summary>
    /// <param name="rotation"></param>
    public void OnRotate(Vector2 rotation, bool canceled)
    {
        /*
         * if (rotation != Vector2.zero
         * ishooting = true;
         */
        
        if (!useAlternativeRotation)
        {
            Vector2 rotationVector = rotation.normalized * sightDistanceOffset;
            sightObject.transform.localPosition = rotationVector;
        }
        else
        {
            if (canceled)
                rotationInput = 0;
            else
                rotationInput = rotation.x;
        }
    }
}
