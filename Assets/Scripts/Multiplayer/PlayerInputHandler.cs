using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The first step of the inputAction output.
/// Every func connected to the inputAction will call the respective func on the dedicated script
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerWeaponHandler _playerWeaponHandler;
    private PlayerConfiguration _playerConfig;
    private BubbleGrabber _bubbleGrabber;
    
    private InputAction _moveAction;
    private InputAction _rotateAction;
    private InputAction _throwBubbleAction;

    private Vector2 _lastDirection;
    
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerWeaponHandler = GetComponentInChildren<PlayerWeaponHandler>();
        _bubbleGrabber = GetComponentInChildren<BubbleGrabber>();
        
        _lastDirection = Vector2.zero;  
    }


    /// <summary>
    /// Sets the configuration relative to a player and then prepares
    /// the script to receive the game inputs
    /// </summary>
    /// <param name="playerConfiguration"></param>
    public void InitializePlayer(PlayerConfiguration playerConfiguration)
    {
        _playerConfig = playerConfiguration;
                
        _moveAction = _playerConfig.Input.actions.FindActionMap("Player").FindAction("Move");
        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;
        _moveAction.Enable();
        
        _rotateAction = _playerConfig.Input.actions.FindActionMap("Player").FindAction("Rotate");
        _rotateAction.performed += OnRotate;
        _rotateAction.canceled += OnRotate;
        _rotateAction.Enable();
        
        _throwBubbleAction = _playerConfig.Input.actions.FindActionMap("Player").FindAction("ThrowBubble");
        _throwBubbleAction.performed += OnThrowBubble;
        _throwBubbleAction.Enable();
        
        // Change color in the "Visual" child of the prefab
        _playerController.SetVisualColor(_playerConfig.PlayerColor, true);
        _playerWeaponHandler.EquipWeapon(_playerConfig.StartingWeapon);

    }

    private void OnThrowBubble(InputAction.CallbackContext obj)
    {
        _playerController.StopTask(_bubbleGrabber.StopOnThrowDuration).Forget();
        _bubbleGrabber.ThrowBubble(_playerController.lastMoveDirection);
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        _playerController.OnMove(ctx.ReadValue<Vector2>());
    }

    private void OnRotate(InputAction.CallbackContext ctx)
    {
        Vector2 validDirection = GetValidDirection(ctx.ReadValue<Vector2>());
            
        _playerController.OnRotate(validDirection);
        _playerWeaponHandler.OnShoot(validDirection);
        
        _lastDirection = validDirection;
    }

    private Vector2 GetValidDirection(Vector2 newDir)
    {
        if (newDir == Vector2.zero) return Vector2.zero;
        
        float x = newDir.x == _lastDirection.x ? 0 : newDir.x;
        float y = newDir.y == _lastDirection.y ? 0 : newDir.y;

        if ((x,y) == (0,0))
            return _lastDirection;
        
        return new Vector2(x, y);
    }
}
