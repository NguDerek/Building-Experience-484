using UnityEngine;
using UnityEngine.UI;


public class ColorChangerUI : MonoBehaviour
{
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

    public LastGrabbed lastGrabbedManager;

    public void UpdateColor()
    {
        GameObject grabbed = lastGrabbedManager.LastGrabbedObject;
        if (grabbed == null)
        {
            Debug.LogWarning("No object is currently grabbed.");
            return;
        }

        MeshRenderer renderer = grabbed.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            Debug.LogWarning("No renderer for object.");
            return;
        }

        Color newColor = new Color(redSlider.value, greenSlider.value, blueSlider.value);

        Debug.Log("Hi!");
        Debug.Log(redSlider.value);
        Debug.Log(greenSlider.value);
        Debug.Log(blueSlider.value);
        
        // You may want to use material instead of `sharedMaterial` if you're changing the instance's color
        renderer.material.color = newColor;
    }
}
