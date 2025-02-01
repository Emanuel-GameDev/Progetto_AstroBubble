using UnityEngine;

/// <summary>
/// Keeps player movement scripts and playerInput script separated.
/// On Join a playerControllerPrefab will be instantiated as child of the playerInput;
/// that way we can have different types of players 
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    
    private Vector2 _moveDirection;

    private void Update()
    {
        transform.Translate(new Vector2(_moveDirection.x, _moveDirection.y) * (moveSpeed * Time.deltaTime));
    }

    public void OnMove(Vector2 movement)
    {
        _moveDirection = new Vector2(movement.x, movement.y);
    }
}
