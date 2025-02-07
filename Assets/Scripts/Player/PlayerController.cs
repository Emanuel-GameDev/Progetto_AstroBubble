using Cysharp.Threading.Tasks;
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
    
    public Vector2 lastMoveDirection;
    
    private Vector2 _moveDirection;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _headSpriteRenderer;
    private bool _isFacingRight = true;
    private bool _canMove = true;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _headSpriteRenderer = _spriteRenderer.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Move player
        if (_canMove)
            transform.Translate(new Vector2(_moveDirection.x, _moveDirection.y) * (moveSpeed * Time.deltaTime));
        
        if (_moveDirection.x < 0 && _isFacingRight)
            Flip();
        if (_moveDirection.x > 0 && !_isFacingRight)
            Flip();
    }
    
    /// <summary>
    /// Called from the playerInputHandler, receives movement input
    /// </summary>
    /// <param name="movement"></param>
    public void OnMove(Vector2 movement)
    {
        // Set movement var
        _moveDirection = new Vector2(movement.x, movement.y);
        lastMoveDirection = _moveDirection;
    }

    /// <summary>
    /// Called from the playerInputHandler, receives rotation input
    /// </summary>
    /// <param name="rotation"></param>
    public void OnRotate(Vector2 rotation)
    {
        /*
         * if (rotation != Vector2.zero
         * ishooting = true;
         */
        if (rotation.x != 0 && rotation.y != 0) return;
        
        Vector3 newPosition = new Vector3(rotation.x, rotation.y, 0) * sightDistanceOffset;
        sightObject.transform.localPosition = newPosition;
    }
    
    
    private void Flip()
    {
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
        _headSpriteRenderer.flipX = !_headSpriteRenderer.flipX;
        _isFacingRight = !_isFacingRight;
    }

    public void SetVisualColor(Color color, bool onlyHead)
    {
        if (onlyHead)
            _headSpriteRenderer.color = color;
        else
        {
            _spriteRenderer.color = color;
            _headSpriteRenderer.color = color;
        }
    }

    public async UniTask StopTask(float duration)
    {
        _canMove = false;

        await UniTask.Delay((int)(duration * 1000));
        
        _canMove = true;
    }
}
