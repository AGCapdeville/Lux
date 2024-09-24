using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor.SearchService;

public class ButtonGlow : MonoBehaviour
{
    private Image myImage;

    private bool isFading = true;
    private PlayerInputActions _inputActions;
    private SceneDirector _sceneDirector;
    
    private void Awake()
    {
        _inputActions = InputManager.Instance.InputActions;
        _sceneDirector = SceneDirector.Instance;
    }

    void Start()
    {
        Debug.Log(gameObject.GetComponent<Image>());
        myImage = GetComponent<Image>();
        Debug.Log(myImage.color);
    }


    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.UI.Select.performed += EnterGame;
    }

    private void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.UI.Select.performed -= EnterGame;
    }

    void Update()
    {
        if (isFading) {
            myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, myImage.color.a - 0.001f);
        } else {
            myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, myImage.color.a + 0.001f);
        }

        if (myImage.color.a < 0.1f) {
            isFading = false;
        }

        if (myImage.color.a > 0.9f) {
            isFading = true;
        }
    }


    // Doesn't seem to work TODO: FIX
    public void ClickEvent() {
        Debug.Log("Enter Game Through Click!");
        _sceneDirector.LoadScene("Combat", SceneDirector.GameState.CombatScreen);
    }

    private void EnterGame(InputAction.CallbackContext context)
    {
        Debug.Log("Enter Game!");
        _sceneDirector.LoadScene("Combat", SceneDirector.GameState.CombatScreen);
    }

}