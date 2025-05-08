using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ManualToggleGrabber : MonoBehaviour
{
    [Header("References")]
    public XRRayInteractor rayInteractor;                   // Your XRRayInteractor
    public XRInteractionManager interactionManager;         // Drag your Interaction Manager here

    [Header("Input")]
    public InputActionReference triggerAction;              // Trigger to toggle grab
    public InputActionReference freezeAction;               // Optional: A/B to toggle freeze

    private UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable selectedInteractable;
    private bool isHolding = false;
    private bool isFrozen = false;

    private void OnEnable()
    {
        triggerAction.action.performed += OnTriggerPressed;
        freezeAction.action.performed += OnFreezePressed;

        triggerAction.action.Enable();
        freezeAction.action.Enable();
    }

    private void OnDisable()
    {
        triggerAction.action.performed -= OnTriggerPressed;
        freezeAction.action.performed -= OnFreezePressed;

        triggerAction.action.Disable();
        freezeAction.action.Disable();
    }

    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        if (!isHolding)
            TryGrab();
        else
            Release();
    }

    private void OnFreezePressed(InputAction.CallbackContext context)
    {
        if (selectedInteractable is MonoBehaviour mb && isHolding)
        {
            Rigidbody rb = mb.GetComponent<Rigidbody>();
            isFrozen = !isFrozen;

            if (rb != null)
                rb.isKinematic = isFrozen;

            Debug.Log("Freeze toggled: " + isFrozen);
        }
    }

    private void TryGrab()
    {
        Debug.Log("Trigger pressed, trying to grab...");
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            var interactable = hit.collider.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable>();
            if (interactable != null)
            {
                interactionManager.SelectEnter(rayInteractor, interactable);
                selectedInteractable = interactable;
                isHolding = true;

                Rigidbody rb = (interactable as MonoBehaviour)?.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.isKinematic = true;

                Debug.Log("Grabbed: " + hit.collider.name);
            }
        }
    }

    private void Release()
    {
        if (selectedInteractable != null)
        {
            interactionManager.SelectExit(rayInteractor, selectedInteractable);

            Rigidbody rb = (selectedInteractable as MonoBehaviour)?.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = false;

            selectedInteractable = null;
            isHolding = false;
            isFrozen = false;

            Debug.Log("Released object.");
        }
    }
}

