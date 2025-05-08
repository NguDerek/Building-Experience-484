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
        if (grabbed == null){ 
            Debug.Log("No object grabbed");
            return;
            }

        Renderer renderer = grabbed.GetComponent<Renderer>();
        if (renderer == null) return;

        Debug.Log("R val: " + redSlider.value);
        Debug.Log("G val: " + greenSlider.value);
        Debug.Log("B val: " + blueSlider.value);
        Color newColor = new Color(redSlider.value, greenSlider.value, blueSlider.value);
        
        renderer.material.color = newColor;
    }
}
