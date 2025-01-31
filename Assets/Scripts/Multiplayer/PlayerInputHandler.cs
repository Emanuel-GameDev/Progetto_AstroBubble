using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The first step of the inputAction output.
/// Every func connected to the inputAction will call the respective func on the dedicated script
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private Vector2 startPos;

    private InputAction _inputAction;
    
    private PlayerController _playerController;
    private PlayerConfiguration _playerConfig;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }


    /// <summary>
    /// Sets the configuration relative to a player and then prepares
    /// the script to receive the game inputs
    /// </summary>
    /// <param name="playerConfiguration"></param>
    public void InitializePlayer(PlayerConfiguration playerConfiguration)
    {
        _playerConfig = playerConfiguration;
                
        _inputAction = _playerConfig.Input.actions.FindActionMap("Player").FindAction("Move");
        _inputAction.performed += OnMove;
        _inputAction.canceled += OnMove;
        _inputAction.Enable();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            _playerController.OnMove(ctx.ReadValue<Vector2>());
        else if (ctx.canceled)
            _playerController.OnMove(Vector2.zero);
    }
}
