using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneDirector : MonoBehaviour
{
    // Singleton instance
    public static SceneDirector Instance { get; private set; }

    // Enum to define different game states
    public enum GameState
    {
        TitleScreen,
        NewGame,
        CombatScreen,
        GameOver,
        MainMenu,
        PauseMenu,
        // Add other states as needed
    }

    // Current game state
    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to load a scene and update the game state
    public void LoadScene(string sceneName, GameState newState, System.Action onLoaded = null)
    {
        StartCoroutine(LoadSceneAsync(sceneName, newState, onLoaded));
    }

    // Coroutine to load the scene asynchronously and update the state
    private IEnumerator LoadSceneAsync(string sceneName, GameState newState, System.Action onLoaded)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Update the current game state after loading the scene
        CurrentState = newState;

        // Perform additional actions after loading the scene
        onLoaded?.Invoke();
    }
}
