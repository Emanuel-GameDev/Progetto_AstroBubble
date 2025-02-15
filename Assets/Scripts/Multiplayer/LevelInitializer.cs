using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// This script takes from the PlayerConfigurationManager the list of configurations
/// and spawns the players in the new scene
/// </summary>
public class LevelInitializer : MonoBehaviour
{
    [FormerlySerializedAs("spawnPoints")]
    [Header("SPAWN-POINTS")]
    [SerializeField]
    private Transform[] playerSpawnPoints;
    [SerializeField]
    private Transform bubbleSpawnPoint;
    
    [Header("PREFABS")]
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject bubblePrefab;

    private void Awake()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.PlayerConfigs.ToArray();

        for (int i = 0; i < playerConfigs.Length; i++)
        {
            // Instantiate playerPrefab
            var player = Instantiate(playerPrefab, playerSpawnPoints[i].position, playerSpawnPoints[i].rotation, gameObject.transform);
            // Initializing player by assigning the respective playerConfig
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);
        }
    }

    private void Start()
    {
        GameObject bubble = Instantiate(bubblePrefab, bubbleSpawnPoint.position, Quaternion.identity, gameObject.transform);
    }
}
