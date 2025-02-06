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
    private PlayerConfiguration _playerConfig;
    private BubbleGrabber _bubbleGrabber;
    
    private InputAction _moveAction;
    private InputAction _rotateAction;
    private InputAction _throwBubbleAction;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _bubbleGrabber = GetComponentInChildren<BubbleGrabber>();
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
        _rotateAction.Enable();
        
        _throwBubbleAction = _playerConfig.Input.actions.FindActionMap("Player").FindAction("ThrowBubble");
        _throwBubbleAction.performed += OnThrowBubble;
        _throwBubbleAction.Enable();
        
        // Change color in the "Visual" child of the prefab
        GetComponentInChildren<SpriteRenderer>().color = playerConfiguration.PlayerColor;

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
        _playerController.OnRotate(ctx.ReadValue<Vector2>());
    }
}
