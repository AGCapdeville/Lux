using UnityEngine;
using UnityEngine.UI;

public class ButtonGlow : MonoBehaviour
{
    // Start is called before the first frame update
    private Image myImage;
    private float start = 0f;
    private float end = 0f;

    private bool isFading = true;

    void Start()
    {
        Debug.Log(gameObject.GetComponent<Image>());
        myImage = GetComponent<Image>();
        Debug.Log(myImage.color);
        // myImage.color = Color.red;
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
}
