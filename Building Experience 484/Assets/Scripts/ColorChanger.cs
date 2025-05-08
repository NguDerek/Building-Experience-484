using UnityEngine;
using UnityEngine.UI;


public class ColorChangerUI : MonoBehaviour
{
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

    void Start()
    {
        // Add listeners to call UpdateColor() whenever a slider is changed
        redSlider.onValueChanged.AddListener(delegate { UpdateColor(); });
        greenSlider.onValueChanged.AddListener(delegate { UpdateColor(); });
        blueSlider.onValueChanged.AddListener(delegate { UpdateColor(); });
    }

    void UpdateColor()
    {
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabbed = LastGrabbed.Instance?.LastGrabbedObject;
        if (grabbed == null) return;

        Renderer renderer = grabbed.GetComponent<Renderer>();
        if (renderer == null) return;

        Color newColor = new Color(redSlider.value, greenSlider.value, blueSlider.value);
        
        // You may want to use material instead of `sharedMaterial` if you're changing the instance's color
        renderer.material.color = newColor;
    }
}
