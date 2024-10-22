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
            Resources.Load<GameObject>("Prefabs/TitleScreen")
        );

        // screenInstance.transform.SetParent(canvas.transform, false);

        List<GameObject> gameObjectList = new List<GameObject>();

        foreach (Transform child in screenInstance.transform)
        {
            Debug.Log(child.gameObject);
            gameObjectList.Add(child.gameObject);
        }

        // I don't know why but it works to loop over twice...
        foreach (GameObject go in gameObjectList) {
            
            RectTransform childRectTransform = go.GetComponent<RectTransform>();
            
            // Set the child as a child of the canvas
            go.transform.SetParent(canvas.transform, false);

            // Optionally, adjust the child's RectTransform to ensure proper positioning

            // need to change this...... so that the prefab title screen locations of ui stuff load correctly...
            childRectTransform.anchoredPosition = Vector2.zero;
            childRectTransform.localScale = Vector3.one;
            
        }

        Destroy(screenInstance);
    }

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
