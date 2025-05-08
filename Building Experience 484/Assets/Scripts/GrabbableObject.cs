using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabbableObject : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private bool isFrozen = false;
    private bool isHeld = false;
    public LastGrabbed grabManager;

    private int defaultLayerMask;

    private void Awake() {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        defaultLayerMask = 1 << LayerMask.NameToLayer("Default");
    }

    private void Destroy() {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args) {
        grabManager.SetLastGrabbed(grabInteractable);

        isHeld = true;
    }

    private void OnRelease(SelectExitEventArgs args) {

        isHeld = false;

        if (isFrozen) {
            // Freeze object
            rb.isKinematic = true;
            rb.excludeLayers = defaultLayerMask;
        } else {
            rb.isKinematic = false;
            rb.excludeLayers = 0;
        }

    }

    private void Update()
    {
        if (isHeld) {
            // Check for primary button press
            if (Keyboard.current.bKey.wasPressedThisFrame) {
                isFrozen = !isFrozen;
            }
        }   
    }
}
