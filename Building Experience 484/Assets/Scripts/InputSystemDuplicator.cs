using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class InputSystemDuplicator : MonoBehaviour
{
    [Header("Input & References")]
    public InputActionReference duplicateAction; // Your "Duplicate" action
    public GameObject cubePrefab;               // The object to clone
    public Transform rayTransform;              // Usually your right-hand controller or ray origin
    
    [Header("Preview Settings")]
    public GameObject previewPrefab;             // Semi-transparent ghost cube

    private GameObject currentClone;
    private GameObject previewInstance;
    private bool isHolding = false;
    private void OnEnable()
    {
        duplicateAction.action.performed += OnDuplicatePressed;
        duplicateAction.action.Enable();
    }

    private void OnDisable()
    {
        duplicateAction.action.performed -= OnDuplicatePressed;
        duplicateAction.action.Disable();
    }

    private void Update()
    {
        UpdatePreviewCube();
    }
    private void OnDuplicatePressed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Duplicate button pressed!");

        if (!isHolding)
            AttachCubeToRay();
        else
            ReleaseCube();
    }

    private void AttachCubeToRay()
    {
    UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor = rayTransform.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
    Vector3 spawnPosition;
    Quaternion spawnRotation;

    if (rayInteractor != null && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
    {
        // Use hit point for cube position, with small offset to avoid clipping
        spawnPosition = hit.point + hit.normal * 0.05f;
        spawnRotation = Quaternion.LookRotation(-hit.normal); // Cube faces outward from the surface
        Debug.Log("Ray hit detected at: " + spawnPosition);
    }
    else
    {
        // If no hit, use the ray's forward direction to place the cube at a fixed distance
        spawnPosition = rayTransform.position + (-rayTransform.forward) * 4f; // Distance in front of ray
        spawnRotation = rayTransform.rotation; // Align cube rotation with the ray
        Debug.Log("No ray hit. Using fallback position at: " + spawnPosition);
    }

    // Instantiate the cube prefab at the calculated position and rotation
    currentClone = Instantiate(cubePrefab, spawnPosition, spawnRotation);

    // Attach cube to the ray controller (if desired)
    currentClone.transform.SetParent(rayTransform);

    // Set Rigidbody to kinematic to avoid it falling when held
    Rigidbody rb = currentClone.GetComponent<Rigidbody>();
    if (rb != null)
        rb.isKinematic = true;

    // Hide the preview while holding the cube
    if (previewInstance != null)
        previewInstance.SetActive(false);

    // Set holding flag to true
    isHolding = true;
    }

    private void ReleaseCube()
    {
        if (currentClone == null) return;

        Debug.Log("Releasing cube...");

        currentClone.transform.SetParent(null); // Detach from controller

        Rigidbody rb = currentClone.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;

        currentClone = null;
        isHolding = false;

        if (previewInstance != null)
            previewInstance.SetActive(true); // Show preview again
    }

    private void UpdatePreviewCube()
    {
        if(isHolding) return;

        UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor = rayTransform.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();

        if (rayInteractor != null && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (previewInstance == null)
            {
                previewInstance = Instantiate(previewPrefab);
            }

            previewInstance.SetActive(true);
            previewInstance.transform.position = hit.point + hit.normal * 0.05f;
            previewInstance.transform.rotation = Quaternion.identity;
        }
        else
        {
            if (previewInstance != null)
                previewInstance.SetActive(false);
        }
    }
}


