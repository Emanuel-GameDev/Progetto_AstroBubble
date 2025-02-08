using System.Collections.Generic;
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

    [SerializeField] private List<GameObject> playerPresets;

    private PlayerInput _playerInput;
    private int _playerIndex;
    private InputAction _submitAction;
    private InputAction _navigationAction;
    private int _currentPresetID;

    private bool _presetSelected; 
    
    public void SetPlayerInput(PlayerInput playerInput)
    {
        _playerInput = playerInput;
        
        _submitAction = _playerInput.actions.FindActionMap("UI").FindAction("Submit");
        _submitAction.performed += OnSubmit;
        _submitAction.Enable();
        
        _navigationAction = _playerInput.actions.FindActionMap("UI").FindAction("Navigate");
        _navigationAction.performed += OnNavigate;
        _navigationAction.Enable();
        
        SetPlayerTexts();
        SetupPresets();
    }

    private void OnDisable()
    {
        _submitAction.performed -= OnSubmit;
        _submitAction.Disable();
        
        _navigationAction.performed -= OnNavigate;
        _navigationAction.Disable();
    }
    
    private void OnSubmit(InputAction.CallbackContext obj)
    {
        if (!_presetSelected)
        {
            var config = PlayerConfigurationManager.Instance.GetPlayerConfig(_playerIndex);
            var defaultWeapons = PlayerConfigurationManager.Instance.DefaultSettings;
            
            // Color
            Color colorSelected = playerPresets[_currentPresetID].GetComponentInChildren<Button>().colors.normalColor;
            config.SetColor(colorSelected);
            
            // Weapon
            BaseWeapon weaponSelected = defaultWeapons[_currentPresetID].DefaultWeapon;
            config.SetStartingWeapon(weaponSelected);
            
            _presetSelected = true; 
            _navigationAction.Disable();
        }
        else
            ReadyPlayer();
    }

    private void OnNavigate(InputAction.CallbackContext obj)
    {
        if (!obj.performed || playerPresets.Count == 0) return;
        
        Vector2 inputValue = obj.ReadValue<Vector2>();
        if (inputValue.x > 0)
        {
            _currentPresetID = (_currentPresetID + 1) % playerPresets.Count;
        }
        else if (inputValue.x < 0)
        {
            _currentPresetID = (_currentPresetID - 1 + playerPresets.Count) % playerPresets.Count;
        }
        
        playerPresets[_currentPresetID].GetComponentInChildren<Button>().Select();
    }
    
    private void SetupPresets()
    {
        List<PlayerDefaultSetting> defaultPlayers = PlayerConfigurationManager.Instance.DefaultSettings;

        // Color + WeaponName
        for (int i = 0; i < playerPresets.Count; i++)
        {
            ChangeButtonColor(playerPresets[i].GetComponentInChildren<Button>(), defaultPlayers[i].defaultColor);
            playerPresets[i].GetComponentInChildren<TextMeshProUGUI>().SetText(defaultPlayers[i].DefaultWeapon.name);
        }
    }
    
    private void ChangeButtonColor(Button button, Color newColor)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = newColor;
        cb.highlightedColor = newColor;
        cb.pressedColor = Color.white;
        cb.selectedColor = new Color(newColor.r, newColor.g, newColor.b, 0.2f);
        button.colors = cb;
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
