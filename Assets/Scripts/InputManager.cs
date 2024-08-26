using UnityEngine;
using UnityEngine.InputSystem;


// TODO: could run into issues when switching context of inputs/actions players will be using in different scenes.
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public PlayerInputActions InputActions { get; private set; }

    private void Awake()
    {
        // Ensure there's only one instance of InputManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize the PlayerInputActions
        InputActions = new PlayerInputActions();
        InputActions.Enable(); // Enable input actions
    }

    private void OnDestroy()
    {
        // Clean up and disable input actions when the object is destroyed
        InputActions.Disable();
    }
}
