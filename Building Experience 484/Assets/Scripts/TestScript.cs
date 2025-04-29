using UnityEngine;
using UnityEngine.InputSystem;

public class TestInput : MonoBehaviour
{
    public InputActionReference duplicateAction;

    private void OnEnable()
    {
        duplicateAction.action.performed += ctx => Debug.Log("Test input triggered!");
        duplicateAction.action.Enable();
    }

    private void OnDisable()
    {
        duplicateAction.action.performed -= ctx => Debug.Log("Test input triggered!");
        duplicateAction.action.Disable();
    }
}

