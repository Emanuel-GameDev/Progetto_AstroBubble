using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

/// <summary>
/// This script spawns the menu for every player that joins the game in the UI scene
/// </summary>
public class SpawnPlayerSetup : MonoBehaviour
{
    [SerializeField]
    private GameObject playerSetupPrefab;
    
    private PlayerInput _playerInput;
    private InputAction _inputAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        var rootMenu = GameObject.Find("RootMultiplayerPanel");
        if (rootMenu == null) return;
        
        var menu = Instantiate(playerSetupPrefab, rootMenu.transform);
        _playerInput.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
        
        menu.GetComponent<PlayerSetupController>().SetPlayerInput(_playerInput);
    }
}
