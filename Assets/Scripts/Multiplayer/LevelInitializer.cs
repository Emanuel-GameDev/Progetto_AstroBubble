using UnityEngine;

/// <summary>
/// This script takes from the PlayerConfigurationManager the list of configurations
/// and spawns the players in the new scene
/// </summary>
public class LevelInitializer : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;
    
    [SerializeField]
    private GameObject playerPrefab;

    private void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();

        for (int i = 0; i < playerConfigs.Length; i++)
        {
            // Instantiate playerPrefab
            var player = Instantiate(playerPrefab, spawnPoints[i].position, spawnPoints[i].rotation, gameObject.transform);
            // Initializing player by assigning the respective playerConfig
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);
        }
    }
}
