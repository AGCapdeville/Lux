using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor.SearchService;

public class ButtonGlow : MonoBehaviour
{
    // public Button myButton; // Reference to the Button component

    // Start is called before the first frame update
    private Image myImage;
    // private float start = 0f;
    // private float end = 0f;

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
        // myImage.color = Color.red;
    }


    private void OnEnable()
    {
        _inputActions.Enable();
        // _inputActions.Camera.RotateLeft.performed += RotateCameraLeft;
        _inputActions.UI.Select.performed += EnterGame;
    }

    private void OnDisable()
    {
        _inputActions.Disable();
        // _inputActions.Camera.RotateLeft.performed -= RotateCameraLeft;
        _inputActions.UI.Select.performed -= EnterGame;
    }




    // private float calcDistance(float start, float end) 
    // {
    //     return Float.Distance(transform.localPosition, end) / Vector3.Distance(start, end);
    // }
    // Update is called once per frame
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
        // Color.Lerp(Color.red, Color.clear, calcDistance(startFadeIn, endFadeIn));

    }


    // Doesn't seem to work TODO: FIX
    public void ClickEvent() {
        Debug.Log("Enter Game Through Click!");
    }

    private void EnterGame(InputAction.CallbackContext context)
    {
        Debug.Log("Enter Game!");
        _sceneDirector.LoadScene("Combat", SceneDirector.GameState.CombatScreen);


        // Make Button Flash, as though it was clicked
        // Fade Animation for transition to next screen.. Maybe handle that in the Scene Director

        // _targetOffset = Quaternion.Euler(0, -90f, 0) * _initialOffset;
        // float zoomFactor = _ZOOM / Mathf.Abs(_targetOffset.y);
        // _targetOffset = new Vector3(_targetOffset.x * zoomFactor, _ZOOM, _targetOffset.z * zoomFactor);

        // StartRotation();
    }

}