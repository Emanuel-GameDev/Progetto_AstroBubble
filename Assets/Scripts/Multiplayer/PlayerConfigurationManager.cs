using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// This script hold the list of player configuration.
/// It also registers the buttons in the UI in order to start the game.
/// 
/// </summary>
public class PlayerConfigurationManager : MonoBehaviour
{
    [SerializeField]
    private int maxPlayers = 2;
    
    private List<PlayerConfiguration> _playerConfigs;
    
    public static PlayerConfigurationManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            _playerConfigs = new List<PlayerConfiguration>();
        }
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Checks if all players pressed the "Ready" btn, signed as "Submit" in the UI action map
    /// </summary>
    /// <param name="index"></param>
    public void SetReadyPlayer(int index)
    {
        _playerConfigs[index].IsReady = true;

        if (_playerConfigs.Count == maxPlayers && _playerConfigs.All(p => p.IsReady))
        {
            SceneManager.LoadScene("MultiplayerGameScene");
        }
    }
    
    /// <summary>
    /// Called whenever a player joins the game.
    /// The behaviour for joining is set "btn pressed", can be changed in PlayerInputManager component
    /// </summary>
    /// <param name="input"></param>
    public void OnPlayerJoined(PlayerInput input)
    {
        Debug.Log("PlayerJoined " + input.playerIndex);

        if (_playerConfigs.All(p => p.PlayerIndex != input.playerIndex))
        {
            _playerConfigs.Add(new PlayerConfiguration(input));
            input.gameObject.transform.SetParent(transform);
        }
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return _playerConfigs;
    }
}

/// <summary>
/// struct that holds the player data
/// </summary>
public class PlayerConfiguration
{
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public string ControlScheme { get; set; }
    public bool IsReady { get; set; }

    public PlayerConfiguration(PlayerInput input)
    {
        Input = input;
        PlayerIndex = input.playerIndex;
        ControlScheme = input.currentControlScheme;
    }
}
