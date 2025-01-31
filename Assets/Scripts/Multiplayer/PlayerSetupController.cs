using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// This scripts holds the data of the player's menu
/// </summary>
public class PlayerSetupController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI indexText;
    [SerializeField]
    private TextMeshProUGUI controlSchemeText;
    
    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private Button readyButton;

    private PlayerInput _playerInput;
    private int _playerIndex;
    private InputAction _inputAction;

    private void OnDisable()
    {
        _inputAction.performed -= OnSubmit;
        _inputAction.Disable();
    }
    
    private void OnSubmit(InputAction.CallbackContext obj)
    {
        ReadyPlayer();
    }

    public void SetPlayerInput(PlayerInput playerInput)
    {
        _playerInput = playerInput;
        
        _inputAction = _playerInput.actions.FindActionMap("UI").FindAction("Submit");
        _inputAction.performed += OnSubmit;
        _inputAction.Enable();
        
        SetPlayerTexts();
    }
    
    private void SetPlayerTexts()
    {
        string controlScheme = _playerInput.currentControlScheme;
        _playerIndex = _playerInput.playerIndex;
        
        indexText.SetText($"Player {_playerIndex + 1}");
        controlSchemeText.SetText($"Control Scheme:\n{controlScheme}");
    }

    private void ReadyPlayer()
    {
        PlayerConfigurationManager.Instance.SetReadyPlayer(_playerIndex);
        readyButton.gameObject.SetActive(false);
    }
}
