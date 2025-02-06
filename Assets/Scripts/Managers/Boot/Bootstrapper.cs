using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    // static void Init()
    // {
    //     #if (UNITY_EDITOR)
    //     var currentlyLoadedScene = SceneManager.GetActiveScene();
    //     #endif
    //
    //     if (SceneManager.GetActiveScene().isLoaded != true)
    //         SceneManager.LoadScene("UIMultiplayerScene");
    //     
    //     #if (UNITY_EDITOR)
    //     if (currentlyLoadedScene.IsValid())
    //         SceneManager.LoadSceneAsync(currentlyLoadedScene.name, LoadSceneMode.Additive);
    //     #endif
    // }
}
