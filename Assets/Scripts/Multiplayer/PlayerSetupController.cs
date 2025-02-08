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

    [SerializeField] private List<Button> colorButtons;

    private PlayerInput _playerInput;
    private int _playerIndex;
    private InputAction _submitAction;
    private InputAction _navigationAction;
    private int _currentColorID;
    private Color _colorSelected; 
    
    public void SetPlayerInput(PlayerInput playerInput)
    {
        _playerInput = playerInput;
        
        _submitAction = _playerInput.actions.FindActionMap("UI").FindAction("Submit");
        _submitAction.performed += OnSubmit;
        _submitAction.Enable();
        
        _navigationAction = _playerInput.actions.FindActionMap("UI").FindAction("Navigate");
        _navigationAction.performed += OnNavigate;
        _navigationAction.Enable();
        
        _colorSelected = Color.white;
        SetPlayerTexts();
        SetupColors();
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
        if (_colorSelected == Color.white)
        {
            _colorSelected = colorButtons[_currentColorID].colors.normalColor;
            var config = PlayerConfigurationManager.Instance.GetPlayerConfig(_playerIndex);
            config.SetColor(_colorSelected);
            _navigationAction.Disable();
        }
        else
            ReadyPlayer();
    }

    private void OnNavigate(InputAction.CallbackContext obj)
    {
        if (!obj.performed || colorButtons.Count == 0) return;
        
        Vector2 inputValue = obj.ReadValue<Vector2>();
        if (inputValue.x > 0)
        {
            _currentColorID = (_currentColorID + 1) % colorButtons.Count;
        }
        else if (inputValue.x < 0)
        {
            _currentColorID = (_currentColorID - 1 + colorButtons.Count) % colorButtons.Count;
        }
        
        colorButtons[_currentColorID].Select();
    }

    private void SetupColors()
    {
        List<PlayerDefaultSetting> defaultPlayers = PlayerConfigurationManager.Instance.DefaultSettings;

        for (int i = 0; i < colorButtons.Count; i++)
        {
            ChangeButtonColor(colorButtons[i], defaultPlayers[i].DefaultColor);
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
