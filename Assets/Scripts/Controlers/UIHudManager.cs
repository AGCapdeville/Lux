using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHudManager : MonoBehaviour
{
    public static  MonoBehaviour _instance { get; private set; }
    private Canvas canvas { get; set; }
    private GameObject currentScreen;
    private GameObject HUD;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() {
        HUD = Instantiate(
            Resources.Load<GameObject>("Prefabs/UI"),
            Vector3.zero,
            Quaternion.identity
        );

        canvas = HUD.GetComponentInChildren<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        LoadUIIntoCanvas();
    }

    public void LoadUIIntoCanvas()
    {
        // Instantiate the prefab
        GameObject screenInstance = Instantiate(
            Resources.Load<GameObject>("Prefabs/TitleScreen"),
            Vector3.zero,
            Quaternion.identity
        );

        screenInstance.transform.SetParent(canvas.transform, false);

        // Iterate through each child of the instantiated prefab
        foreach (Transform child in screenInstance.transform)
        {

            Debug.Log(child.name);
            
            // Check if the child has a RectTransform (to ensure it's a UI element)
            RectTransform childRectTransform = child.GetComponent<RectTransform>();

            if (childRectTransform != null)
            {
                // Set the child as a child of the canvas
                child.SetParent(canvas.transform, false);

                // Optionally, adjust the child's RectTransform to ensure proper positioning
                childRectTransform.anchoredPosition = Vector2.zero;
                childRectTransform.localScale = Vector3.one;
            }

            // Check for TextMeshProUGUI in the child
            TextMeshPro textMesh = child.GetComponent<TextMeshPro>();
            Debug.Log(textMesh);

            if (textMesh != null)
            {
                Debug.Log("TextMeshPro found in: " + child.name);
                textMesh.text = "Updated Title"; // Set the text
            }

        }

        // Destroy the parent prefab after transferring children if you don't need it anymore
        Destroy(screenInstance);
    }

    // public void LoadUIIntoCanvas()
    // {
    //     // Instantiate the prefab
    //     GameObject screenInstance = Instantiate(
    //         Resources.Load<GameObject>("Prefabs/TitleScreen"),
    //         Vector3.zero,
    //         Quaternion.identity
    //     );

    //     screenInstance.transform.SetParent(canvas.transform, false);

    //     // Iterate through each child of the instantiated prefab
    //     foreach (Transform child in screenInstance.transform)
    //     {
    //         Debug.Log(child);
    //         // Check if the child has a RectTransform (to ensure it's a UI element)
    //         RectTransform childRectTransform = child.GetComponent<RectTransform>();

    //         if (childRectTransform != null)
    //         {
    //             // Set the child as a child of the canvas
    //             child.SetParent(canvas.transform, false);

    //             // Optionally, adjust the child's RectTransform to ensure proper positioning
    //             childRectTransform.anchoredPosition = Vector2.zero;
    //             childRectTransform.localScale = Vector3.one;
    //         }
    //     }

    //     // Destroy the parent prefab after transferring children if you don't need it anymore
    //     Destroy(screenInstance);
    // }

    public void ClearCanvas()
    {
        // Loop through all children of the Canvas and destroy them
        foreach (Transform child in canvas.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void LoadScreen(GameObject screenPrefab)
    {
        // Destroy the current screen if one is already loaded
        if (currentScreen != null)
        {
            Destroy(currentScreen);
        }

        // Instantiate the new screen and attach it to the canvas
        currentScreen = Instantiate(screenPrefab, canvas.transform);
        currentScreen.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }


    public void UnloadScreen()
    {
        // Destroy the current screen if it's active
        if (currentScreen != null)
        {
            Destroy(currentScreen);
        }
    }

}
