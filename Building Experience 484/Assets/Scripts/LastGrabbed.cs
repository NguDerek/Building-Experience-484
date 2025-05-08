using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LastGrabbed : MonoBehaviour
{
    
    public static LastGrabbed Instance;
    public GameObject LastGrabbedObject;

    public void SetLastGrabbed(GameObject interactable)
    {
        LastGrabbedObject = interactable;
        Debug.Log("Last grabbed object: " + interactable.name);
    }

}
